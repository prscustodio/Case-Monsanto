using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace WeatherLink
{
	public partial class Form1 : Form
	{
		private Timer _timer;
		private TcpClient _weatherLinkSocket;
		private string _configFile;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			FilePathTb.Text = @"U:\Utilidades\Estação Meteorológica\Base de Dados\Weatherlink6.0.3\WeatherS\MonsantoWeatherStation";
			
			// TODO: Verificar se realmente precisa escapar aqui
			_configFile = FilePathTb.Text.Replace(@"\", @"\\");

			string interval = File.ReadLines(_configFile).Skip(2).Take(1).First();
			_timer = new Timer {Interval = Convert.ToInt32(interval)};
			_timer.Tick += UpdateInterface;
			_timer.Start();

			// Make sure the the program wont exit
			Console.ReadLine();
		}

		private void UpdateInterface(object sender, EventArgs e)
		{
			int newInterval = Convert.ToInt32(File.ReadLines(_configFile).Skip(2).Take(1).First());

			if (_timer.Interval != newInterval) _timer.Interval = newInterval;

			using (TcpClient socket = CreateSocketConnection())
			{
				if (socket == null) return;

				_weatherLinkSocket = socket;

				if (WakeWeatherLink())
				{
					byte[] response = GetWeatherLinkResponse("LOOP 1", 95);
					if (response == null)
					{
						ShowMessage(@"Lost connection with WeatherLink");
						return;
					}

					var weatherLinkData = new WeatherLinkData();
					weatherLinkData.FromByteArray(response);

					LogTb.Text = weatherLinkData.Pretty();

					ShowMessage(SaveWeatherLinkData(weatherLinkData));
				}
				else ShowMessage(@"Unable to connect to WeatherLink");
			}
		}

		private string SaveWeatherLinkData(WeatherLinkData weatherLinkData)
		{
			string pressure = weatherLinkData.Barometer.ToString("f2");
			float inTemperature = weatherLinkData.InternalTemperature;
			int inHumidity = weatherLinkData.InternalHumidity;
			float outTemperature = weatherLinkData.ExternalTemperature;
			int outHumidity = weatherLinkData.ExternalHumidity;
			int windDirection = weatherLinkData.WindDirection;
			int windSpeed = weatherLinkData.CurrentWindSpeed;
			float dailyRain = weatherLinkData.DailyRain;

			var dataBuilder = new StringBuilder();

			if (!File.ReadLines(_configFile).Any())
			{
				dataBuilder.Append("Pressão").Append(",");
				dataBuilder.Append("Temperatura Interna").Append(",");
				dataBuilder.Append("Umidade Interna").Append(",");
				dataBuilder.Append("Temperatura Externa").Append(",");
				dataBuilder.Append("Umidade Externa").Append(",");
				dataBuilder.Append("Direção do Vento").Append(",");
				dataBuilder.Append("Velocidade do Vento").Append(",");
				dataBuilder.Append("Chuva Diária").Append(Environment.NewLine);
			}

			dataBuilder.Append(pressure).Append(",");
			dataBuilder.Append(inTemperature).Append(",");
			dataBuilder.Append(inHumidity).Append(",");
			dataBuilder.Append(outTemperature).Append(",");
			dataBuilder.Append(outHumidity).Append(",");
			dataBuilder.Append(windDirection).Append(",");
			dataBuilder.Append(windSpeed).Append(",");
			dataBuilder.Append(dailyRain).Append(Environment.NewLine);

			string data = dataBuilder.ToString();

			string csvFilePath = File.ReadLines(_configFile).Last();
			File.AppendAllText(csvFilePath, data);

			return data;
		}


		private TcpClient CreateSocketConnection()
		{
			const int timeout = 2500;

			try
			{
				string hostname = File.ReadLines(_configFile).First();
				int port = Convert.ToInt32(File.ReadLines(_configFile).Skip(1).Take(1).First());

				var socket = new TcpClient(hostname, port);
				socket.GetStream().ReadTimeout = timeout;

				return socket;
			}
			catch (Exception e)
			{
				ShowMessage($@"Failed to create a connection with WeatherLink after {timeout}ms: " + e.Message);
				return null;
			}
		}

		/*
		 * The WeatherLink sleeps to save power. In order to get it to respond the requests, we need to wake it up.
		 * To wake it up, we need to send a newline character ('\n').
		 * It will respond with a '\n\r'.
		 * If no response arrives after 1.2 seconds (max delay according to documentation), we'll try again.
		 */
		private bool WakeWeatherLink()
		{
			// ASCII 10
			const byte newLineChar = 10;
			const int maxNumberAttempts = 3;


			NetworkStream communicationStream = _weatherLinkSocket.GetStream();
			var currentAttempt = 0;
			do
			{
				// Send '\n'
				communicationStream.WriteByte(newLineChar);

				// Give WeatherLink time to respond
				Thread.Sleep(1200);
			} while (!communicationStream.DataAvailable && currentAttempt++ <= maxNumberAttempts - 1);

			// If didn't tried all attempts, it means WeatherLink responded
			return currentAttempt < maxNumberAttempts;
		}

		// Retrieves data from the WeatherLink using the requestCommand
		private byte[] GetWeatherLinkResponse(string requestCommand, int returnLength)
		{
			// ASCII 6
			const int ackChar = 6;
			const int maxNumberAttempts = 3;

			var hasFoundAck = false;
			var currentAttempt = 0;

			NetworkStream communicationStream = _weatherLinkSocket.GetStream();

			/*
			 * Try the requestCommand until we get a clean ACKnowledge from WeatherLink. If we don't get nothing, we assume
			 * the connection is busted
			 */
			do
			{
				requestCommand += "\n";
				communicationStream.Write(Encoding.ASCII.GetBytes(requestCommand), 0, requestCommand.Length);

				/*
				 * According to the documentation, the LOOP requestCommand sends its response every 2 seconds. My trials have
				 * show that this can move faster, but still needs some delay.
				 */
				Thread.Sleep(500);

				/*
				 * Wait for the WeatherLink to acknowledge the request, because sometimes we get a
				 * '\r\n' in the buffer first or nor response is given. If all else fails, try again.
				 */
				while (communicationStream.DataAvailable && !hasFoundAck)
				{
					// Read the current character
					int currentChar = communicationStream.ReadByte();
					if (currentChar == ackChar) hasFoundAck = true;
				}
			} while (!hasFoundAck && currentAttempt++ <= maxNumberAttempts - 1);

			// WeatherLink didn't acknowledge the request 
			if (currentAttempt == maxNumberAttempts) return null;

			/*
			 * Allocate a byte array to hold the return data that we care about up to, but not including the '\n'.
			 * Size is determined by LOOP data return.
			 * This procedure has no way of knowing if it is not passed in.
			 */
			var loopDataReturn = new byte[returnLength];

			// Wait until the buffer is full (we've received 'returnLength' characters from the 'requestCommand' response)
			while (_weatherLinkSocket.Available <= loopDataReturn.Length)
			{
				// Wait a short period to let more data load into the buffer
				Thread.Sleep(200);
			}

			// Read the first 'returnLength' bytes of the buffer into the array
			communicationStream.Read(loopDataReturn, 0, returnLength);

			return loopDataReturn;
		}

		/*
		 * Update interface outside main thread
		 */
		private void ShowMessage(string message)
		{
			void ShowMessageFromThread(object sender, EventArgs e)
			{
				LogTb.Text = message;
			}

			LogTb.Invoke((EventHandler) ShowMessageFromThread);
		}

        private void FilePathLbl_Click(object sender, EventArgs e)
        {

        }

        private void LogTb_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}