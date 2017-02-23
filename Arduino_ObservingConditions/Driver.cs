// --------------------------------------------------------------------------------
//
// ASCOM ObservingConditions driver for Arduino
//
// Description:	A weather station built around Arduino and Weather shield.  
//				Based on Arduino Uno, Sparkfun weather shield https://www.sparkfun.com/products/12081
//              and Sparkfun weather meters: https://www.sparkfun.com/products/8942
//              Also using Melexis 90614 IR Sensor for sky temperature and cloud detection
//				The Arduino sketch is available from the same git repo. And also a detailed build report
//
// Implements:	ASCOM ObservingConditions interface version: 1.0.0
// Author:		Manoj Koushik (manoj.koushik@gmail.com
//
// --------------------------------------------------------------------------------


#define ObservingConditions

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;

using ASCOM;
using ASCOM.Astrometry;
using ASCOM.Astrometry.AstroUtils;
using ASCOM.Utilities;
using ASCOM.DeviceInterface;
using System.Globalization;
using System.Collections;
using System.Threading;
using System.IO;
using System.IO.Ports;

namespace ASCOM.Arduino
{
    //
    // Your driver's DeviceID is ASCOM.Arduino.ObservingConditions
    //
    // The Guid attribute sets the CLSID for ASCOM.Arduino.ObservingConditions
    // The ClassInterface/None addribute prevents an empty interface called
    // _Arduino from being created and used as the [default] interface
    //

    /// <summary>
    /// ASCOM ObservingConditions Driver for Arduino.
    /// </summary>
    [Guid("34e3f7d0-907c-4900-9ac3-5f172239c50c")]
    [ClassInterface(ClassInterfaceType.None)]
    public class ObservingConditions : IObservingConditions
    {
        /// <summary>
        /// ASCOM DeviceID (COM ProgID) for this driver.
        /// The DeviceID is used by ASCOM applications to load the driver at runtime.
        /// </summary>
        internal static string driverID = "ASCOM.Arduino.ObservingConditions";
        // TODO Change the descriptive string for your driver then remove this line
        /// <summary>
        /// Driver description that displays in the ASCOM Chooser.
        /// </summary>
        private static string driverDescription = "Arduino ObservingConditions";

        // Variables to hold the currrent device configuration
        internal static string comPortProfileName = "COM Port"; // COM Port
        internal static string comPortDefault = "COM1";
        internal static string comPort;

        internal static string traceStateProfileName = "Trace Level"; // Trace Level
        internal static string traceStateDefault = "false";

        internal static int RECEIVE_TIMEOUT = 30;

        internal static string updateIntervalProfileName = "updateInterval"; // How often should we update sensors?
        internal static int updateIntervalDefault = 30;
        internal static int updateInterval;

        internal static string clearSkiesProfileName = "clearSkies"; // When is it clear?
        internal static float clearSkiesDefault = 25;
        internal static float clearSkies;

        internal static string cloudySkiesProfileName = "cloudySkies"; // when is it cloudy?
        internal static float cloudySkiesDefault = 10;
        internal static float cloudySkies;

        internal static string cloudyCondProfileName = "cloudyCond"; // What defines a Cloudy Condition?
        internal static int cloudyCondDefault = 50;
        internal static int cloudyCond;

        internal static string veryCloudyCondProfileName = "veryCloudyCond"; // What defines a Very Cloudy Condition?
        internal static int veryCloudyCondDefault = 80;
        internal static int veryCloudyCond;

        internal static float nightVol;
        internal static string nightVolProfileName = "nightVoltage"; // when is it dark?
        internal static float nightVolDefault = 0;

        internal static float dayVol;
        internal static string dayVolProfileName = "dayVoltage"; // when is it day?
        internal static float dayVolDefault = 5;

        internal static int nightLux;
        internal static string nightLuxProfileName = "nightLux"; // when is it dark?
        internal static int nightLuxDefault = 0;

        internal static int dayLux;
        internal static string dayLuxProfileName = "dayLux"; // when is it day?
        internal static int dayLuxDefault = 12000;

        internal static int twilightCond;
        internal static string twilightCondProfileName = "twilightCond"; // what defines a light condition
        internal static int twilightCondDefault = 200;

        internal static int daylightCond;
        internal static string daylightCondProfileName = "daylightCond"; // what defines a very light condition
        internal static int daylightCondDefault = 10000;

        internal static float windyCond;
        internal static string windyCondProfileName = "windyCond"; // What defines a windy condition
        internal static float windyCondDefault = 4.2F;

        internal static float veryWindyCond;
        internal static string veryWindyCondProfileName = "veryWindyCond"; // what defines a very windy condition
        internal static float veryWindyCondDefault = 8.4F;

        internal static string bwf;
        internal static string bwfProfileName = "bwf"; // Default boltwood file location
        internal static string bwfDefault = "C:/ArduinoBWF.txt";

        internal static bool bwfEnabled;
        internal static string bwfEnabledProfileName = "bwfEnabled"; // Default boltwood file location
        internal static bool bwfEnabledDefault = true;

        private SerialPort arduino;

        // Variables to hold all the weather measurements. This is updated from the worker thread every so often
        // and also dumped into the boltwood file. 
        // The individual methods only return these variables and don't query Arduino
        double cloudCover = 0;
        double dewPoint = 0;
        double humidity = 0;
        double pressure = 0;
        double rainRate = 0;
        double skyBrightness = 0;
        double skyTemperature = 0;
        double temperature = 0;
        double windSpeed = 0;
        double windDirection = 0;
        double windGust = 0;
        DateTime lastRainIncident;
        DateTime lastCondIncident;
        string rainDetect = "0";
        string condensationDetect = "0";

        private Thread arduinoThread;
        private bool arduinoThreadRunning = false;

        /// <summary>
        /// Private variable to hold the connected state
        /// </summary>
        private bool connectedState;

        /// <summary>
        /// Private variable to hold an ASCOM Utilities object
        /// </summary>
        private Util utilities;

        /// <summary>
        /// Private variable to hold an ASCOM AstroUtilities object to provide the Range method
        /// </summary>
        private AstroUtils astroUtilities;

        /// <summary>
        /// Variable to hold the trace logger object (creates a diagnostic log file with information that you specify)
        /// </summary>
        internal static TraceLogger tl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Arduino"/> class.
        /// Must be public for COM registration.
        /// </summary>
        public ObservingConditions()
        {
            tl = new TraceLogger("", "Arduino");
            ReadProfile(); // Read device configuration from the ASCOM Profile store

            tl.LogMessage("ObservingConditions", "Starting initialisation");

            connectedState = false; // Initialise connected to false
            utilities = new Util(); //Initialise util object
            astroUtilities = new AstroUtils(); // Initialise astro utilities object

            tl.LogMessage("ObservingConditions", "Completed initialisation");
        }


        //
        // PUBLIC COM INTERFACE IObservingConditions IMPLEMENTATION
        //

        #region Common properties and methods.

        /// <summary>
        /// Displays the Setup Dialog form.
        /// If the user clicks the OK button to dismiss the form, then
        /// the new settings are saved, otherwise the old values are reloaded.
        /// THIS IS THE ONLY PLACE WHERE SHOWING USER INTERFACE IS ALLOWED!
        /// </summary>
        public void SetupDialog()
        {
            // consider only showing the setup dialog if not connected
            // or call a different dialog if connected
            if (IsConnected)
                System.Windows.Forms.MessageBox.Show("Already connected, just press OK");

            using (SetupDialogForm F = new SetupDialogForm())
            {
                var result = F.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    WriteProfile(); // Persist device configuration values to the ASCOM Profile store
                }
            }
        }

        public ArrayList SupportedActions
        {
            get
            {
                tl.LogMessage("SupportedActions Get", "Returning list...");
                ArrayList supportedActions = new ArrayList();

                supportedActions.Add("Humidity");
                supportedActions.Add("Pressure");
                supportedActions.Add("CloudCover");
                supportedActions.Add("DewPoint");
                supportedActions.Add("RainRate");
                supportedActions.Add("SkyBrightness");
                supportedActions.Add("SkyTemperature");
                supportedActions.Add("Temperature");
                supportedActions.Add("WindGust");
                supportedActions.Add("WindSpeed");
                supportedActions.Add("WindDirection");

                return supportedActions;
            }
        }

        // Query Weather Station sensors directly 
        public string Action(string actionName, string actionParameters)
        {
            LogMessage("Action", "ActionName {0}, ActionParameters {1} called", actionName, actionParameters);

            string deviceType = actionName.Substring(0, actionName.IndexOf(':'));
            string action = actionName.Substring(actionName.IndexOf(':') + 1);

            if (!deviceType.Equals("ArduinoWeatherStation", StringComparison.OrdinalIgnoreCase))
            {
                return "";
            }

            if (action.Equals("Humidity", StringComparison.OrdinalIgnoreCase))
            {
                if (actionParameters.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("HumidityValue", false);
                } else if (actionParameters.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("HumidityDesciption", false);
                }
                else
                {
                    return "";
                }
            } else if (action.Equals("Pressure", StringComparison.OrdinalIgnoreCase))
            {
                if (actionParameters.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("PressureValue", false);
                }
                else if (actionParameters.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("PressureDesciption", false);
                }
                else
                {
                    return "";
                }
            }
            else if (action.Equals("SkyBrightness", StringComparison.OrdinalIgnoreCase))
            {
                if (actionParameters.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("SkyBrightnessValue", false);
                }
                else if (actionParameters.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("SkyBrightnessDesciption", false);
                }
                else
                {
                    return "";
                }
            }
            else if (action.Equals("Temperature", StringComparison.OrdinalIgnoreCase))
            {
                if (actionParameters.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("TemperatureValue", false);
                }
                else if (actionParameters.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("TemperatureDesciption", false);
                }
                else
                {
                    return "";
                }
            }
            else if (action.Equals("SkyTemperature", StringComparison.OrdinalIgnoreCase))
            {
                if (actionParameters.Equals("Value", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("SkyTemperatureValue", false);
                }
                else if (actionParameters.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    return CommandString("SkyTemperatureDesciption", false);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public void boltWoodFile()
        {
            arduinoThreadRunning = true;
            DateTime timeStamp = DateTime.Now;

            while (this.Connected)
            {
                DateTime lastUpdate = timeStamp;
                
                LogMessage("BoltWoodFile", "get TimeStamp");
                timeStamp = DateTime.Now;

                LogMessage("BoltWoodFile", "get Humidity");
                string hum = CommandString("H", false);
                humidity = double.Parse(hum);

                LogMessage("BoltWoodFile", "get Pressure");
                pressure = double.Parse(CommandString("P", false));
                // Pressure comes in from sensor as Pa. We need to convert to hPa
                pressure /= 100;

                LogMessage("BoltWoodFile", "get Temperature");
                temperature = double.Parse(CommandString("T", false));

                LogMessage("BoltWoodFile", "get SkyTemperature");
                skyTemperature = double.Parse(CommandString("ST", false));

                // This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.
                // Rainfall intensity is classified according to the rate of precipitation:
                // Light rain — when the precipitation rate is < 2.5 mm (0.098 in) per hour
                // Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) - 7.6 mm (0.30 in) or 10 mm (0.39 in) per hour
                // Heavy rain — when the precipitation rate is > 7.6 mm (0.30 in) per hour, or between 10 mm (0.39 in) and 50 mm (2.0 in) per hour
                // Violent rain — when the precipitation rate is > 50 mm (2.0 in) per hour
                LogMessage("BoltWoodFile", "get RainRate");
                rainRate = double.Parse(CommandString("RR", false));
                // Sensor reports inches/hour. We need to convert to mm/hour
                rainRate *= 25.4;

                LogMessage("BoltWoodFile", "get WindSpeed");
                windSpeed = double.Parse(CommandString("WS", false));
                // Sensor reports MPH. We need to convert to m/s
                windSpeed *= 0.44704;

                LogMessage("BoltWoodFile", "get WindGust");
                windGust = double.Parse(CommandString("WG", false));
                // Sensor reports MPH. We need to convert to m/s
                windGust *= 0.44704;

                LogMessage("BoltWoodFile", "get WindDirection");
                windDirection = double.Parse(CommandString("WD", false));

               
                LogMessage("BoltWoodFile", "get RainDetect");
                rainDetect = CommandString("RD", false);
              

                if (rainDetect.Equals("1"))
                    lastRainIncident = DateTime.Now;

                LogMessage("BoltWoodFile", "get CondensationDetect");
                condensationDetect = CommandString("CD", false);

                if (condensationDetect.Equals("1"))
                    lastCondIncident = DateTime.Now;

                // Sky brightness (Ascom needs Lux, but arduino is returning voltage. Calibration settings are used to convert
                // 0.0001 lux  Moonless, overcast night sky (starlight)
                // 0.002 lux Moonless clear night sky with airglow
                // 0.27–1.0 lux  Full moon on a clear night
                // 3.4 lux Dark limit of civil twilight under a clear sky
                // 50 lux  Family living room lights (Australia, 1998)
                // 80 lux  Office building hallway/toilet lighting
                // 100 lux Very dark overcast day
                // 320–500 lux Office lighting
                // 400 lux Sunrise or sunset on a clear day.
                // 1000 lux  Overcast day; typical TV studio lighting
                // 10000–25000 lux Full daylight (not direct sun)
                // 32000–100000 lux  Direct sunlight
                LogMessage("BoltWoodFile", "get SkyBrightness");
                float lightSlope = (dayLux - nightLux) / (dayVol - nightVol);
                skyBrightness = (int)(lightSlope * double.Parse(CommandString("SB", false)));

                LogMessage("BoltWoodFile", "calc CloudCover");
                double cloudSlope = (0-100)/(clearSkies - cloudySkies);
                double cloudIntercept = 100 - (cloudySkies * cloudSlope);
                cloudCover = cloudIntercept + (cloudSlope * (temperature - skyTemperature));
                if (cloudCover > 100) cloudCover = 100;
                if (cloudCover < 0) cloudCover = 0;

                LogMessage("BoltWoodFile", "calc DewPoint");
                dewPoint = 243.04 * (Math.Log(humidity / 100) + ((17.625 * temperature) / (243.04 + temperature))) / (17.625 - Math.Log(humidity / 100) - ((17.625 * temperature) / (243.04 + temperature)));

                // Date       Time        T V SkyT  AmbT SenT Wind Hum DewPt Hea
                // 2005-06-03 02:07:23.34 C K -28.5 18.7 22.5 45.3 75  10.3  3
                // R W Since Now()        c w r d C A
                // 0 0 00004 038506.08846 1 2 1 0 0 0
                // The fields mean:
                // Heading Col’s Meaning
                // Date    1-10  local date yyyy - mm - dd
                // Time    12-22 local time hh: mm: ss.ss(24 hour clock)
                // T       24    temperature units displayed and in this data, 'C' for Celsius or 'F' for Fahrenheit
                // V       26    wind velocity units displayed and in this data, ‘K’ for km / hr or ‘M’ for mph or'm' for m/s
                // SkyT    28-33 sky - ambient temperature, 999. for saturated hot, -999. for saturated cold, or –998. for wet
                // AmbT    35-40 ambient temperature
                // SenT    41-47 sensor case temperature, 999. for saturated hot, -999. for saturated cold. Neither
                //               saturated condition should ever occur.
                // Wind    49-54 wind speed or:
                //               -1. if still heating up,
                //               -2. if wet,
                //               -3. if the A / D from the wind probe is bad (firmware < V56 only) ,
                //               -4. if the probe is not heating(a failure condition),
                //               -5. if the A / D from the wind probe is low (shorted, a failure condition),
                //               -6. if the A / D from the wind probe is high (no probe plugged in or a failure)
                // Hum     56-58 relative humidity in %
                // DewPt   60-65 dew point temperature
                // Hea     67-69 heater setting in %
                // R       71    rain flag, = 0 for dry, = 1 for rain in the last minute, = 2 for rain right now
                // W       73    wet flag, = 0 for dry, = 1 for wet in the last minute, = 2 for wet right now
                // Since   75-79 seconds since the last valid data
                // Now()   81-92 date/time given as the VB6 Now() function result (in days) when Clarity II last
                //                wrote this file
                // c       94    cloud condition(see the Cloudcond enum in section 20) 
                // w       96    wind condition (see the Windcond enum in section 20)
                // r       98    rain condition(see the Raincond enum in section 20)
                // d       100   daylight condition(see the Daycond enum in section 20)
                // C       102   roof close, = 0 not requested, = 1 if roof close was requested on this cycle
                // A       104   alert, = 0 when not alerting, = 1 when alerting

                //Date and Time
                if (bwfEnabled)
                {
                    string boltwoodLine = timeStamp.ToString("yyyy-MM-dd HH:mm:ss.ff")
                        // Temperature and velocity units
                        + " C m "
                        // skyTemperature
                        + skyTemperature.ToString("000.00;-00.00;000.00") + " "
                        // ambient temperature
                        + temperature.ToString("000.00;-00.00;000.00") + " "
                        // case temperature
                        + " 000000 "
                        // wind speed
                        + windSpeed.ToString("000.00;-00.00;000.00") + " "
                        // humidity
                        + humidity.ToString("000") + " "
                        // dewpoint
                        + dewPoint.ToString("000.00;-00.00;000.00") + " "
                        // heater setting
                        + " 000 ";

                    // Public Enum CloudCond
                    // cloudUnknown = 0
                    // cloudClear = 1
                    // cloudCloudy = 2
                    // cloudVeryCloudy = 3
                    //End Enum
                    string cloudCond = "1";
                    if (cloudCover > cloudyCond)
                        cloudCond = "2";
                    if (cloudCover > veryCloudyCond)
                        cloudCond = "3";

                    //Public Enum WindCond
                    // windUnknown = 0
                    // windCalm = 1
                    // windWindy = 2
                    // windVeryWindy = 3
                    //End Enum
                    string windCond = "1";
                    if (windSpeed > windyCond)
                        windCond = "2";
                    if (windSpeed > veryWindyCond)
                        windCond = "3";

                    //Public Enum DayCond
                    // dayUnknown = 0
                    // 'Below are based upon thresholds set in the setup window.
                    // dayDark = 1
                    // dayLight = 2
                    // dayVeryLight = 3
                    //End Enum
                    string lightCond = "1";
                    if (skyBrightness > twilightCond)
                        lightCond = "2";
                    if (skyBrightness > daylightCond)
                        lightCond = "3";

                    // R       71    rain flag, = 0 for dry, = 1 for rain in the last minute, = 2 for rain right now
                    // RainFlag
                    string rainFlag = "0";
                    TimeSpan lastRain = DateTime.Now - lastRainIncident;
                    if (lastRain.TotalMinutes <= 1)
                        rainFlag = "1";
                    if (rainDetect.Equals("1"))
                        rainFlag = "2";

                    //Public Enum RainCond
                    // rainUnknown = 0
                    // rainDry = 1
                    // rainWet = 2 'sensor has water on it
                    // rainRain = 3 'falling rain drops detected
                    //End Enum
                    string rainCond = "1";
                    if (lastRain.TotalMinutes <= 5)
                        rainFlag = "1";
                    if (rainDetect.Equals("1"))
                        rainFlag = "2";

                    // W       73    wet flag, = 0 for dry, = 1 for wet in the last minute, = 2 for wet right now
                    string wetFlag = "0";
                    TimeSpan lastWet = DateTime.Now - lastCondIncident;
                    if (lastWet.TotalMinutes <= 1)
                        wetFlag = "1";
                    if (condensationDetect.Equals("1"))
                        wetFlag = "2";

                    boltwoodLine += rainFlag + " "
                        // Wetness Flag
                        + wetFlag + " ";


                    // time since last update
                    TimeSpan t = timeStamp - lastUpdate;
                    boltwoodLine += t.TotalSeconds.ToString("00000") + " "
                        // Now() in days. Leaving blank
                        + " 000000000000 ";


                    // various flags
                    boltwoodLine += cloudCond + " " + windCond + " " + rainCond + " " + lightCond;

                    // roof close and alert
                    boltwoodLine += " 0 0";

                    using (FileStream f = File.Create(bwf))
                    {
                        using (StreamWriter s = new StreamWriter(f))
                            s.WriteLine(boltwoodLine);
                    }
                }

                Thread.Sleep(updateInterval * 1000);
            }

            arduinoThreadRunning = false;
        }

        public void CommandBlind(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBlind");
        }

        public bool CommandBool(string command, bool raw)
        {
            throw new ASCOM.MethodNotImplementedException("CommandBool");
        }

        public string CommandString(string command, bool raw)
        {
            CheckConnected("CommandString");

            arduino.WriteLine(command);

            string response = arduino.ReadLine();
            return response;   // Wait for terminated character
        }

        public void Dispose()
        {
            // Clean up the tracelogger and util objects
            tl.Enabled = false;
            tl.Dispose();
            tl = null;
            utilities.Dispose();
            utilities = null;
            astroUtilities.Dispose();
            astroUtilities = null;
        }

        public bool Connected
        {
            get
            {
                LogMessage("Connected", "Get {0}", IsConnected);
                return IsConnected;
            }
            set
            {
                tl.LogMessage("Connected", "Set {0}", value);
                if (value == IsConnected)
                    return;

                if (value)
                {
                    LogMessage("Connected Set", "Connecting to port {0}", comPort);
                    try
                    {
                        // This thread will run as long as we are connected
                        arduino = new SerialPort(comPort, 9600);
                        arduino.Open();

                        arduinoThread = new Thread(this.boltWoodFile);
                        arduinoThread.Start();
                                               
                    }
                    catch (Exception e)
                    {
                        LogMessage("Could not connect to port {0}", comPort);
                        throw new ASCOM.NotConnectedException("Could not connect to port ", e);
                    }
                    connectedState = true;
                }
                else
                {
                    LogMessage("Connected Unset", "Disconnecting to port {0}", comPort);

                    // Before closing connection to Arduino, ask update thread to stop
                    connectedState = false;

                    while (arduinoThreadRunning)
                    {
                        Thread.Sleep(1000);
                    }

                    arduinoThread = null;

                    if (arduino != null)
                    {
                        arduino.Close();
                        arduino.Dispose();
                        arduino = null;
                    }
                }
            }
        }

        public string Description
        {
            // TODO customise this device description
            get
            {
                tl.LogMessage("Description Get", driverDescription);
                return driverDescription;
            }
        }

        public string DriverInfo
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverInfo = "Ascom Observing Conditions driver based on Arduino. Version: " + String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverInfo Get", driverInfo);
                return driverInfo;
            }
        }

        public string DriverVersion
        {
            get
            {
                Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string driverVersion = String.Format(CultureInfo.InvariantCulture, "{0}.{1}", version.Major, version.Minor);
                tl.LogMessage("DriverVersion Get", driverVersion);
                return driverVersion;
            }
        }

        public short InterfaceVersion
        {
            // set by the driver wizard
            get
            {
                LogMessage("InterfaceVersion Get", "1");
                return Convert.ToInt16("1");
            }
        }

        public string Name
        {
            get
            {
                string name = "Arduino Observing Conditions";
                tl.LogMessage("Name Get", name);
                return name;
            }
        }

        #endregion

        #region IObservingConditions Implementation

        /// <summary>
        /// Gets and sets the time period over which observations wil be averaged
        /// </summary>
        /// <remarks>
        /// Get must be implemented, if it can't be changed it must return 0
        /// Time period (hours) over which the property values will be averaged 0.0 =
        /// current value, 0.5= average for the last 30 minutes, 1.0 = average for the
        /// last hour
        /// </remarks>
        public double AveragePeriod
        {
            get
            {
                LogMessage("AveragePeriod", "get - 0");
                return 0;
            }
            set
            {
                LogMessage("AveragePeriod", "set - {0}", value);
                if (value != 0)
                    throw new InvalidValueException("AveragePeriod", value.ToString(), "0 only");
            }
        }

        /// <summary>
        /// Amount of sky obscured by cloud
        /// </summary>
        /// <remarks>0%= clear sky, 100% = 100% cloud coverage</remarks>
        public double CloudCover
        {
            get
            {
                LogMessage("CloudCover", "get");
                return cloudCover;
            }
        }

        /// <summary>
        /// Atmospheric dew point at the observatory in deg C
        /// </summary>
        /// <remarks>
        /// Normally optional but mandatory if <see cref=" ASCOM.DeviceInterface.IObservingConditions.Humidity"/>
        /// Is provided
        /// </remarks>
        public double DewPoint
        {
            get
            {
                LogMessage("DewPoint", "get");
                return dewPoint;
            }
        }

        /// <summary>
        /// Atmospheric relative humidity at the observatory in percent
        /// </summary>
        /// <remarks>
        /// Normally optional but mandatory if <see cref="ASCOM.DeviceInterface.IObservingConditions.DewPoint"/> 
        /// Is provided
        /// </remarks>
        public double Humidity
        {
            get
            {
                LogMessage("Humidity", "get");
                return humidity;
            }
        }

        /// <summary>
        /// Atmospheric pressure at the observatory in hectoPascals (mB)
        /// </summary>
        /// <remarks>
        /// This must be the pressure at the observatory and not the "reduced" pressure
        /// at sea level. Please check whether your pressure sensor delivers local pressure
        /// or sea level pressure and adjust if required to observatory pressure.
        /// </remarks>
        public double Pressure
        {
            get
            {
                LogMessage("Pressure", "get");
                return pressure;
            }
        }

        /// <summary>
        /// Rain rate at the observatory
        /// </summary>
        /// <remarks>
        /// This property can be interpreted as 0.0 = Dry any positive nonzero value
        /// = wet.
        /// </remarks>
        public double RainRate
        {
            get
            {
                LogMessage("RainRate", "get");
                return rainRate;
            }
        }

        /// <summary>
        /// Forces the driver to immediatley query its attached hardware to refresh sensor
        /// values
        /// </summary>
        public void Refresh()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Provides a description of the sensor providing the requested property
        /// </summary>
        /// <param name="PropertyName">Name of the property whose sensor description is required</param>
        /// <returns>The sensor description string</returns>
        /// <remarks>
        /// PropertyName must be one of the sensor properties, 
        /// properties that are not implemented must throw the MethodNotImplementedException
        /// </remarks>
        public string SensorDescription(string PropertyName)
        {
            switch (PropertyName.Trim().ToLowerInvariant())
            {
                case "averageperiod":
                    return "Average period in hours, immediate values are only available";
                case "dewpoint":
                case "humidity":
                    LogMessage("SensorDescription", "{0}", PropertyName);
                    return CommandString("HumidityDescription", false);
                case "temperature":
                    LogMessage("SensorDescription", "{0}", PropertyName);
                    return CommandString("TemperatureDescription", false);
                case "skytemperature":
                    LogMessage("SensorDescription", "{0}", PropertyName);
                    return CommandString("SkyTemperatureDescription", false);
                case "pressure":
                    LogMessage("SensorDescription", "{0}", PropertyName);
                    return CommandString("PressureDescription", false);
                case "skybrightness":
                    LogMessage("SensorDescription", "{0}", PropertyName);
                    return CommandString("SkyBrightnessDescription", false);
                case "rainrate":
                case "skyquality":
                case "starfwhm":
                case "winddirection":
                case "windgust":
                case "windspeed":
                    LogMessage("SensorDescription", "{0} - not implemented", PropertyName);
                    throw new MethodNotImplementedException("SensorDescription(" + PropertyName + ")");
                default:
                    LogMessage("SensorDescription", "{0} - unrecognised", PropertyName);
                    throw new ASCOM.InvalidValueException("SensorDescription(" + PropertyName + ")");
            }
        }

        /// <summary>
        /// Sky brightness at the observatory
        /// </summary>
        public double SkyBrightness
        {
            get
            {
                LogMessage("SkyBrightness", "get");
                return skyBrightness;
            }
        }

        /// <summary>
        /// Sky quality at the observatory
        /// </summary>
        public double SkyQuality
        {
            get
            {
                LogMessage("SkyQuality", "get - not implemented");
                throw new PropertyNotImplementedException("SkyQuality", false);
            }
        }

        /// <summary>
        /// Seeing at the observatory
        /// </summary>
        public double StarFWHM
        {
            get
            {
                LogMessage("StarFWHM", "get - not implemented");
                throw new PropertyNotImplementedException("StarFWHM", false);
            }
        }

        /// <summary>
        /// Sky temperature at the observatory in deg C
        /// </summary>
        public double SkyTemperature
        {
            get
            {
                LogMessage("SkyTemperature", "get");
                return skyTemperature;
            }
        }

        /// <summary>
        /// Temperature at the observatory in deg C
        /// </summary>
        public double Temperature
        {
            get
            {
                LogMessage("Temperature", "get");
                return temperature;
            }
        }

        /// <summary>
        /// Provides the time since the sensor value was last updated
        /// </summary>
        /// <param name="PropertyName">Name of the property whose time since last update Is required</param>
        /// <returns>Time in seconds since the last sensor update for this property</returns>
        /// <remarks>
        /// PropertyName should be one of the sensor properties Or empty string to get
        /// the last update of any parameter. A negative value indicates no valid value
        /// ever received.
        /// </remarks>
        public double TimeSinceLastUpdate(string PropertyName)
        {
            // the checks can be removed if all properties have the same time.
            if (!string.IsNullOrEmpty(PropertyName))
            {
                switch (PropertyName.Trim().ToLowerInvariant())
                {
                    // break or return the time on the properties that are implemented
                    case "averageperiod":
                    case "dewpoint":
                    case "humidity":
                    case "pressure":
                    case "rainrate":
                    case "skybrightness":
                    case "skyquality":
                    case "starfwhm":
                    case "skytemperature":
                    case "temperature":
                    case "winddirection":
                    case "windgust":
                    case "windspeed":
                        // throw an exception on the properties that are not implemented
                        LogMessage("TimeSinceLastUpdate", "{0} - not implemented", PropertyName);
                        throw new MethodNotImplementedException("SensorDescription(" + PropertyName + ")");
                    default:
                        LogMessage("TimeSinceLastUpdate", "{0} - unrecognised", PropertyName);
                        throw new ASCOM.InvalidValueException("SensorDescription(" + PropertyName + ")");
                }
            }
            // return the time
            LogMessage("TimeSinceLastUpdate", "{0} - not implemented", PropertyName);
            throw new MethodNotImplementedException("TimeSinceLastUpdate(" + PropertyName + ")");
        }

        /// <summary>
        /// Wind direction at the observatory in degrees
        /// </summary>
        /// <remarks>
        /// 0..360.0, 360=N, 180=S, 90=E, 270=W. When there Is no wind the driver will
        /// return a value of 0 for wind direction
        /// </remarks>
        public double WindDirection
        {
            get
            {
                LogMessage("WindDirection", "get");
                return windDirection;
            }
        }

        /// <summary>
        /// Peak 3 second wind gust at the observatory over the last 2 minutes in m/s
        /// </summary>
        public double WindGust
        {
            get
            {
                LogMessage("WindGust", "get");
                return windGust;
            }
        }

        /// <summary>
        /// Wind speed at the observatory in m/s
        /// </summary>
        public double WindSpeed
        {
            get
            {
                LogMessage("WindSpeed", "get");
                return windSpeed;
            }
        }

        #endregion

        #region private methods

        #region calculate the gust strength as the largest wind recorded over the last two minutes

        // save the time and wind speed values
        private Dictionary<DateTime, double> winds = new Dictionary<DateTime, double>();

        private double gustStrength;

        private void UpdateGusts(double speed)
        {
            Dictionary<DateTime, double> newWinds = new Dictionary<DateTime, double>();
            var last = DateTime.Now - TimeSpan.FromMinutes(2);
            winds.Add(DateTime.Now, speed);
            var gust = 0.0;
            foreach (var item in winds)
            {
                if (item.Key > last)
                {
                    newWinds.Add(item.Key, item.Value);
                    if (item.Value > gust)
                        gust = item.Value;
                }
            }
            gustStrength = gust;
            winds = newWinds;
        }

        #endregion

        #endregion

        #region Private properties and methods
        // here are some useful properties and methods that can be used as required
        // to help with driver development

        #region ASCOM Registration

        // Register or unregister driver for ASCOM. This is harmless if already
        // registered or unregistered. 
        //
        /// <summary>
        /// Register or unregister the driver with the ASCOM Platform.
        /// This is harmless if the driver is already registered/unregistered.
        /// </summary>
        /// <param name="bRegister">If <c>true</c>, registers the driver, otherwise unregisters it.</param>
        private static void RegUnregASCOM(bool bRegister)
        {
            using (var P = new ASCOM.Utilities.Profile())
            {
                P.DeviceType = "ObservingConditions";
                if (bRegister)
                {
                    P.Register(driverID, driverDescription);
                }
                else
                {
                    P.Unregister(driverID);
                }
            }
        }

        /// <summary>
        /// This function registers the driver with the ASCOM Chooser and
        /// is called automatically whenever this class is registered for COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is successfully built.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During setup, when the installer registers the assembly for COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually register a driver with ASCOM.
        /// </remarks>
        [ComRegisterFunction]
        public static void RegisterASCOM(Type t)
        {
            RegUnregASCOM(true);
        }

        /// <summary>
        /// This function unregisters the driver from the ASCOM Chooser and
        /// is called automatically whenever this class is unregistered from COM Interop.
        /// </summary>
        /// <param name="t">Type of the class being registered, not used.</param>
        /// <remarks>
        /// This method typically runs in two distinct situations:
        /// <list type="numbered">
        /// <item>
        /// In Visual Studio, when the project is cleaned or prior to rebuilding.
        /// For this to work correctly, the option <c>Register for COM Interop</c>
        /// must be enabled in the project settings.
        /// </item>
        /// <item>During uninstall, when the installer unregisters the assembly from COM Interop.</item>
        /// </list>
        /// This technique should mean that it is never necessary to manually unregister a driver from ASCOM.
        /// </remarks>
        [ComUnregisterFunction]
        public static void UnregisterASCOM(Type t)
        {
            RegUnregASCOM(false);
        }

        #endregion

        /// <summary>
        /// Returns true if there is a valid connection to the driver hardware
        /// </summary>
        private bool IsConnected
        {
            get
            {
                // TODO check that the driver hardware connection exists and is connected to the hardware
                return connectedState;
            }
        }

        /// <summary>
        /// Use this function to throw an exception if we aren't connected to the hardware
        /// </summary>
        /// <param name="message"></param>
        private void CheckConnected(string message)
        {
            if (!IsConnected)
            {
                throw new ASCOM.NotConnectedException(message);
            }
        }

        /// <summary>
        /// Read the device configuration from the ASCOM Profile store
        /// </summary>
        internal void ReadProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                tl.Enabled = Convert.ToBoolean(driverProfile.GetValue(driverID, traceStateProfileName, string.Empty, traceStateDefault));
                comPort = driverProfile.GetValue(driverID, comPortProfileName, string.Empty, comPortDefault);

                updateInterval = int.Parse(driverProfile.GetValue(driverID, updateIntervalProfileName, string.Empty, updateIntervalDefault.ToString()));

                clearSkies = float.Parse(driverProfile.GetValue(driverID, clearSkiesProfileName, string.Empty, clearSkiesDefault.ToString()));
                cloudySkies = float.Parse(driverProfile.GetValue(driverID, cloudySkiesProfileName, string.Empty, cloudySkiesDefault.ToString()));

                cloudyCond = int.Parse(driverProfile.GetValue(driverID, cloudyCondProfileName, string.Empty, cloudyCondDefault.ToString()));
                veryCloudyCond = int.Parse(driverProfile.GetValue(driverID, veryCloudyCondProfileName, string.Empty, veryCloudyCondDefault.ToString()));

                nightVol = float.Parse(driverProfile.GetValue(driverID, nightVolProfileName, string.Empty, nightVolDefault.ToString()));
                dayVol = float.Parse(driverProfile.GetValue(driverID, dayVolProfileName, string.Empty, dayVolDefault.ToString()));

                nightLux = int.Parse(driverProfile.GetValue(driverID, nightLuxProfileName, string.Empty, nightLuxDefault.ToString()));
                dayLux = int.Parse(driverProfile.GetValue(driverID, dayLuxProfileName, string.Empty, dayLuxDefault.ToString()));

                twilightCond = int.Parse(driverProfile.GetValue(driverID, twilightCondProfileName, string.Empty, twilightCondDefault.ToString()));
                daylightCond = int.Parse(driverProfile.GetValue(driverID, daylightCondProfileName, string.Empty, daylightCondDefault.ToString()));

                windyCond = float.Parse(driverProfile.GetValue(driverID, windyCondProfileName, string.Empty, windyCondDefault.ToString()));
                veryWindyCond = float.Parse(driverProfile.GetValue(driverID, veryWindyCondProfileName, string.Empty, veryWindyCondDefault.ToString()));

                bwf = driverProfile.GetValue(driverID, bwfProfileName, string.Empty, bwfDefault);

                bwfEnabled = Convert.ToBoolean(driverProfile.GetValue(driverID, bwfEnabledProfileName, string.Empty, bwfEnabledDefault.ToString()));
            }
        }

        /// <summary>
        /// Write the device configuration to the  ASCOM  Profile store
        /// </summary>
        internal void WriteProfile()
        {
            using (Profile driverProfile = new Profile())
            {
                driverProfile.DeviceType = "ObservingConditions";
                driverProfile.WriteValue(driverID, traceStateProfileName, tl.Enabled.ToString());

                driverProfile.WriteValue(driverID, comPortProfileName, comPort.ToString());

                driverProfile.WriteValue(driverID, updateIntervalProfileName, updateInterval.ToString());

                driverProfile.WriteValue(driverID, clearSkiesProfileName, clearSkies.ToString());
                driverProfile.WriteValue(driverID, cloudySkiesProfileName,cloudySkies.ToString());

                driverProfile.WriteValue(driverID, cloudyCondProfileName, cloudyCond.ToString());
                driverProfile.WriteValue(driverID, veryCloudyCondProfileName, veryCloudyCond.ToString());

                driverProfile.WriteValue(driverID, nightVolProfileName, nightVol.ToString());
                driverProfile.WriteValue(driverID, dayVolProfileName, dayVol.ToString());

                driverProfile.WriteValue(driverID, nightLuxProfileName, nightLux.ToString());
                driverProfile.WriteValue(driverID, dayLuxProfileName, dayLux.ToString());

                driverProfile.WriteValue(driverID, twilightCondProfileName, twilightCond.ToString());
                driverProfile.WriteValue(driverID, daylightCondProfileName, daylightCond.ToString());

                driverProfile.WriteValue(driverID, windyCondProfileName, windyCond.ToString());
                driverProfile.WriteValue(driverID, veryWindyCondProfileName, veryWindyCond.ToString());

                driverProfile.WriteValue(driverID, bwfProfileName, bwf);

                driverProfile.WriteValue(driverID, bwfEnabledProfileName, bwfEnabled.ToString());
            }
        }

        /// <summary>
        /// Log helper function that takes formatted strings and arguments
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        internal static void LogMessage(string identifier, string message, params object[] args)
        {
            var msg = string.Format(message, args);
            tl.LogMessage(identifier, msg);
        }
        #endregion
    }
}
