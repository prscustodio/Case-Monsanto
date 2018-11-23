using System;
using System.Text;

namespace WeatherLink
{
	/*
	 * The WeatherLinkData class extracts and stores the weather data from the array of bytes returned from the Vantage weather station
	 * The array is generated from the return of the LOOP command.
	 * Contents of the character array (LOOP packet from Vantage):
	 *
	 * Field                           Offset  Size    Explanation 
	 * "L"                             0       1 
	 * "O"                             1       1 
	 * "O"                             2       1       Spells out "LOO" for Rev B packets and "LOOP" for Rev A packets. Identifies a LOOP packet 
	 * "P" (Rev A), Bar Trend (Rev B)  3       1       Signed byte that indicates the current 3-hour barometer trend. It is one of these values: 
	 *                                                 -60 = Falling Rapidly  = 196 (as an unsigned byte) 
	 *                                                 -20 = Falling Slowly   = 236 (as an unsigned byte)   
	 *                                                 0 = Steady  
	 *                                                 20 = Rising Slowly  
	 *                                                 60 = Rising Rapidly  
	 *                                                 80 = ASCII "P" = Rev A firmware, no trend info is available. 
	 *                                                 Any other value means that the Vantage does not have the 3 hours of bar data needed 
	 *                                                    to determine the bar trend. 
	 * Packet Type                     4       1       Has the value zero. In the future we may define new LOOP packet formats and assign a different 
	 *                                                     value to this field. 
	 * Next Record                     5       2       Location in the archive memory where the next data packet will be written. This can be 
	 *                                                     monitored to detect when a new record is created. 
	 * Barometer                       7       2       Current Barometer. Units are (in Hg / 1000). The barometric value should be between 20 inches 
	 *                                                     and 32.5 inches in Vantage Pro and between 20 inches and 32.5 inches in both Vantatge Pro 
	 *                                                     Vantage Pro2.  Values outside these ranges will not be logged. 
	 * Inside Temperature              9       2       The value is sent as 10th of a degree in F.  For example, 795 is returned for 79.5�F. 
	 * Inside Humidity                 11      1       This is the relative humidity in %, such as 50 is returned for 50%. 
	 * Outside Temperature             12      2       The value is sent as 10th of a degree in F.  For example, 795 is returned for 79.5�F. 
	 * Wind Speed                      14      1       It is a byte unsigned value in mph.  If the wind speed is dashed because it lost synchronization 
	 *                                                     with the radio or due to some other reason, the wind speed is forced to be 0. 
	 * 10 Min Avg Wind Speed           15      1       It is a byte unsigned value in mph. 
	 * Wind Direction                  16      2       It is a two byte unsigned value from 0 to 360 degrees.  
	 *                                                     (0� is North, 90� is East, 180� is South and 270� is West.) 
	 * Extra Temperatures              18      7       This field supports seven extra temperature stations. Each byte is one extra temperature value 
	 *                                                     in whole degrees F with an offset of 90 degrees.  For example, a value of 0 = -90�F ; 
	 *                                                     a value of 100 = 10�F ; and a value of 169 = 79�F.
	 * Soil Temperatures               25      4       This field supports four soil temperature sensors, in the same format as the Extra Temperature 
	 *                                                     field above 
	 * Leaf Temperatures               29      4       This field supports four leaf temperature sensors, in the same format as the Extra Temperature 
	 *                                                     field above 
	 * Outside Humidity                33      1       This is the relative humitiy in %.  
	 * Extra Humidities                34      7       Relative humidity in % for extra seven humidity stations.  
	 * Rain Rate                       41      2       This value is sent as 100th of a inch per hour.  For example, 256 represent 2.56 inches/hour. 
	 * UV                              43      1       The unit is in UV index. 
	 * Solar Radiation                 44      2       The unit is in watt/meter2. 
	 * Storm Rain                      46      2       The storm is stored as 100th of an inch. 
	 * Start Date of current Storm     48      2       Bit 15 to bit 12 is the month, bit 11 to bit 7 is the day and bit 6 to bit 0 is the year offseted 
	 *                                                     by 2000. 
	 * Day Rain                        50      2       This value is sent as the 100th of an inch. 
	 * Month Rain                      52      2       This value is sent as the 100th of an inch. 
	 * Year Rain                       54      2       This value is sent as the 100th of an inch. 
	 * Day ET                          56      2       This value is sent as the 100th of an inch. 
	 * Month ET                        58      2       This value is sent as the 100th of an inch. 
	 * Year ET                         60      2       This value is sent as the 100th of an inch. 
	 * Soil Moistures                  62      4       The unit is in centibar.  It supports four soil sensors. 
	 * Leaf Wetnesses                  66      4       This is a scale number from 0 to 15 with 0 meaning very dry and 15 meaning very wet.  It supports 
	 *                                                     four leaf sensors. 
	 * Inside Alarms                   70      1       Currently active inside alarms. See the table below 
	 * Rain Alarms                     71      1       Currently active rain alarms. See the table below 
	 * Outside Alarms                  72      2       Currently active outside alarms. See the table below 
	 * Extra Temp/Hum Alarms           74      8       Currently active extra temp/hum alarms. See the table below 
	 * Soil & Leaf Alarms              82      4       Currently active soil/leaf alarms. See the table below 
	 * Transmitter Battery Status      86      1       
	 * Console Battery Voltage         87      2       Voltage = ((Data * 300)/512)/100.0 
	 * Forecast Icons                  89      1       
	 * Forecast Rule number            90      1  
	 * Time of Sunrise                 91      2       The time is stored as hour * 100 + min. 
	 * Time of Sunset                  93      2       The time is stored as hour * 100 + min. 
	 * "\n" <LF> = 0x0A                95      1  
	 * "\r" <CR> = 0x0D                96      1   
	 * CRC                             97      2  
	 * Total Length                    99 
	 */
	public class WeatherLinkData
	{
		private int BarometricTrend { get; set; }

		public int CurrentWindSpeed { get; private set; }

		private int AvgWindSpeed { get; set; }

		public int InternalHumidity { get; private set; }

		public int ExternalHumidity { get; private set; }

		public float Barometer { get; private set; }

		public float InternalTemperature { get; private set; }

		public float ExternalTemperature { get; private set; }

		public float DailyRain { get; private set; }

		public int WindDirection { get; private set; }

		private DateTime SunRise { get; set; } = DateTime.Now;

		private DateTime SunSet { get; set; } = DateTime.Now;

		/*
		 * FromByteArray - disassembles the byte array passed in and loads it into local data that the accessors can use.
		 * Actual data is in the format to the right of the assignments - I convert it to make it easier to use
		 * When bytes have to be assembled into 2-byte, 16-bit numbers, I convert two bytes from the array into 
		 * an Int16 (16-bit integer).  When a single byte is all that's needed, I just convert it to an Int32.
		 * In the end, all integers are cast to Int32 for return. 
		 */
		public void FromByteArray(byte[] dataByteArray)
		{
			BarometricTrend = Convert.ToInt32((sbyte) dataByteArray[3]); // Sbyte - signed byte
			Barometer = (float) BitConverter.ToInt16(dataByteArray, 7) / 1000; // Uint16
			InternalTemperature = (float) BitConverter.ToInt16(dataByteArray, 9) / 10; // Uint16
			InternalHumidity = Convert.ToInt32(dataByteArray[11]); // Byte - unsigned byte
			ExternalTemperature = (float) BitConverter.ToInt16(dataByteArray, 12) / 10; // Uint16
			ExternalHumidity = Convert.ToInt32(dataByteArray[33]); // Byte - unsigned byte
			WindDirection = BitConverter.ToInt16(dataByteArray, 16); // Uint16
			CurrentWindSpeed = Convert.ToInt32(dataByteArray[14]); // Byte - unsigned byte
			AvgWindSpeed = Convert.ToInt32(dataByteArray[15]); // Byte - unsigned byte
			DailyRain = (float) BitConverter.ToInt16(dataByteArray, 50) / 100; // Uint16

			DateTime currentTime = DateTime.Now;

			// Time from the Vatnage is all in 24-hour format. I move it into a string so I can manipulate it easily.
			string timeString = BitConverter.ToInt16(dataByteArray, 91).ToString();
			// Exract hours and minutes and convert them to integers - required by DateTime
			int hours = Convert.ToInt32(timeString.Substring(0, timeString.Length - 2));
			int minutes = Convert.ToInt32(timeString.Substring(timeString.Length - 2, 2));

			// Create a new Datetime instance using current year, month and day
			SunRise = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, 0);

			timeString = BitConverter.ToInt16(dataByteArray, 93).ToString(); // Uint16
			hours = Convert.ToInt32(timeString.Substring(0, timeString.Length - 2));
			minutes = Convert.ToInt32(timeString.Substring(timeString.Length - 2, 2));
			SunSet = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hours, minutes, 0);
		}

		private string GetWindDirectionText()
		{
			string windDirString;

			// The wind direction is given in degrees (0 - 359), so convert the direction to string representation
			if (WindDirection >= 337 || WindDirection <= 23) windDirString = "N";
			else if (WindDirection > 292) windDirString = "NW";
			else if (WindDirection >= 247) windDirString = "W";
			else if (WindDirection > 203) windDirString = "SW";
			else if (WindDirection >= 157) windDirString = "S";
			else if (WindDirection > 113) windDirString = "SE";
			else if (WindDirection >= 67) windDirString = "E";
			else if (WindDirection > 23) windDirString = "NE";
			else windDirString = "Undetermined";

			return windDirString;
		}

		private string GetBarTrendText()
		{
			// The barometric trend is in signed integer values.  Convert these to something meaningful.
			switch (BarometricTrend)
			{
				case -60:
					return "Falling Rapidly";
				case -20:
					return "Falling Slowly";
				case 0:
					return "Steady";
				case 20:
					return "Rising Slowly";
				case 60:
					return "Rising Rapidly";
				default:
					return "No Barometric Trend Available";
			}
		}

		// Displays WeatherLink data formatted
		public string Pretty()
		{
			var outputString = new StringBuilder();

			outputString.Append("Barometer: ").Append(Barometer.ToString("f2")).Append("in. ").Append(GetBarTrendText())
				.Append(Environment.NewLine);
			outputString.Append("Internal Temperature: ").Append(InternalTemperature).Append(Environment.NewLine);
			outputString.Append("Internal Humidity: ").Append(InternalHumidity).Append("%").Append(Environment.NewLine);
			outputString.Append("External Temperature: ").Append(ExternalTemperature).Append(Environment.NewLine);
			outputString.Append("External Humidity: ").Append(ExternalHumidity).Append("%").Append(Environment.NewLine);
			outputString.Append("Wind Direction: ").Append(GetWindDirectionText()).Append(" @ ").Append(WindDirection).Append(" degrees").Append(Environment.NewLine);
			outputString.Append("Current Wind Speed: ").Append(CurrentWindSpeed).Append("MPH").Append(Environment.NewLine);
            outputString.Append("10 Minute Average Wind Speed: ").Append(AvgWindSpeed).Append("MPH");.Append(Environment.NewLine);
			outputString.Append("Daily Rain: ").Append(DailyRain).Append("in").Append(Environment.NewLine);
			outputString.Append("Sunrise: ").Append(SunRise.ToString("t")).Append(Environment.NewLine);
			outputString.Append("Sunset: ").Append(SunSet.ToString("t")).Append(Environment.NewLine);

			return outputString.ToString();
		}
	}
}