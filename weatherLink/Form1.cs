using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Net.Sockets;

namespace WeatherLink
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Timer aTimer = new Timer();
            aTimer.Interval = 5000;
            aTimer.Tick += new EventHandler(Telnet_Click);
            aTimer.Enabled = true;
            Console.ReadLine();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // This is how you output to a texbox from an event handler.  Event handlers run in a separate process
            // EventHandler eh = new EventHandler(delegate { this.textBox1.Text += temp; });
            // this.textBox1.Invoke(eh); 
        }

        private void Serial_Click(object sender, EventArgs e)
        {
            byte[] loopArray;
            SerialPort thePort;
            WeatherLoopData loopData;

            textBox1.Clear();

            thePort = Open_Serial_Port();

            if (thePort != null)
            {
                if (Wake_Serial_Vantage(thePort))
                {
                    loopData = new WeatherLoopData();
                    loopArray = Retrieve_Serial_Command(thePort, "LOOP 1", 95);
                    if (loopArray != null)
                    {
                        loopData.Load(loopArray);
                        Show_Message(loopData.DebugString());
                    }
                    else
                        Show_Message("Lost Connection to Vantage Weatherstation");
                }
                else
                    Show_Message("Cannot Connect to Vantage Weatherstation");

                thePort.Close();
            }
        }

        private void Telnet_Click(object sender, EventArgs e)
        {
            byte[] loopArray;
            TcpClient thePort;
            WeatherLoopData loopData;

            textBox1.Clear();

            thePort = Open_Telnet_Port();

            if (thePort != null)
            {
                if (Wake_Telnet_Vantage(thePort))
                {
                    loopData = new WeatherLoopData();
                    loopArray = Retrieve_Telnet_Command(thePort, "LOOP 1", 95);
                    if (loopArray != null)
                    {
                        loopData.Load(loopArray);
                        Show_Message(loopData.DebugString());
                    }
                    else
                        Show_Message("Lost Connection to Vantage Weatherstation");
                }
                else
                    Show_Message("Cannot Connect to Vantage Weatherstation");

                thePort.Close();
            }
        }

             
        // Open a TCP socket.  Most operations will work on the underlying stream from the port which aren't completely 
        // implemented in .Net 2.x
        private TcpClient Open_Telnet_Port()
        {
	
            string IP = BoxIP.Text;
            string porta =BoxPorta.Text;
            if ((IP == "") || (porta == ""))
                return null;
            try
            {
                // Creating the new TCP socket effectively opens it - secify IP address or domain name and port
                TcpClient sock = new TcpClient(IP,Convert.ToInt32(porta));
		
                // Set the timeout of the underlying stream
                // WARNING: several of the methods on the underlying stream object are not implemented in .Net 2.x
                sock.GetStream().ReadTimeout = 2500;

                return sock;
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return (null);
            }
        }

        // The Vantage weather station sleeps when it can to save power.  In order to get it to respond to commands, it 
        // needs to "wake up."  To wake it up, it needs to receive a '\n' (newline character).  It responds with a '\n\r'.
        // If no response arrives after 1.2 seconds (the max delay according to the Davis documentation), we try again.
        private bool Wake_Telnet_Vantage(TcpClient thePort)
        {
            byte newLineASCII = 10;
            int passCount = 1,
                maxPasses = 4;

            try
            {
                NetworkStream theStream = thePort.GetStream();

                // Put a newline character ('\n') out the serial port
                theStream.WriteByte(newLineASCII);
                // Wait .5 seconds to see if anything's been returned
                System.Threading.Thread.Sleep(500);
                // Now check and see if anything's been returned.  If nothing, ping the Vantage with another newline.
                while (!theStream.DataAvailable && passCount < maxPasses)
                {
                    theStream.WriteByte(newLineASCII);
                    // The Vantage documentation states that 1.2 seconds is the maximum delay - since we're looping
                    // anyway, let's try somthing more agressive
                    System.Threading.Thread.Sleep(500);
                    passCount += 1;
                }

                if (passCount < maxPasses)
                    return (true);
                else
                    return (false);
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return (false);
            }
        }

        // Retrieve_Command retrieves data from the Vantage weather station using the specified command
        private byte[] Retrieve_Telnet_Command(TcpClient thePort, string commandString, int returnLength)
        {
            bool Found_ACK = false;
            int currChar,
                ACK = 6,        // ASCII 6
                passCount = 1,
                maxPasses = 4;
            string termCommand;

            try
            {
                // Set a local variable so that it's easier to work with the stream underlying the TCP socket
                NetworkStream theStream = thePort.GetStream();

                // Try the command until we get a clean ACKnowledge from the Vantage.  We count the number of passes since
                // a timeout will never occur reading from the sockets buffer.  If we try a bunch of times (maxPasses) and
                // we get nothing back, we assume that the connection is busted
                while (!Found_ACK && passCount < maxPasses)
                {
                    termCommand = commandString + "\n";
                    // Convert the command string to an ASCII byte array - required for the .Write method - and send
                    theStream.Write(Encoding.ASCII.GetBytes(termCommand), 0, termCommand.Length);
                    // According to the Davis documentation, the LOOP command sends its response every 2 seconds.  It's
                    // not clear if there is a 2-second delay for the first response.  My trials have show that this can
                    // move faster, but still needs some delay.
                    System.Threading.Thread.Sleep(500);

                    // Wait for the Vantage to acknowledge the the receipt of the command - sometimes we get a '\r\n'
                    // in the buffer first or nor response is given.  If all else fails, try again.
                    while (theStream.DataAvailable && !Found_ACK)
                    {
                        // Read the current character
                        currChar = theStream.ReadByte();
                        if (currChar == ACK)
                            Found_ACK = true;
                    }

                    passCount += 1;
                }

                // We've tried a bunch of times and have heard nothing back from the port (nothing's in the buffer).  Let's 
                // bounce outta here
                if (passCount == maxPasses)
                    return (null);
                else
                {
                    // Allocate a byte array to hold the return data that we care about - up to, but not including the '\n'
                    // Size is determined by LOOP data return - this procedure has no way of knowing if it is not passed in.
                    byte[] loopString = new byte[returnLength];

                    // Wait until the buffer is full - we've received returnLength characters from the command response
                    while (thePort.Available <= loopString.Length)
                    {
                        // Wait a short period to let more data load into the buffer
                        System.Threading.Thread.Sleep(200);
                    }

                    // Read the first 95 bytes of the buffer into the array
                    theStream.Read(loopString, 0, returnLength);

                    return loopString;
                }
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return null;
            }
        }

        // Open the serial port for communication
        private SerialPort Open_Serial_Port()
        {
            try
            {
                SerialPort thePort = new SerialPort("COM1", 19200, Parity.None, 8, StopBits.One);

                // This establishes an event handler for serial comm errors
                thePort.ErrorReceived += new SerialErrorReceivedEventHandler(SerialPort_ErrorReceived);

                // Set a timeout just in case there's a big problem and nothing is being received.  The rest of the code should
                // take care of most problems.  The following line can be used if no timeout is desired:
                // thePort.ReadTimeout = SerialPort.InfiniteTimeout;
                thePort.ReadTimeout = 2500;
                thePort.WriteTimeout = 2500;

                // Set Data Terminal Ready to true - can't transmit without DTR turned on
                thePort.DtrEnable = true;

                thePort.Open();

                return (thePort);
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return (null);
            }
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs ex)
        {
            Show_Message(ex.ToString());
        }

        // The Vantage weather station sleeps when it can to save power.  In order to get it to respond to commands, it 
        // needs to "wake up."  To wake it up, it needs to receive a '\n' (newline character).  It responds with a '\n\r'.
        // If no response arrives after 1.2 seconds (the max delay according to the Davis documentation), we try again.
        private bool Wake_Serial_Vantage(SerialPort thePort)
        {
            int passCount = 1,
                maxPasses = 4;

            try
            {
                // Clear out both input and output buffers just in case something is in there already
                thePort.DiscardInBuffer();
                thePort.DiscardOutBuffer();

                // Put a newline character ('\n') out the serial port - the Writeline method terminates with a '\n' of its own
                thePort.WriteLine("");
                // Wait for 1.2 seconds - this is being very conservative - shorten if things look good
                System.Threading.Thread.Sleep(1200);
                // Now check and see if anything's been returned.  If nothing, ping the Vantage with another newline.
                while (thePort.BytesToRead == 0 && passCount < maxPasses)
                {
                    thePort.WriteLine("");
                    // The Vantage documentation states that 1.2 seconds is the maximum delay - wait for that amount of time
                    System.Threading.Thread.Sleep(1200);
                    passCount += 1;
                }

                // Vantage found and awakened
                if (passCount < maxPasses)
                {
                    // Now that the Vantage is awake, clean out the input buffer again for good measure.
                    thePort.DiscardInBuffer();
                    return (true);
                }
                else
                    return (false);
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return (false);
            }
        }

        // Retrieve_Command retrieves data from the Vantage weather station using the specified command
        private byte[] Retrieve_Serial_Command(SerialPort thePort, string commandString, int returnLength)
        {
            bool Found_ACK = false;
            int ACK = 6,        // ASCII 6
                passCount = 1,
                maxPasses = 4;
            int currChar;

            try
            {
                // Clean out the input (receive) buffer just in case something showed up in it
                thePort.DiscardInBuffer();
                // . . . and clean out the output buffer while we're at it for good measure
                thePort.DiscardOutBuffer();

                // Try the command until we get a clean ACKnowledge from the Vantage.  We count the number of passes since
                // a timeout will never occur reading from the sockets buffer.  If we try a bunch of times (maxPasses) and
                // we get nothing back, we assume that the connection is busted
                while (!Found_ACK && passCount < maxPasses)
                {
                    thePort.WriteLine(commandString);
                    // I'm using the LOOP command as the baseline here because many its parameters are a superset of
                    // those for other commands.  The most important part of this is that the LOOP command is iterative
                    // and the station waits 2 seconds between its responses.  Although it's not clear from the documentation, 
                    // I'm assuming that the first packet isn't sent for 2 seconds.  In any event, the conservative nature
                    // of waiting this amount of time probably makes sense to deal with serial IO in this manner anyway.
                    System.Threading.Thread.Sleep(2000);

                    // Wait for the Vantage to acknowledge the the receipt of the command - sometimes we get a '\n\r'
                    // in the buffer first or nor response is given.  If all else fails, try again.
                    while (thePort.BytesToRead > 0 && !Found_ACK)
                    {
                        // Read the current character
                        currChar = thePort.ReadChar();
                        if (currChar == ACK)
                            Found_ACK = true;
                    }

                    passCount += 1;
                }

                // We've tried a bunch of times and have heard nothing back from the port (nothing's in the buffer).  Let's 
                // bounce outta here
                if (passCount == maxPasses)
                    return (null);
                else
                {
                    // Allocate a byte array to hold the return data that we care about - up to, but not including the '\n'
                    // Size the array according to the data passed to the procedure
                    byte[] loopString = new byte[returnLength];

                    // Wait until the buffer is full - we've received returnLength characters from the LOOP response, 
                    // including the final '\n' 
                    while (thePort.BytesToRead <= loopString.Length)
                    {
                        // Wait a short period to let more data load into the buffer
                        System.Threading.Thread.Sleep(200);
                    }

                    // Read the first returnLength bytes of the buffer into the array
                    thePort.Read(loopString, 0, returnLength);

                    return loopString;
                }
            }
            catch (Exception ex)
            {
                Show_Message(ex.ToString());
                return null;
            }
        }

        private void Show_Message(string msgString)
        {
            textBox1.Text = msgString;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
