/*
 * 
 Arduino based weather station implementing Ascom Observing Conditions
 
 Based on Arduino Uno, Sparkfun weather shield https://www.sparkfun.com/products/12081
 and Sparkfun weather meters: https://www.sparkfun.com/products/8942
 Also using Melexis 90614 IR Sensor for sky temperature and cloud detection

 Original sketch based on what was written by Nathan Seidle https://github.com/sparkfun/Weather_Shield

 Author: Manoj Koushik (manoj.koushik@gmail.com)
 Version: 1.1.0

 License: This code is public domain. Beers and a thank you are always welcome. 
 
 */

#include <Wire.h> //I2C needed for sensors
#include <SparkFunMPL3115A2.h> //Pressure sensor - Search "SparkFun MPL3115" and install from Library Manager
#include <SparkFunHTU21D.h> //Humidity sensor - Search "SparkFun HTU21D" and install from Library Manager
#include <SparkFunMLX90614.h> // MLX90614 IR thermometer library https://github.com/sparkfun/SparkFun_MLX90614_Arduino_Library

// Which Serial port on the leonardo?
#define WRITE Serial
#define READ Serial

// Weather Objects to read sensors
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
MPL3115A2 myPressure; //Create an instance of the pressure sensor
HTU21D myHumidity; //Create an instance of the humidity sensor
IRTherm myIRSkyTemp; // Create an IRTherm object called temp

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
const byte RG11 = 8;
const byte HDS10 = A5;
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Constants
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
const byte  WINDGUST_MAX_PERIOD = 40;
const byte  WINDGUST_AVG_PERIOD = 3;
const byte  RAIN_PERIOD = 60;
const byte  SWITCH_BOUNCE = 10;
const float RAIN_PER_INT = 0.011;
const int   MSECS_TO_SEC = 1000;
const byte  SECS_TO_MIN = 60;
const byte  MINS_TO_HOUR = 60;
const float SPEED_PER_WINDCLICK = 1.492;

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Global Variables to keep track of things
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Various timers and indices
long lastSecond; //The millis counter to see when a second rolls by
byte seconds = 0; //When it hits 60, increase the current minute
byte minutes = 0; //Keeps track of where we are in various arrays of data

long          lastWindCheck = 0;
volatile long lastWindIRQ = 0;
volatile byte windClicks = 0;

// volatiles are subject to modification by IRQs
volatile float rainHour[60]; //60 floating numbers to keep track of 60 minutes of rain
volatile unsigned long raintime, rainlast, raininterval;

float windspeed = 0; // [m/s instantaneous wind speed]
float windgusts[40]; // Keep track of 3s windgusts over the last 2 minutes
byte  windgusts_index = 0; // Index to track wind gusts over the last two minutes
float windgust_calc[WINDGUST_AVG_PERIOD - 1]; // keep the last 2 wind speeds (plus the current) to calculate windgust 
byte  windgust_calc_index = 0; // Index to keep track of these 3 wind speeds 

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

  pinMode(STAT1, OUTPUT); //Status LED Blue
  pinMode(STAT2, OUTPUT); //Status LED Green

  pinMode(WSPEED, INPUT_PULLUP); // input from wind meters windspeed sensor
  pinMode(RAIN, INPUT_PULLUP); // input from wind meters rain gauge sensor

  pinMode(REFERENCE_3V3, INPUT);
  pinMode(LIGHT, INPUT);

  pinMode(RG11, INPUT); // Hydreon RG11 for rain sensing. Remember to divide the voltage down before consuming as input
  pinMode(HDS10, INPUT); // HDS10 for condensation sensing
    
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

  myIRSkyTemp.begin(MLX90614_DEFAULT_ADDRESS); // Initialize I2C library and the MLX90614
  myIRSkyTemp.setUnit(TEMP_C); // Set units to Centigrade (alternatively TEMP_C or TEMP_K)
  
  // Zero out the arrays
  int i;
  for (i = 0; i < WINDGUST_MAX_PERIOD; i++)
    windgusts[i] = 0;
 
  for (i = 0; i < RAIN_PERIOD; i++)
    rainHour[i] = 0;

  for (i = 0; i < WINDGUST_AVG_PERIOD - 1; i++)
    windgust_calc[i] = 0;

  // turn on interrupts
  interrupts();
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Commands for Action property of Ascom Driver
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=

void HumidityDescription()
{
  WRITE.println("HTU21D");
}

void PressureDescription()
{
  WRITE.println("MPL3115A2");
}

void SkyTemperatureDescription()
{
  WRITE.println("MLX90614");
}

void TemperatureDescription()
{
  WRITE.println("MLX90614");
}

void SkyBrightnessDescription()
{
  WRITE.println("ALS-PT19");
}

//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
// Ascom command handlers
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=
void RainDetect()
{
  // If not using hydreon sensor, then print 1 if rainHour[minutes] > 1
  WRITE.println(digitalRead(RG11));
}

void CondensationDetect()
{
  if (analogRead(HDS10) > 1000 || rainHour[minutes] > 0 || digitalRead(RG11) > 0) {
    WRITE.println("1");
  } else {
    WRITE.println("0");
  }
}

void Humidity()
{
  WRITE.println(myHumidity.readHumidity(),5);
}

void Pressure()
{
  WRITE.println(myPressure.readPressure(),5);
}

void RainRate()
{
  float rainRate;

  for (int i = 0; i < RAIN_PERIOD; i++)
    rainRate += rainHour[i];

  WRITE.println(rainRate,5);
}

void SkyBrightness()
{
  WRITE.println(get_light_level(),5);
}

void SkyTemperature()
{
  if (myIRSkyTemp.read()) // Read from the sensor
  { 
    // If the read is successful:
    WRITE.println(myIRSkyTemp.object(),5);
    float irTempc = myIRSkyTemp.ambient(); // Get updated ambient temperature
  } else
  {
    WRITE.println("");
  }
}

void Temperature()
{
  if (myIRSkyTemp.read()) // Read from the sensor
  { 
    // If the read is successful:
    WRITE.println(myIRSkyTemp.ambient(),5);
  } else
  {
    WRITE.println("");
  }
}

void WindGust()
{
  float peakwindgust = 0;

  for (int i = 0; i < WINDGUST_MAX_PERIOD; i++)
    if (windgusts[i] > peakwindgust)
      peakwindgust = windgusts[i];
      
  WRITE.println(peakwindgust,5);
}

void WindSpeed()
{
  WRITE.println(windspeed,5);
}

void WindDirection()
{
  unsigned int winddir;
  
  unsigned int adc;

  adc = analogRead(WDIR); // get the current reading from the sensor

  // The following table is ADC readings for the wind direction sensor output, sorted from low to high.
  // Each threshold is the midpoint between adjacent headings. The output is degrees for that ADC reading.
  // Note that these are not in compass degree order! See Weather Meters datasheet for more information.
  //0,33k, 3.84v
  //22.5, 6.57k, 1.98v
  //45, 8.2k, 2.25v
  //67.5, 891, 0.41v
  //90, 1k, 0.45v
  //112.5, 688, 0.32v
  //135,2.2k, 0.90v
  //157.5, 1.41k, 0.62v
  //180 ,3.9k, 1.40v
  //202.5, 3.14k, 1.19v
  //225, 16k ,3.08v
  //247.5, 14.12k, 2.93v
  //270, 120k ,4.62v
  //292.5, 42.12k, 4.04v
  //315, 64.9k, 4.78v
  //337.5, 21.88k, 3.43v

  winddir = -1;
  if (adc < 990) winddir = 270;
  if (adc < 967) winddir = 315;
  if (adc < 940) winddir = 293;
  if (adc < 913) winddir = 0;
  if (adc < 878) winddir = 338;
  if (adc < 833) winddir = 225;
  if (adc < 801) winddir = 248;
  if (adc < 746) winddir = 45;
  if (adc < 680) winddir = 23;
  if (adc < 615) winddir = 180;
  if (adc < 551) winddir = 203;
  if (adc < 508) winddir = 135;
  if (adc < 456) winddir = 158;
  if (adc < 414) winddir = 90;
  if (adc < 393) winddir = 68;
  if (adc < 380) winddir = 113;

  if(winddir > 180) winddir -= 180;
  else winddir += 180;
  if (windspeed == 0) winddir = 0;
  WRITE.println(winddir,DEC);
}
//-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=


void loop()
{
  //Keep track of which minute it is
  if(millis() - lastSecond >= MSECS_TO_SEC)
  {
    lastSecond += MSECS_TO_SEC;

    // Calculate all weather readings
    // Calc rain
    calc_rain();

    // Calc Wind
    calc_wind();
  }

  String command = "";
  if (READ.available() > 0) {
    command = READ.readStringUntil('\n');
    if (command.equalsIgnoreCase("H")) {
      // Atmospheric humidity (%)
      Humidity();
    } else if (command.equalsIgnoreCase("P")) {
      // Atmospheric presure at the observatory (Ascom needs hPa)
      // This must be the pressure at the observatory altitude and not the adjusted pressure at sea level.
      Pressure();     
    } else if (command.equalsIgnoreCase("RR")) {
      // Rain rate (Ascom needs mm/hour)
      // This property can be interpreted as 0.0 = Dry any positive nonzero value = wet.
      //   Rainfall intensity is classified according to the rate of precipitation:
      //   Light rain — when the precipitation rate is < 2.5 mm (0.098 in) per hour
      //   Moderate rain — when the precipitation rate is between 2.5 mm (0.098 in) - 7.6 mm (0.30 in) or 10 mm (0.39 in) per hour
      //   Heavy rain — when the precipitation rate is > 7.6 mm (0.30 in) per hour, or between 10 mm (0.39 in) and 50 mm (2.0 in) per hour
      //   Violent rain — when the precipitation rate is > 50 mm (2.0 in) per hour
      RainRate();
    } else if (command.equalsIgnoreCase("SB")) {
      // Sky brightness (Ascom needs Lux, but we are returning voltage. Ascom driver will have to be calibrated)
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
      SkyBrightness();      
    } else if (command.equalsIgnoreCase("ST")) {
      // Sky temperature in °C
      SkyTemperature();
    } else if (command.equalsIgnoreCase("T")) {
      // Temperature in °C
      Temperature();
    } else if (command.equalsIgnoreCase("WD")) {
      // Wind direction (degrees, 0..360.0) 
      // Value of 0.0 is returned when the wind speed is 0.0. 
      // Wind direction is measured clockwise from north, through east, 
      // where East=90.0, South=180.0, West=270.0 and North=360.0
      WindDirection();
    } else if (command.equalsIgnoreCase("WG")) {
      // Wind gust (Ascom needs m/s) Peak 3 second wind speed over the last 2 minutes
      WindGust();
    } else if (command.equalsIgnoreCase("WS")) {
      // Wind speed (Ascom needs m/s)
      WindSpeed();
    } else if (command.equalsIgnoreCase("RD")) {
      // Rain Detect through Hydreon RG11
      RainDetect();
    } else if (command.equalsIgnoreCase("CD")) {
      // Condensation Detect through HDS10
      CondensationDetect();
    } else if (command.equalsIgnoreCase("HD")) {
      HumidityDescription();
    } else if (command.equalsIgnoreCase("PD")) {
      PressureDescription();
    } else if (command.equalsIgnoreCase("STD")) {
      SkyTemperatureDescription();
    } else if (command.equalsIgnoreCase("TD")) {
      TemperatureDescription();
    } else if (command.equalsIgnoreCase("SBD")) {
      SkyBrightnessDescription();
    } else if (command.equalsIgnoreCase("PW")) {
      printWeather();
    }
  }
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


  // WindSpeed
  // Interrupt adds up the clicks. Each click is 1.492MPH of additional speed
  
  float deltaTime = millis() - lastWindCheck; //750ms
  lastWindCheck = millis();

  deltaTime /= MSECS_TO_SEC; //Convert to seconds

  windspeed = (float)windClicks / deltaTime; //3 / 0.750s = 4
  windClicks = 0; //Reset and start watching for new wind

  windspeed *= SPEED_PER_WINDCLICK; //4 * 1.492 = 5.968MPH

  // To calculate windgust we keep a 2 minute rolling history of instantaneous wingusts
  // Ascom needs 3 second gust over 2 minutes

  // Figure out the average windspeed over the last 3 seconds
  // Current is available from above calculations
  // Pas two are in the array windgust_calc
  windgusts[windgusts_index] = windspeed;
  for (int i = 0; i < WINDGUST_AVG_PERIOD - 1; i++)
    windgusts[windgusts_index] += windgust_calc[i];

  windgusts[windgusts_index] /= WINDGUST_AVG_PERIOD;
  
  // Store this windspeed in an array that covers 2 minutes
  windgust_calc[windgust_calc_index] = windspeed;

  // Increment the indices on a rolling dial basis
  windgust_calc_index = (windgust_calc_index + 1) % (WINDGUST_AVG_PERIOD - 1);
  windgusts_index = (windgusts_index + 1) % WINDGUST_MAX_PERIOD;
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
  WRITE.println("*********************");
  
  WRITE.print("H: ");
  Humidity();

  WRITE.print("P: ");
  Pressure();

  WRITE.print("RR: ");
  RainRate();

  WRITE.print("SB: ");
  SkyBrightness();

  WRITE.print("ST: ");
  SkyTemperature();

  // Calc Temp
  WRITE.println("T:");
  // Ambient from humidity sensor
  WRITE.print("->H: ");
  WRITE.println(myHumidity.readTemperature(),5);
  // Ambient from pressure sensor
  WRITE.print("->P: ");
  WRITE.println(myPressure.readTemp(),5);
  // Ambient from IR sensor
  WRITE.print("->IR: ");
  if (myIRSkyTemp.read()) // Read from the sensor
  { 
    // If the read is successful:
    // Get updated ambient temperature
    WRITE.println(myIRSkyTemp.ambient()); 
  } else 
  {
    WRITE.println("");
  }

  WRITE.print("WG: ");
  WindGust();

  WRITE.print("WS: ");
  WindSpeed();

  WRITE.print("WD: ");
  WindDirection();

  WRITE.print("RD: ");
  RainDetect();

  WRITE.print("CD: ");
  CondensationDetect();
  WRITE.println("*********************");
}


