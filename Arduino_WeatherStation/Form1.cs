using System;
using System.Windows.Forms;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.WindowsForms;
using System.Threading;


namespace ASCOM.Arduino
{
    public partial class Form1 : Form
    {
        private ASCOM.DriverAccess.ObservingConditions driver;
        private System.Threading.Thread t;
        private bool graphThreadRun = false;
        private bool graphThreadRunning = false;
        public Form1()
        {
            InitializeComponent();
            SetUIState();
        }
        private void updateGraphs()
        {
            // Set the run flag to true
            graphThreadRunning = true;

            // Temperature Plots
            var tempAxisText = "°c";
            if (fahrenheit.Checked)
            {
                tempAxisText = "°f";
            }

            // Ambient Temperature
            var tempSeries = new LineSeries();
            var temperature = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            temperature.Series.Add(tempSeries);
            temperature.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            temperature.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = tempAxisText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.tempPlot.Model = temperature;

            // Sky Temperature
            var skyTempSeries = new LineSeries();
            var skyTemperature = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            skyTemperature.Series.Add(skyTempSeries);
            skyTemperature.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            skyTemperature.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = tempAxisText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.skyTempPlot.Model = skyTemperature;

            // Temp Diff
            var tempDiffSeries = new LineSeries();
            var tempDiff = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            tempDiff.Series.Add(tempDiffSeries);
            tempDiff.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            tempDiff.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = tempAxisText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.tempDiffPlot.Model = tempDiff;

            // Wind items
            // Wind Speed

            var windUnitsText = "m/s";

            if (this.kph.Checked) windUnitsText = "kph";
            if (this.mph.Checked) windUnitsText = "mph";

            var windSpeedSeries = new LineSeries();
            var windSpeed = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            windSpeed.Series.Add(windSpeedSeries);
            windSpeed.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            windSpeed.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = windUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.windSpeedPlot.Model = windSpeed;

            // Wind Direction
            var windDirectionSeries = new LineSeries();
            var windDirection = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            windDirection.Series.Add(windDirectionSeries);
            windDirection.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            windDirection.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = "Degrees",
                AxislineStyle = LineStyle.Solid,
                MajorStep = 90,
                MinorStep = 15,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.windDirPlot.Model = windDirection;

            // Wind Speed
            var windGustSeries = new LineSeries();
            var windGust = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            windGust.Series.Add(windGustSeries);
            windGust.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            windGust.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = windUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.windGustPlot.Model = windGust;

            // Rain
            double rainMultiplier = 1;
            var rainUnitsText = "mm/Hr";

            if (this.inHr.Checked)
            {
                rainMultiplier = 0.0394;
                rainUnitsText = "in/Hr";
            }
            var rainSeries = new LineSeries();
            var rain = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            rain.Series.Add(rainSeries);
            rain.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            rain.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = rainUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.rainPlot.Model = rain;

            // Clouds
            var cloudsSeries = new LineSeries();
            var clouds = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            clouds.Series.Add(cloudsSeries);
            clouds.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            clouds.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = "%age",
                AxislineStyle = LineStyle.Solid,
                MajorStep = 10,
                MinorStep = 5,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.cloudsPlot.Model = clouds;

            // Sky Brightness
            var skyBrightnessSeries = new LineSeries();
            var skyBrightness = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            skyBrightness.Series.Add(skyBrightnessSeries);
            skyBrightness.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            skyBrightness.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = "Lux",
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5000,
                MinorStep = 1000,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.brightnessPlot.Model = skyBrightness;

            // Humidity
            var humiditySeries = new LineSeries();
            var humidity = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            humidity.Series.Add(humiditySeries);
            humidity.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            humidity.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = "%age",
                AxislineStyle = LineStyle.Solid,
                MajorStep = 10,
                MinorStep = 5,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.humidityPlot.Model = humidity;

            // Dew Point
            var dewPointSeries = new LineSeries();
            var dewPoint = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            dewPoint.Series.Add(dewPointSeries);
            dewPoint.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            dewPoint.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = tempAxisText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 5,
                MinorStep = 1,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.dewPtPlot.Model = dewPoint;

            // Pressure
            double pressureMult = 1;
            var pressureUnitsText = "hPa";

            if (this.pascals.Checked)
            {
                pressureMult *= 100;
                pressureUnitsText = "Pa";
            }

            if (this.kiloPascals.Checked)
            {
                pressureMult *= 0.1;
                pressureUnitsText = "kPa";
            }

            var pressureSeries = new LineSeries();
            var pressure = new PlotModel
            {
                PlotAreaBorderThickness = new OxyThickness(0)
            };
            pressure.Series.Add(pressureSeries);
            pressure.Axes.Add(new OxyPlot.Axes.DateTimeAxis
            {
                AxislineStyle = LineStyle.Solid,
                IntervalType = OxyPlot.Axes.DateTimeIntervalType.Minutes,
                MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Seconds,
                StringFormat = "hh:mm:ss",
                Title = "Time",
                TitleFontSize = 10,
                FontSize = 8,
            });
            pressure.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Title = pressureUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = 100,
                MinorStep = 10,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.pressurePlot.Model = pressure;

            // Start updating graphs
            while (graphThreadRun)
            {
                var now = DateTime.Now;

                var before = now.AddMinutes((double)((decimal)-100 * updateInterval.Value / 60));
                // Temperature series
                var temp = driver.Temperature;
                var skyTemp = driver.SkyTemperature;
                var dewPt = driver.DewPoint;

                if (fahrenheit.Checked)
                {
                    temp -= 32;
                    skyTemp -= 32;
                    dewPt -= 32;

                    temp *= 5 / 9;
                    skyTemp *= 5 / 9;
                    dewPt *= 5 / 9;
                }

                tempSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), temp));
                temperature.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.tempPlot.InvalidatePlot(true);

                skyTempSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), skyTemp));
                skyTemperature.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.skyTempPlot.InvalidatePlot(true);

                tempDiffSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), temp - skyTemp));
                tempDiff.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.tempDiffPlot.InvalidatePlot(true);

                //Wind Series
                var wSpeed = driver.WindSpeed;
                var wGust = driver.WindGust;

                if (this.kph.Checked)
                { 
                    wSpeed *= 3.6;
                    wGust *= 3.6;
                }

                if (this.mph.Checked)
                {
                    wSpeed *= 2.24;
                    wGust *= 2.24;
                }

                windSpeedSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), wSpeed));
                windSpeed.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windSpeedPlot.InvalidatePlot(true);

                windGustSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), wGust));
                windGust.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windGustPlot.InvalidatePlot(true);

                windDirectionSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.WindDirection));
                windDirection.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windDirPlot.InvalidatePlot(true);

                // Rain
                rainSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.RainRate * rainMultiplier));
                rain.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.rainPlot.InvalidatePlot(true);

                // Clouds
                cloudsSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.CloudCover));
                clouds.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.cloudsPlot.InvalidatePlot(true);

                // Sky Brightness
                skyBrightnessSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.SkyBrightness));
                skyBrightness.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.brightnessPlot.InvalidatePlot(true);

                // Humidity
                humiditySeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.Humidity));
                humidity.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.humidityPlot.InvalidatePlot(true);

                // Dew Point
                dewPointSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), dewPt));
                dewPoint.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.dewPtPlot.InvalidatePlot(true);

                // Pressure
                pressureSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), driver.Pressure * pressureMult));
                pressure.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.pressurePlot.InvalidatePlot(true);

                Thread.Sleep((int)this.updateInterval.Value * 1000);
            }

            // Indicate that this thread has not stopped
            graphThreadRunning = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsConnected)
                driver.Connected = false;

            Properties.Settings.Default.Save();
        }

        private void buttonChoose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.DriverId = ASCOM.DriverAccess.ObservingConditions.Choose(Properties.Settings.Default.DriverId);
            SetUIState();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (IsConnected)
            {
                // Disable the button till we are done disconnecting
                buttonConnect.Text = "Disconnecting, Please Wait...";
                buttonConnect.Enabled = false;
                
                // Tell the graphing thread to stop
                graphThreadRun = false;

                // Wait for the graphing thread to quit before disconnecting
                while (graphThreadRunning)
                {
                    // Check every second
                    Thread.Sleep(1000);
                }

                // Disconnect
                driver.Connected = false;

                // Enable all unit settings. Graph update thread will exit when driver is not connected
                centigrade.Enabled = true;
                fahrenheit.Enabled = true;
                pascals.Enabled = true;
                hectoPascals.Enabled = true;
                kiloPascals.Enabled = true;
                mmHr.Enabled = true;
                inHr.Enabled = true;
                ms.Enabled = true;
                kph.Enabled = true;
                mph.Enabled = true;
            }
            else
            {
                driver = new ASCOM.DriverAccess.ObservingConditions(Properties.Settings.Default.DriverId);
                driver.Connected = true;

                // Disable all unit settings before starting graph updates in a thread
                centigrade.Enabled = false;
                fahrenheit.Enabled = false;
                pascals.Enabled = false;
                hectoPascals.Enabled = false;
                kiloPascals.Enabled = false;
                mmHr.Enabled = false;
                inHr.Enabled = false;
                ms.Enabled = false;
                kph.Enabled = false;
                mph.Enabled = false;

                // Start thread to update graphs
                t = new System.Threading.Thread(updateGraphs);
                t.Start();

                // Ask graphing thread to update graphs
                graphThreadRun = true;
            }
            SetUIState();
        }

        private void SetUIState()
        {
            buttonConnect.Enabled = !string.IsNullOrEmpty(Properties.Settings.Default.DriverId);
            buttonChoose.Enabled = !IsConnected;
            buttonConnect.Text = IsConnected ? "Disconnect" : "Connect";
        }

        private bool IsConnected
        {
            get
            {
                return ((this.driver != null) && (driver.Connected == true));
            }
        }
    }
}
