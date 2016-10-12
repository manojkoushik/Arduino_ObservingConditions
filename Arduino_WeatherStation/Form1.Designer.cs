namespace ASCOM.Arduino
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tempDiffPlot = new OxyPlot.WindowsForms.PlotView();
            this.skyTempPlot = new OxyPlot.WindowsForms.PlotView();
            this.tempPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.windSpeedPlot = new OxyPlot.WindowsForms.PlotView();
            this.windGustPlot = new OxyPlot.WindowsForms.PlotView();
            this.windDirPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cloudsPlot = new OxyPlot.WindowsForms.PlotView();
            this.brightnessPlot = new OxyPlot.WindowsForms.PlotView();
            this.rainPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pressurePlot = new OxyPlot.WindowsForms.PlotView();
            this.dewPtPlot = new OxyPlot.WindowsForms.PlotView();
            this.humidityPlot = new OxyPlot.WindowsForms.PlotView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.pressureUnits = new System.Windows.Forms.GroupBox();
            this.hectoPascals = new System.Windows.Forms.RadioButton();
            this.kiloPascals = new System.Windows.Forms.RadioButton();
            this.pascals = new System.Windows.Forms.RadioButton();
            this.rainUnits = new System.Windows.Forms.GroupBox();
            this.inHr = new System.Windows.Forms.RadioButton();
            this.mmHr = new System.Windows.Forms.RadioButton();
            this.windUnits = new System.Windows.Forms.GroupBox();
            this.kph = new System.Windows.Forms.RadioButton();
            this.mph = new System.Windows.Forms.RadioButton();
            this.ms = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.updateInterval = new System.Windows.Forms.NumericUpDown();
            this.tempUnits = new System.Windows.Forms.GroupBox();
            this.fahrenheit = new System.Windows.Forms.RadioButton();
            this.centigrade = new System.Windows.Forms.RadioButton();
            this.labelDriverId = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonChoose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.pressureUnits.SuspendLayout();
            this.rainUnits.SuspendLayout();
            this.windUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateInterval)).BeginInit();
            this.tempUnits.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(0, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1922, 1495);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.tempDiffPlot);
            this.tabPage1.Controls.Add(this.skyTempPlot);
            this.tabPage1.Controls.Add(this.tempPlot);
            this.tabPage1.Location = new System.Drawing.Point(4, 43);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1914, 1448);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Temperature";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 957);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 32);
            this.label4.TabIndex = 5;
            this.label4.Text = "Diff";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 476);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 32);
            this.label3.TabIndex = 4;
            this.label3.Text = "Sky";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Ambient";
            // 
            // tempDiffPlot
            // 
            this.tempDiffPlot.Location = new System.Drawing.Point(20, 992);
            this.tempDiffPlot.Name = "tempDiffPlot";
            this.tempDiffPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.tempDiffPlot.Size = new System.Drawing.Size(1862, 435);
            this.tempDiffPlot.TabIndex = 2;
            this.tempDiffPlot.Text = "tempDifference";
            this.tempDiffPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.tempDiffPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.tempDiffPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // skyTempPlot
            // 
            this.skyTempPlot.Location = new System.Drawing.Point(20, 511);
            this.skyTempPlot.Name = "skyTempPlot";
            this.skyTempPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.skyTempPlot.Size = new System.Drawing.Size(1862, 435);
            this.skyTempPlot.TabIndex = 1;
            this.skyTempPlot.Text = "skyTemperature";
            this.skyTempPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.skyTempPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.skyTempPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tempPlot
            // 
            this.tempPlot.Location = new System.Drawing.Point(20, 38);
            this.tempPlot.Name = "tempPlot";
            this.tempPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.tempPlot.Size = new System.Drawing.Size(1862, 435);
            this.tempPlot.TabIndex = 0;
            this.tempPlot.Text = "Temperature";
            this.tempPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.tempPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.tempPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.windSpeedPlot);
            this.tabPage2.Controls.Add(this.windGustPlot);
            this.tabPage2.Controls.Add(this.windDirPlot);
            this.tabPage2.Location = new System.Drawing.Point(4, 43);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1914, 1448);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Wind";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(14, 957);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(147, 32);
            this.label7.TabIndex = 6;
            this.label7.Text = "Wind Gust";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 476);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(200, 32);
            this.label6.TabIndex = 5;
            this.label6.Text = "Wind Direction";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(170, 32);
            this.label5.TabIndex = 4;
            this.label5.Text = "Wind Speed";
            // 
            // windSpeedPlot
            // 
            this.windSpeedPlot.Location = new System.Drawing.Point(20, 38);
            this.windSpeedPlot.Name = "windSpeedPlot";
            this.windSpeedPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.windSpeedPlot.Size = new System.Drawing.Size(1862, 435);
            this.windSpeedPlot.TabIndex = 3;
            this.windSpeedPlot.Text = "WindSpeed";
            this.windSpeedPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.windSpeedPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.windSpeedPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // windGustPlot
            // 
            this.windGustPlot.Location = new System.Drawing.Point(20, 992);
            this.windGustPlot.Name = "windGustPlot";
            this.windGustPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.windGustPlot.Size = new System.Drawing.Size(1862, 435);
            this.windGustPlot.TabIndex = 2;
            this.windGustPlot.Text = "Wind Gust";
            this.windGustPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.windGustPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.windGustPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // windDirPlot
            // 
            this.windDirPlot.Location = new System.Drawing.Point(20, 511);
            this.windDirPlot.Name = "windDirPlot";
            this.windDirPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.windDirPlot.Size = new System.Drawing.Size(1862, 435);
            this.windDirPlot.TabIndex = 1;
            this.windDirPlot.Text = "Wind Direction";
            this.windDirPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.windDirPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.windDirPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.cloudsPlot);
            this.tabPage3.Controls.Add(this.brightnessPlot);
            this.tabPage3.Controls.Add(this.rainPlot);
            this.tabPage3.Location = new System.Drawing.Point(4, 43);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1914, 1448);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Clouds, Rain & Sky Brightness";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 957);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(204, 32);
            this.label10.TabIndex = 9;
            this.label10.Text = "Sky Brightness";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 476);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 32);
            this.label9.TabIndex = 8;
            this.label9.Text = "Rain";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 8);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(104, 32);
            this.label8.TabIndex = 7;
            this.label8.Text = "Clouds";
            // 
            // cloudsPlot
            // 
            this.cloudsPlot.Location = new System.Drawing.Point(20, 38);
            this.cloudsPlot.Name = "cloudsPlot";
            this.cloudsPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.cloudsPlot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cloudsPlot.Size = new System.Drawing.Size(1862, 435);
            this.cloudsPlot.TabIndex = 6;
            this.cloudsPlot.Text = "Clouds";
            this.cloudsPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.cloudsPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.cloudsPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // brightnessPlot
            // 
            this.brightnessPlot.Location = new System.Drawing.Point(20, 992);
            this.brightnessPlot.Name = "brightnessPlot";
            this.brightnessPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.brightnessPlot.Size = new System.Drawing.Size(1862, 435);
            this.brightnessPlot.TabIndex = 5;
            this.brightnessPlot.Text = "Sky Brightness";
            this.brightnessPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.brightnessPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.brightnessPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // rainPlot
            // 
            this.rainPlot.Location = new System.Drawing.Point(20, 511);
            this.rainPlot.Name = "rainPlot";
            this.rainPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.rainPlot.Size = new System.Drawing.Size(1862, 435);
            this.rainPlot.TabIndex = 4;
            this.rainPlot.Text = "Rain";
            this.rainPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.rainPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.rainPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.label13);
            this.tabPage5.Controls.Add(this.label12);
            this.tabPage5.Controls.Add(this.label11);
            this.tabPage5.Controls.Add(this.pressurePlot);
            this.tabPage5.Controls.Add(this.dewPtPlot);
            this.tabPage5.Controls.Add(this.humidityPlot);
            this.tabPage5.Location = new System.Drawing.Point(4, 43);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1914, 1448);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Humidity & Pressure";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 957);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(128, 32);
            this.label13.TabIndex = 6;
            this.label13.Text = "Pressure";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 476);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(144, 32);
            this.label12.TabIndex = 5;
            this.label12.Text = "Dew Point";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 8);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(126, 32);
            this.label11.TabIndex = 4;
            this.label11.Text = "Humidity";
            // 
            // pressurePlot
            // 
            this.pressurePlot.Location = new System.Drawing.Point(20, 992);
            this.pressurePlot.Name = "pressurePlot";
            this.pressurePlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.pressurePlot.Size = new System.Drawing.Size(1862, 435);
            this.pressurePlot.TabIndex = 3;
            this.pressurePlot.Text = "Pressure";
            this.pressurePlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.pressurePlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.pressurePlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // dewPtPlot
            // 
            this.dewPtPlot.Location = new System.Drawing.Point(20, 511);
            this.dewPtPlot.Name = "dewPtPlot";
            this.dewPtPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.dewPtPlot.Size = new System.Drawing.Size(1862, 435);
            this.dewPtPlot.TabIndex = 2;
            this.dewPtPlot.Text = "Dew Point";
            this.dewPtPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.dewPtPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.dewPtPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // humidityPlot
            // 
            this.humidityPlot.Location = new System.Drawing.Point(22, 38);
            this.humidityPlot.Name = "humidityPlot";
            this.humidityPlot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.humidityPlot.Size = new System.Drawing.Size(1862, 435);
            this.humidityPlot.TabIndex = 1;
            this.humidityPlot.Text = "Humidity";
            this.humidityPlot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.humidityPlot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.humidityPlot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.pressureUnits);
            this.tabPage4.Controls.Add(this.rainUnits);
            this.tabPage4.Controls.Add(this.windUnits);
            this.tabPage4.Controls.Add(this.label1);
            this.tabPage4.Controls.Add(this.updateInterval);
            this.tabPage4.Controls.Add(this.tempUnits);
            this.tabPage4.Controls.Add(this.labelDriverId);
            this.tabPage4.Controls.Add(this.buttonConnect);
            this.tabPage4.Controls.Add(this.buttonChoose);
            this.tabPage4.Location = new System.Drawing.Point(4, 43);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1914, 1448);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Connection Settings";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // pressureUnits
            // 
            this.pressureUnits.Controls.Add(this.hectoPascals);
            this.pressureUnits.Controls.Add(this.kiloPascals);
            this.pressureUnits.Controls.Add(this.pascals);
            this.pressureUnits.Location = new System.Drawing.Point(122, 291);
            this.pressureUnits.Name = "pressureUnits";
            this.pressureUnits.Size = new System.Drawing.Size(1669, 100);
            this.pressureUnits.TabIndex = 7;
            this.pressureUnits.TabStop = false;
            this.pressureUnits.Text = "Pressure Units";
            // 
            // hectoPascals
            // 
            this.hectoPascals.AutoSize = true;
            this.hectoPascals.Checked = true;
            this.hectoPascals.Location = new System.Drawing.Point(641, 32);
            this.hectoPascals.Name = "hectoPascals";
            this.hectoPascals.Size = new System.Drawing.Size(233, 36);
            this.hectoPascals.TabIndex = 3;
            this.hectoPascals.TabStop = true;
            this.hectoPascals.Text = "Hecto Pascals";
            this.hectoPascals.UseVisualStyleBackColor = true;
            // 
            // kiloPascals
            // 
            this.kiloPascals.AutoSize = true;
            this.kiloPascals.Location = new System.Drawing.Point(1130, 32);
            this.kiloPascals.Name = "kiloPascals";
            this.kiloPascals.Size = new System.Drawing.Size(208, 36);
            this.kiloPascals.TabIndex = 2;
            this.kiloPascals.Text = "Kilo Pascals";
            this.kiloPascals.UseVisualStyleBackColor = true;
            // 
            // pascals
            // 
            this.pascals.AutoSize = true;
            this.pascals.Location = new System.Drawing.Point(276, 37);
            this.pascals.Name = "pascals";
            this.pascals.Size = new System.Drawing.Size(152, 36);
            this.pascals.TabIndex = 1;
            this.pascals.Text = "Pascals";
            this.pascals.UseVisualStyleBackColor = true;
            // 
            // rainUnits
            // 
            this.rainUnits.Controls.Add(this.inHr);
            this.rainUnits.Controls.Add(this.mmHr);
            this.rainUnits.Location = new System.Drawing.Point(122, 489);
            this.rainUnits.Name = "rainUnits";
            this.rainUnits.Size = new System.Drawing.Size(1669, 100);
            this.rainUnits.TabIndex = 7;
            this.rainUnits.TabStop = false;
            this.rainUnits.Text = "Rain Units";
            // 
            // inHr
            // 
            this.inHr.AutoSize = true;
            this.inHr.Location = new System.Drawing.Point(928, 32);
            this.inHr.Name = "inHr";
            this.inHr.Size = new System.Drawing.Size(112, 36);
            this.inHr.TabIndex = 3;
            this.inHr.Text = "in/Hr";
            this.inHr.UseVisualStyleBackColor = true;
            // 
            // mmHr
            // 
            this.mmHr.AutoSize = true;
            this.mmHr.Checked = true;
            this.mmHr.Location = new System.Drawing.Point(552, 32);
            this.mmHr.Name = "mmHr";
            this.mmHr.Size = new System.Drawing.Size(135, 36);
            this.mmHr.TabIndex = 2;
            this.mmHr.TabStop = true;
            this.mmHr.Text = "mm/Hr";
            this.mmHr.UseVisualStyleBackColor = true;
            // 
            // windUnits
            // 
            this.windUnits.Controls.Add(this.kph);
            this.windUnits.Controls.Add(this.mph);
            this.windUnits.Controls.Add(this.ms);
            this.windUnits.Location = new System.Drawing.Point(122, 683);
            this.windUnits.Name = "windUnits";
            this.windUnits.Size = new System.Drawing.Size(1669, 100);
            this.windUnits.TabIndex = 7;
            this.windUnits.TabStop = false;
            this.windUnits.Text = "Wind Units";
            // 
            // kph
            // 
            this.kph.AutoSize = true;
            this.kph.Location = new System.Drawing.Point(668, 30);
            this.kph.Name = "kph";
            this.kph.Size = new System.Drawing.Size(98, 36);
            this.kph.TabIndex = 6;
            this.kph.Text = "kph";
            this.kph.UseVisualStyleBackColor = true;
            // 
            // mph
            // 
            this.mph.AutoSize = true;
            this.mph.Location = new System.Drawing.Point(1157, 30);
            this.mph.Name = "mph";
            this.mph.Size = new System.Drawing.Size(107, 36);
            this.mph.TabIndex = 5;
            this.mph.Text = "mph";
            this.mph.UseVisualStyleBackColor = true;
            // 
            // ms
            // 
            this.ms.AutoSize = true;
            this.ms.Checked = true;
            this.ms.Location = new System.Drawing.Point(303, 35);
            this.ms.Name = "ms";
            this.ms.Size = new System.Drawing.Size(97, 36);
            this.ms.TabIndex = 4;
            this.ms.TabStop = true;
            this.ms.Text = "m/s";
            this.ms.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(654, 892);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(207, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Update Interval";
            // 
            // updateInterval
            // 
            this.updateInterval.Location = new System.Drawing.Point(964, 886);
            this.updateInterval.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.updateInterval.Minimum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.updateInterval.Name = "updateInterval";
            this.updateInterval.Size = new System.Drawing.Size(148, 38);
            this.updateInterval.TabIndex = 7;
            this.updateInterval.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // tempUnits
            // 
            this.tempUnits.Controls.Add(this.fahrenheit);
            this.tempUnits.Controls.Add(this.centigrade);
            this.tempUnits.Location = new System.Drawing.Point(122, 66);
            this.tempUnits.Name = "tempUnits";
            this.tempUnits.Size = new System.Drawing.Size(1669, 100);
            this.tempUnits.TabIndex = 6;
            this.tempUnits.TabStop = false;
            this.tempUnits.Text = "Temperature Units";
            // 
            // fahrenheit
            // 
            this.fahrenheit.AutoSize = true;
            this.fahrenheit.Location = new System.Drawing.Point(842, 37);
            this.fahrenheit.Name = "fahrenheit";
            this.fahrenheit.Size = new System.Drawing.Size(189, 36);
            this.fahrenheit.TabIndex = 1;
            this.fahrenheit.Text = "Fahrenheit";
            this.fahrenheit.UseVisualStyleBackColor = true;
            // 
            // centigrade
            // 
            this.centigrade.AutoSize = true;
            this.centigrade.Checked = true;
            this.centigrade.Location = new System.Drawing.Point(466, 37);
            this.centigrade.Name = "centigrade";
            this.centigrade.Size = new System.Drawing.Size(192, 36);
            this.centigrade.TabIndex = 0;
            this.centigrade.TabStop = true;
            this.centigrade.Text = "Centigrade";
            this.centigrade.UseVisualStyleBackColor = true;
            // 
            // labelDriverId
            // 
            this.labelDriverId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelDriverId.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ASCOM.Arduino.Properties.Settings.Default, "DriverId", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.labelDriverId.Location = new System.Drawing.Point(122, 1009);
            this.labelDriverId.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.labelDriverId.Name = "labelDriverId";
            this.labelDriverId.Size = new System.Drawing.Size(1669, 47);
            this.labelDriverId.TabIndex = 5;
            this.labelDriverId.Text = global::ASCOM.Arduino.Properties.Settings.Default.DriverId;
            this.labelDriverId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(1252, 1149);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(548, 131);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonChoose
            // 
            this.buttonChoose.Location = new System.Drawing.Point(131, 1149);
            this.buttonChoose.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.buttonChoose.Name = "buttonChoose";
            this.buttonChoose.Size = new System.Drawing.Size(548, 131);
            this.buttonChoose.TabIndex = 3;
            this.buttonChoose.Text = "Choose";
            this.buttonChoose.UseVisualStyleBackColor = true;
            this.buttonChoose.Click += new System.EventHandler(this.buttonChoose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1934, 1502);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Form1";
            this.Text = "Arduino Weather Station";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.pressureUnits.ResumeLayout(false);
            this.pressureUnits.PerformLayout();
            this.rainUnits.ResumeLayout(false);
            this.rainUnits.PerformLayout();
            this.windUnits.ResumeLayout(false);
            this.windUnits.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.updateInterval)).EndInit();
            this.tempUnits.ResumeLayout(false);
            this.tempUnits.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label labelDriverId;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonChoose;
        private System.Windows.Forms.TabPage tabPage5;
        private OxyPlot.WindowsForms.PlotView tempDiffPlot;
        private OxyPlot.WindowsForms.PlotView skyTempPlot;
        private OxyPlot.WindowsForms.PlotView tempPlot;
        private OxyPlot.WindowsForms.PlotView windSpeedPlot;
        private OxyPlot.WindowsForms.PlotView windGustPlot;
        private OxyPlot.WindowsForms.PlotView windDirPlot;
        private OxyPlot.WindowsForms.PlotView pressurePlot;
        private OxyPlot.WindowsForms.PlotView dewPtPlot;
        private OxyPlot.WindowsForms.PlotView humidityPlot;
        private OxyPlot.WindowsForms.PlotView cloudsPlot;
        private OxyPlot.WindowsForms.PlotView brightnessPlot;
        private OxyPlot.WindowsForms.PlotView rainPlot;
        private System.Windows.Forms.GroupBox pressureUnits;
        private System.Windows.Forms.RadioButton hectoPascals;
        private System.Windows.Forms.RadioButton kiloPascals;
        private System.Windows.Forms.RadioButton pascals;
        private System.Windows.Forms.GroupBox rainUnits;
        private System.Windows.Forms.RadioButton inHr;
        private System.Windows.Forms.RadioButton mmHr;
        private System.Windows.Forms.GroupBox windUnits;
        private System.Windows.Forms.RadioButton kph;
        private System.Windows.Forms.RadioButton mph;
        private System.Windows.Forms.RadioButton ms;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown updateInterval;
        private System.Windows.Forms.GroupBox tempUnits;
        private System.Windows.Forms.RadioButton fahrenheit;
        private System.Windows.Forms.RadioButton centigrade;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
    }
}

