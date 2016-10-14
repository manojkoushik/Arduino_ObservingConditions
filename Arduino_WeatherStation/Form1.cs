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


            #region temperature

            // Temperature Plots
            // Used for cloud calibration

            double lowestTempDiff = 100;
            double highestTempDiff = 0;

            double highestTemp = double.MinValue;
            double lowestTemp = double.MaxValue;

            double highestSkyTemp = double.MinValue;
            double lowestSkyTemp = double.MaxValue;

            var tempUnitsText = "°c";
            double tempMajorStep = 5;
            double tempMinorStep = 1;

            if (fahrenheit.Checked)
            {
                tempUnitsText = "°f";
                tempMajorStep = 1;
                tempMinorStep = 0.1;
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
                Title = tempUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = tempMajorStep,
                MinorStep = tempMinorStep,
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
                Title = tempUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = tempMajorStep,
                MinorStep = tempMinorStep,
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
                Title = tempUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = tempMajorStep,
                MinorStep = tempMinorStep,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.tempDiffPlot.Model = tempDiff;
            #endregion

            #region wind

            // Wind items
            // Wind Speed

            var windUnitsText = "m/s";
            double windMajorStep = 5;
            double windMinorStep = 1;

            double windSpeedMax = double.MinValue;
            double windSpeedMin = double.MaxValue;
            double windGustMax = double.MinValue;
            double windGustMin = double.MaxValue;

            if (this.kph.Checked)
            {
                windUnitsText = "kph";
                windMajorStep = 15;
                windMinorStep = 5;
            }

            if (this.mph.Checked)
            {
                windUnitsText = "mph";
                windMajorStep = 10;
                windMinorStep = 2;
            }

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
                MajorStep = windMajorStep,
                MinorStep = windMinorStep,
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
                Maximum = 360,
                Minimum = 0,
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
                MajorStep = windMajorStep,
                MinorStep = windMinorStep,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.windGustPlot.Model = windGust;

            #endregion

            #region rain

            // Rain
            double rainMultiplier = 1;
            var rainUnitsText = "mm/Hr";
            double rainMajorStep = 4;
            double rainMinorStep = 1;
            double rainMax = double.MinValue;
            double rainMin = double.MaxValue;

            if (this.inHr.Checked)
            {
                rainMultiplier = 0.0394;
                rainUnitsText = "in/Hr";
                rainMajorStep = 0.16;
                rainMinorStep = 0.04;
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
                MajorStep = rainMajorStep,
                MinorStep = rainMinorStep,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.rainPlot.Model = rain;
            #endregion

            #region clouds
            // Clouds

            double cloudsMax = double.MinValue;
            double cloudsMin = double.MaxValue;

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
                Maximum = 100,
                Minimum = 0,
            });
            this.cloudsPlot.Model = clouds;
            #endregion

            #region skybrightness
            // Sky Brightness

            double skyBrightnessMax = double.MinValue;
            double skyBrightnessMin = double.MaxValue;
            double sbMajorStep = 5000;
            double sbMinorStep = 1000;

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
                MajorStep = sbMajorStep,
                MinorStep = sbMinorStep,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
                Maximum = 25000,
                Minimum = 0,
            });
            this.brightnessPlot.Model = skyBrightness;
            #endregion

            #region humidity
            // Humidity

            double humidityMax = double.MinValue;
            double humidityMin = double.MaxValue;

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
                Maximum = 100,
                Minimum = 0,
            });
            this.humidityPlot.Model = humidity;
            #endregion

            #region dewpt
            // Dew Point

            double dewPtMax = double.MinValue;
            double dewPtMin = double.MaxValue;

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
                Title = tempUnitsText,
                AxislineStyle = LineStyle.Solid,
                MajorStep = tempMajorStep,
                MinorStep = tempMinorStep,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.dewPtPlot.Model = dewPoint;
            #endregion

            #region pressure
            // Pressure
            double pressureMult = 1;
            var pressureUnitsText = "hPa";
            double pressureMax = double.MinValue;
            double pressureMin = double.MaxValue;

            if (this.pascals.Checked)
            {
                pressureMult = 100;
                pressureUnitsText = "Pa";
            }

            if (this.kiloPascals.Checked)
            {
                pressureMult = 0.1;
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
                MajorStep = 100*pressureMult,
                MinorStep = 10*pressureMult,
                MajorGridlineStyle = LineStyle.Dash,
                MinorGridlineStyle = LineStyle.Dot,
            });
            this.pressurePlot.Model = pressure;
            #endregion

            // give ourselves some time so arduino is connected
            Thread.Sleep(3000);

            #region graph update loop
            // Start updating graphs
            while (graphThreadRun)
            {
                var now = DateTime.Now;

                var before = now.AddMinutes((double)((decimal)-100 * updateInterval.Value / 60));

                #region temp
                // Temperature series
                double temp = driver.Temperature;
                double skyTemp = driver.SkyTemperature;
                double dewPt = driver.DewPoint;

                if (fahrenheit.Checked)
                {
                    temp *= 1.8;
                    skyTemp *= 1.8;
                    dewPt *= 1.8;

                    temp += 32;
                    skyTemp += 32;
                    dewPt += 32;
                }

                if (temp < lowestTemp) lowestTemp = temp;
                if (temp > highestTemp) highestTemp = temp;

                if (skyTemp < lowestSkyTemp) lowestSkyTemp = skyTemp;
                if (skyTemp > highestSkyTemp) highestSkyTemp = skyTemp;

                double diff = temp - skyTemp;
                if (diff < lowestTempDiff) lowestTempDiff = diff;
                if (diff > highestTempDiff) highestTempDiff = diff;

                tempDiff.LegendTitle = "Cur:"+diff.ToString("F2") + tempUnitsText +
                    " Low:" + lowestTempDiff.ToString("F2") + tempUnitsText + 
                    " High:" + highestTempDiff.ToString("F2") + tempUnitsText;
                tempDiff.Axes[1].Maximum = highestTempDiff + tempMajorStep;
                tempDiff.Axes[1].Minimum = lowestTempDiff - tempMajorStep;

                temperature.LegendTitle = "Cur:" + temp.ToString("F2") + tempUnitsText +
                    " Low:" + lowestTemp.ToString("F2") + tempUnitsText + 
                    " High:" + highestTemp.ToString("F2") + tempUnitsText;
                temperature.Axes[1].Maximum = highestTemp + tempMajorStep;
                temperature.Axes[1].Minimum = lowestTemp - tempMajorStep;

                skyTemperature.LegendTitle = "Cur:" + skyTemp.ToString("F2") + tempUnitsText + 
                    " Low:" + lowestSkyTemp.ToString("F2") + tempUnitsText + 
                    " High:" + highestSkyTemp.ToString("F2") + tempUnitsText;
                skyTemperature.Axes[1].Maximum = highestSkyTemp + tempMajorStep;
                skyTemperature.Axes[1].Minimum = lowestSkyTemp - tempMajorStep;

                tempSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), temp));
                temperature.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.tempPlot.InvalidatePlot(true);

                skyTempSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), skyTemp));
                skyTemperature.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.skyTempPlot.InvalidatePlot(true);

                tempDiffSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), diff));
                tempDiff.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.tempDiffPlot.InvalidatePlot(true);
                #endregion

                #region wind
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

                if (wSpeed < windSpeedMin) windSpeedMin = wSpeed;
                if (wSpeed > windSpeedMax) windSpeedMax = wSpeed;

                if (wGust < windGustMin) windGustMin = wGust;
                if (wSpeed > windGustMax) windGustMax = wGust;

                windSpeed.LegendTitle = "Cur:" + wSpeed.ToString("F2") + windUnitsText +
                    " Low:" + windSpeedMin.ToString("F2") + windUnitsText  + 
                    " High:" + windSpeedMax.ToString("F2") + windUnitsText;
                windSpeed.Axes[1].Maximum = windSpeedMax + windMajorStep;
                windSpeed.Axes[1].Minimum = windSpeedMin - windMinorStep;

                windGust.LegendTitle = "Cur:" + wGust.ToString("F2") + windUnitsText +
                    " Low:" + windGustMin.ToString("F2") + windUnitsText + 
                    " High:" + windGustMax.ToString("F2") + windUnitsText;
                windGust.Axes[1].Maximum = windGustMax + windMajorStep;
                windGust.Axes[1].Minimum = windGustMin - windMinorStep;

                windSpeedSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), wSpeed));
                windSpeed.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windSpeedPlot.InvalidatePlot(true);

                windGustSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), wGust));
                windGust.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windGustPlot.InvalidatePlot(true);

                double wDir = driver.WindDirection;
                windDirection.LegendTitle = "Cur:" + wDir.ToString("F2") + "°";

                windDirectionSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), wDir));
                windDirection.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.windDirPlot.InvalidatePlot(true);
                #endregion

                #region rain
                // Rain

                var rainVal = driver.RainRate * rainMultiplier;

                if (rainVal > rainMax) rainMax = rainVal;
                if (rainVal < rainMin) rainMin = rainVal;

                rain.LegendTitle = "Cur:" + rainVal.ToString("F2") + rainUnitsText +
                    " Low:" + rainMin.ToString("F2") + rainUnitsText + 
                    " High:" + rainMax.ToString("F2") + rainUnitsText;
                rain.Axes[1].Maximum = rainMax + rainMajorStep;
                rain.Axes[1].Minimum = rainMin - rainMinorStep;

                rainSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), rainVal));
                rain.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.rainPlot.InvalidatePlot(true);
                #endregion

                #region clouds
                // Clouds

                double c = driver.CloudCover;

                if (c > cloudsMax) cloudsMax = c;
                if (c < cloudsMin) cloudsMin = c;

                clouds.LegendTitle = "Cur:" + c.ToString("F2") +
                    "% Low:" + cloudsMin.ToString("F2") + 
                    "% High:" + cloudsMax.ToString("F2") + "%";

                cloudsSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), c));
                clouds.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.cloudsPlot.InvalidatePlot(true);
                #endregion

                #region sky brightness
                // Sky Brightness
                double sb = driver.SkyBrightness;

                if (sb > skyBrightnessMax) skyBrightnessMax = sb;
                if (sb < skyBrightnessMin) skyBrightnessMin = sb;

                skyBrightness.LegendTitle = "Cur:" + sb.ToString("F2") + 
                    "Lux Low:" + skyBrightnessMin.ToString("F2") + 
                    "Lux High:" + skyBrightnessMax.ToString("F2") + "Lux";
                skyBrightness.Axes[1].Maximum = skyBrightnessMax + sbMajorStep;
                skyBrightness.Axes[1].Minimum =  skyBrightnessMin - sbMajorStep;

                skyBrightnessSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), sb));
                skyBrightness.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.brightnessPlot.InvalidatePlot(true);
                #endregion

                #region humidity
                // Humidity
                double h = driver.Humidity;

                if (h > humidityMax) humidityMax = h;
                if (h < humidityMin) humidityMin = h;

                humidity.LegendTitle = "Cur:" + h.ToString("F2") +
                    "% Low:" + humidityMin.ToString("F2") + 
                    "% High:" + humidityMax.ToString("F2") + "%";

                humiditySeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), h));
                humidity.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.humidityPlot.InvalidatePlot(true);
                #endregion

                #region dew point
                // Dew Point

                if (dewPt < dewPtMin) dewPtMin = dewPt;
                if (dewPt > dewPtMax) dewPtMax = dewPt;

                dewPoint.LegendTitle = "Cur:" + dewPt.ToString("F2") + tempUnitsText +
                    " Low:" + dewPtMin.ToString("F2") + tempUnitsText + 
                    " High:" + dewPtMax.ToString("F2") + tempUnitsText;
                dewPoint.Axes[1].Maximum = dewPtMax + tempMajorStep;
                dewPoint.Axes[1].Minimum = dewPtMin - tempMajorStep;

                dewPointSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), dewPt));
                dewPoint.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.dewPtPlot.InvalidatePlot(true);
                #endregion

                #region pressure
                // Pressure

                double p = driver.Pressure;

                p *= pressureMult;

                if (p < pressureMin) pressureMin = p;
                if (p > pressureMax) pressureMax = p;

                pressure.LegendTitle = "Cur:" + p.ToString("F2") + pressureUnitsText + 
                    " Low:" + pressureMin.ToString("F2") + pressureUnitsText + 
                    " High:" + pressureMax.ToString("F2") + pressureUnitsText;
                pressure.Axes[1].Maximum = pressureMax + 100*pressureMult;
                pressure.Axes[1].Minimum = pressureMin - 100*pressureMult;

                pressureSeries.Points.Add(new DataPoint(OxyPlot.Axes.DateTimeAxis.ToDouble(now), p));
                pressure.Axes[0].Minimum = OxyPlot.Axes.DateTimeAxis.ToDouble(before);
                this.pressurePlot.InvalidatePlot(true);
                #endregion

                Thread.Sleep((int)this.updateInterval.Value * 1000);
            }
            #endregion

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
