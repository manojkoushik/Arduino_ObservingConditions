/*
 * 
 Arduino based weather station implementing Ascom Observing Conditions
 
 Based on Arduino Uno, Sparkfun weather shield https://www.sparkfun.com/products/12081
 and Sparkfun weather meters: https://www.sparkfun.com/products/8942
 Also using Melexis 90614 IR Sensor for sky temperature and cloud detection

 Original sketch based on what was written by Nathan Seidle https://github.com/sparkfun/Weather_Shield

 Author: Manoj Koushik (manoj.koushik@gmail.com)
 Version: 1.0.0

 License: This code is public domain. Beers and a thank you are always welcome. 
 
 */

#include <Wire.h> //I2C needed for sensors
#include <SparkFunMPL3115A2.h> //Pressure sensor - Search "SparkFun MPL3115" and install from Library Manager
#include <SparkFunHTU21D.h> //Humidity sensor - Search "SparkFun HTU21D" and install from Library Manager
#include <SparkFunMLX90614.h> // MLX90614 IR thermometer library https://github.com/sparkfun/SparkFun_MLX90614_Arduino_Library
#include <SerialCommand.h> // Serial command library https://github.com/kroimon/Arduino-SerialCommand

// Weather Objects to read sensors
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
MPL3115A2 myPressure; //Create an instance of the pressure sensor
HTU21D myHumidity; //Create an instance of the humidity sensor
IRTherm myIRSkyTemp; // Create an IRTherm object called temp
SerialCommand myDeviceCmd; // Create a serial command object to deal with Ascom Driver
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

// Hardware pin definitions
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// digital I/O pins
const byte WSPEED = 3;
const byte RAIN = 2;
const byte STAT1 = 7;
const byte STAT2 = 8;

// analog I/O pins
const byte REFERENCE_3V3 = A3;
const byte LIGHT = A1;
const byte BATT = A2;
const byte WDIR = A0;
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

// Constants
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
const byte  WINDGUST_PERIOD = 120;
const byte  RAIN_PERIOD = 60;
const float INCHES_TO_MM = 25.4;
const byte  SWITCH_BOUNCE = 10;
const float RAIN_PER_INT = 0.011;
const int   MSECS_TO_SEC = 1000;
const byte  SECS_TO_MIN = 60;
const byte  MINS_TO_HOUR = 60;
const float SPEED_PER_WINDCLICK = 1.492;
const float MPH_TO_MS = 0.44704;
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

// Global Variables to keep track of things
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Various timers and indices
long lastSecond; //The millis counter to see when a second rolls by
byte seconds = 0; //When it hits 60, increase the current minute
byte minutes = 0; //Keeps track of where we are in various arrays of data
byte windspeed_hist_secs = 0;

long          lastWindCheck = 0;
volatile long lastWindIRQ = 0;
volatile byte windClicks = 0;

// volatiles are subject to modification by IRQs
volatile float rainHour[60]; //60 floating numbers to keep track of 60 minutes of rain
volatile unsigned long raintime, rainlast, raininterval;

float windspeed_hist[120];
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

// Global Variables keeping track of weather parameters
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
int   winddir = 0; // [0-360 instantaneous wind direction]
float windspeed = 0; // [m/s instantaneous wind speed]

float humidity = 0; // [%]
float pressure = 0;

float pTempc = 0; // [Ambient temperature C, from pressure sensor]
float hTempc = 0; // [Ambient temperature C from humidity sensor]
float irTempc = 0; // [Ambient temperature C from IR sensor]
float irSkyTempc = 0; // [Sky temperature C from IR sensor]

float light_lvl = 455; //[analog value from 0 to 1023]
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

//Interrupt routines (these are called by the hardware interrupts, not by the main code)
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
void rainIRQ()
// Count rain gauge bucket tips as they occur
// Activated by the magnet and reed switch in the rain gauge, attached to input D2
{
  raintime = millis(); // grab current time
  raininterval = raintime - rainlast; // calculate interval between this and last event

  if (raininterval > SWITCH_BOUNCE) // ignore switch-bounce glitches less than 10mS after initial edge
  {
    rainHour[minutes] += RAIN_PER_INT; //Increase this minute's amount of rain
    rainlast = raintime; // set up for next event
  }
}

void wspeedIRQ()
// Activated by the magnet in the anemometer (2 ticks per rotation), attached to input D3
{
  if (millis() - lastWindIRQ > SWITCH_BOUNCE) // Ignore switch-bounce glitches less than 10ms (142MPH max reading) after the reed switch closes
  {
    lastWindIRQ = millis(); //Grab the current time
    windClicks++; //There is 1.492MPH for each click per second.
  }
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


void setup()
{
  Serial.begin(9600);

  // Debug
  Serial.println("Booting up Weather Station...");

  pinMode(STAT1, OUTPUT); //Status LED Blue
  pinMode(STAT2, OUTPUT); //Status LED Green

  pinMode(WSPEED, INPUT_PULLUP); // input from wind meters windspeed sensor
  pinMode(RAIN, INPUT_PULLUP); // input from wind meters rain gauge sensor

  pinMode(REFERENCE_3V3, INPUT);
  pinMode(LIGHT, INPUT);

  //Configure the pressure sensor
  myPressure.begin(); // Get sensor online
  myPressure.setModeBarometer(); // Measure pressure in Pascals from 20 to 110 kPa
  myPressure.setOversampleRate(7); // Set Oversample to the recommended 128
  myPressure.enableEventFlags(); // Enable all three pressure and temp event flags

  //Configure the humidity sensor
  myHumidity.begin();

  lastSecond = millis();

  // attach external interrupt pins to IRQ functions
  attachInterrupt(0, rainIRQ, FALLING);
  attachInterrupt(1, wspeedIRQ, FALLING);

  myIRSkyTemp.begin(); // Initialize I2C library and the MLX90614
  myIRSkyTemp.setUnit(TEMP_C); // Set units to Farenheit (alternatively TEMP_C or TEMP_K)

  // Add all command to support Ascom
  //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
  myDeviceCmd.addCommand("Humidity", Humidity); // Atmospheric humidity (%)
  myDeviceCmd.addCommand("Pressure", Pressure); // Atmospheric presure at the observatory (Ascom needs hPa)
                                       // This must be the pressure at the observatory altitude and not the adjusted pressure at sea level. 
  myDeviceCmd.addCommand("RainRate", RainRate); // Rain rate (Ascom needs mm/hour)
                                       // This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.
                                       //   Rainfall intensity is classified according to the rate of precipitation:
                                       //   Light rain — when the precipitation rate is < 2.5 mm (0.098 in) per hour
                                       //   Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) - 7.6 mm (0.30 in) or 10 mm (0.39 in) per hour
                                       //   Heavy rain — when the precipitation rate is > 7.6 mm (0.30 in) per hour, or between 10 mm (0.39 in) and 50 mm (2.0 in) per hour
                                       //   Violent rain — when the precipitation rate is > 50 mm (2.0 in) per hour
  myDeviceCmd.addCommand("SkyBrightness", SkyBrightness); // Sky brightness (Ascom needs Lux, but we are returning voltage. Ascom driver will have to be calibrated)
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
  myDeviceCmd.addCommand("SkyTemperature", SkyTemperature); // Sky temperature in °C
  myDeviceCmd.addCommand("Temperature", Temperature); // Temperature in °C
  myDeviceCmd.addCommand("WindDirection", WindDirection); // Wind direction (degrees, 0..360.0) 
                                                 // Value of 0.0 is returned when the wind speed is 0.0. 
                                                 // Wind direction is measured clockwise from north, through east, 
                                                 // where East=90.0, South=180.0, West=270.0 and North=360.0
  myDeviceCmd.addCommand("WindGust", WindGust); // Wind gust (Ascom needs m/s) Peak 3 second wind speed over the last 2 minutes
  myDeviceCmd.addCommand("WindSpeed", WindSpeed); // Wind speed (Ascom needs m/s)
  //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


  // Add commands for Action property of Ascom Driver
  //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
  // Support for Value
  myDeviceCmd.addCommand("HumidityValue", HumidityValue);
  myDeviceCmd.addCommand("PressureValue", PressureValue);
  myDeviceCmd.addCommand("SkyTemperatureValue", SkyTemperatureValue);
  myDeviceCmd.addCommand("TemperatureValue", TemperatureValue);
  myDeviceCmd.addCommand("SkyBrightnessValue", SkyBrightnessValue);

  // Support for Description
  myDeviceCmd.addCommand("HumidityDescription", HumidityDescription);
  myDeviceCmd.addCommand("PressureDescription", PressureDescription);
  myDeviceCmd.addCommand("SkyTemperatureDescription", SkyTemperatureDescription);
  myDeviceCmd.addCommand("TemperatureDescription", TemperatureDescription);
  myDeviceCmd.addCommand("SkyBrightnessDescription", SkyBrightnessDescription);
  //-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


  // Support for debugging
  myDeviceCmd.addCommand("WeatherDebug", printWeather); // Used to debug and print out all weather parameters

  // Zero out the arrays
  int i;
  for (i = 0; i < WINDGUST_PERIOD; i++)
    windspeed_hist[i] = 0;
 
  for (i = 0; i < RAIN_PERIOD; i++)
    rainHour[i] = 0;

  // turn on interrupts
  interrupts();

  // Debug
  Serial.println("Observatory Weather Station online!");  
}

// Commands for Action property of Ascom Driver
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

void HumidityValue()
{
  Humidity();
}

void HumidityDescription()
{
  Serial.println("HTU21D");
}

void PressureValue()
{
  Pressure();
}

void PressureDescription()
{
  Serial.println("MPL3115A2");
}

void SkyTemperatureValue()
{
  SkyTemperature();
}

void SkyTemperatureDescription()
{
  Serial.println("MLX90614");
}

void TemperatureValue()
{
  Temperature();
}

void TemperatureDescription()
{
  Serial.println("MLX90614");
}

void SkyBrightnessValue()
{
  SkyBrightness();
}

void SkyBrightnessDescription()
{
  Serial.println("ALS-PT19");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

// Ascom command handlers
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

void Humidity()
{
  Serial.println(humidity,5);
}

void Pressure()
{
  Serial.println(pressure,5);
}

void RainRate()
{
  float rainRate;

  for (int i; i < RAIN_PERIOD; i++)
    rainRate += rainHour[i];

  rainRate *= INCHES_TO_MM;
  Serial.println(rainRate,5);
}

void SkyBrightness()
{
  Serial.println(get_light_level(),5);
}

void SkyTemperature()
{
  Serial.println(irSkyTempc,5);
}

void Temperature()
{
  Serial.println(irTempc,5);
}

void WindGust()
{
  float peakwindgust = 0;
  float windgust = 0;
  for (int i = 0; i < WINDGUST_PERIOD; i++)
  {
    int j = i + 1;
    if (j >= WINDGUST_PERIOD) j = 0;
    int k = j + 1;
    if (k >= WINDGUST_PERIOD) k = 0;

    windgust = (windspeed_hist[i] + windspeed_hist[j] + windspeed_hist[k])/3;

    if (windgust > peakwindgust) peakwindgust = windgust;
  }
      
  Serial.println(windgust,5);
}

void WindSpeed()
{
  Serial.println(windspeed,5);
}

void WindDirection()
{
  Serial.println(winddir,5);
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


void loop()
{
  //Keep track of which minute it is
  if(millis() - lastSecond >= MSECS_TO_SEC)
  {
    digitalWrite(STAT1, HIGH); //Blink stat LED

    lastSecond += MSECS_TO_SEC;

    // Calculate all weather readings
    calcWeather();

    digitalWrite(STAT1, LOW); //Turn off stat LED
  }
}

//Calculates each of the variables that wunderground is expecting
void calcWeather()
{
  //Calc humidity
  humidity = myHumidity.readHumidity();

  //Calc pressure
  pressure = myPressure.readPressure();

  // Calc rain
  calc_rain();

  //Calc light level
  light_lvl = get_light_level();

  // Calc Temp
  // Ambient from humidity sensor
  hTempc = myHumidity.readTemperature();
  // Ambient from pressure sensor
  pTempc = myPressure.readTemp();
  // Ambient and sky temp from IR sensor
  if (myIRSkyTemp.read()) // Read from the sensor
  { // If the read is successful:
    float irTempc = myIRSkyTemp.ambient(); // Get updated ambient temperature
    float irSkyTempc = myIRSkyTemp.object(); // Get updated object temperature
  }

    // Calc Wind
  calc_wind();
}

void calc_rain()
{
  if(++seconds >= SECS_TO_MIN)
  {
    seconds = 0;
    if(++minutes >= MINS_TO_HOUR) minutes = 0;
    rainHour[minutes] = 0; //Zero out this minute's rainfall amount
  }
}

void calc_wind()
{
  // Wind Direction

  unsigned int adc;

  adc = analogRead(WDIR); // get the current reading from the sensor

  // The following table is ADC readings for the wind direction sensor output, sorted from low to high.
  // Each threshold is the midpoint between adjacent headings. The output is degrees for that ADC reading.
  // Note that these are not in compass degree order! See Weather Meters datasheet for more information.

  winddir = -1;
  if (adc < 380) winddir = 113;
  if (adc < 393) winddir = 68;
  if (adc < 414) winddir = 90;
  if (adc < 456) winddir = 158;
  if (adc < 508) winddir = 135;
  if (adc < 551) winddir = 203;
  if (adc < 615) winddir = 180;
  if (adc < 680) winddir = 23;
  if (adc < 746) winddir = 45;
  if (adc < 801) winddir = 248;
  if (adc < 833) winddir = 225;
  if (adc < 878) winddir = 338;
  if (adc < 913) winddir = 0;
  if (adc < 940) winddir = 293;
  if (adc < 967) winddir = 315;
  if (adc < 990) winddir = 270;

  // WindSpeed
  // Interrupt adds up the clicks. Each click is 1.492MPH of additional speed
  
  float deltaTime = millis() - lastWindCheck; //750ms
  lastWindCheck = millis();

  deltaTime /= MSECS_TO_SEC; //Covert to seconds

  windspeed = (float)windClicks / deltaTime; //3 / 0.750s = 4
  windClicks = 0; //Reset and start watching for new wind

  windspeed *= SPEED_PER_WINDCLICK; //4 * 1.492 = 5.968MPH
  windspeed *= MPH_TO_MS; // Convert to m/s

  // To calculate windgust we keep a 2 minute rolling history of instantaneous windspeed
  // Ascom needs 3 second gust over 2 minutes

  windspeed_hist[windspeed_hist_secs] = windspeed;
  if (++windspeed_hist_secs > (WINDGUST_PERIOD - 1))
    windspeed_hist_secs = 0;
  //zero out this seconds windspeed history  
  windspeed_hist[windspeed_hist_secs] = 0;  
}

//Returns the voltage of the light sensor based on the 3.3V rail
//This allows us to ignore what VCC might be (an Arduino plugged into USB has VCC of 4.5 to 5.2V)
float get_light_level()
{
  float operatingVoltage = analogRead(REFERENCE_3V3);

  float lightSensor = analogRead(LIGHT);

  operatingVoltage = 3.3 / operatingVoltage; //The reference voltage is 3.3V

  lightSensor = operatingVoltage * lightSensor;

  return(lightSensor);
}

void printWeather()
{
  Serial.print("Humidity: ");
  Humidity();

  Serial.print("Pressure: ");
  Pressure();

  Serial.print("RainRate: ");
  RainRate();

  Serial.print("SkyBrightness: ");
  SkyBrightness();

  Serial.print("SkyTemperature: ");
  SkyTemperature();

  Serial.print("Temperature: ");
  Temperature();

  Serial.print("WindGust: ");
  WindGust();

  Serial.print("WindSpeed: ");
  WindSpeed();

  Serial.print("WindDirection: ");
  WindDirection();
}
