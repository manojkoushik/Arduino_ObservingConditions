namespace ASCOM.Arduino
{
    partial class SetupDialogForm
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.picASCOM = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkTrace = new System.Windows.Forms.CheckBox();
            this.comboBoxComPort = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox7 = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox9 = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.textBox10 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textBox11 = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.textBox12 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.textBox13 = new System.Windows.Forms.TextBox();
            this.chkBWF = new System.Windows.Forms.CheckBox();
            this.chkHDS10 = new System.Windows.Forms.CheckBox();
            this.chkRG11 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(478, 685);
            this.cmdOK.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(157, 57);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(1151, 682);
            this.cmdCancel.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(157, 60);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // picASCOM
            // 
            this.picASCOM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.picASCOM.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picASCOM.Image = global::ASCOM.Arduino.Properties.Resources.ASCOM;
            this.picASCOM.Location = new System.Drawing.Point(1482, 21);
            this.picASCOM.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.picASCOM.Name = "picASCOM";
            this.picASCOM.Size = new System.Drawing.Size(48, 56);
            this.picASCOM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picASCOM.TabIndex = 3;
            this.picASCOM.TabStop = false;
            this.picASCOM.Click += new System.EventHandler(this.BrowseToAscom);
            this.picASCOM.DoubleClick += new System.EventHandler(this.BrowseToAscom);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(278, 52);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 32);
            this.label2.TabIndex = 5;
            this.label2.Text = "Comm Port";
            // 
            // chkTrace
            // 
            this.chkTrace.AutoSize = true;
            this.chkTrace.Location = new System.Drawing.Point(74, 618);
            this.chkTrace.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chkTrace.Name = "chkTrace";
            this.chkTrace.Size = new System.Drawing.Size(180, 36);
            this.chkTrace.TabIndex = 6;
            this.chkTrace.Text = "Trace on?";
            this.chkTrace.UseVisualStyleBackColor = true;
            // 
            // comboBoxComPort
            // 
            this.comboBoxComPort.FormattingEnabled = true;
            this.comboBoxComPort.Location = new System.Drawing.Point(478, 45);
            this.comboBoxComPort.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.comboBoxComPort.Name = "comboBoxComPort";
            this.comboBoxComPort.Size = new System.Drawing.Size(233, 39);
            this.comboBoxComPort.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(829, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(287, 32);
            this.label1.TabIndex = 8;
            this.label1.Text = "Update Frequency (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(393, 32);
            this.label3.TabIndex = 9;
            this.label3.Text = "Cloudy Skies Temp Offset (°c)";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(1152, 46);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(233, 38);
            this.numericUpDown1.TabIndex = 10;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(478, 121);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(233, 38);
            this.textBox1.TabIndex = 11;
            this.textBox1.Text = "5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(747, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(372, 32);
            this.label4.TabIndex = 12;
            this.label4.Text = "Clear Skies Temp Offset (°c)";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(1151, 124);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(233, 38);
            this.textBox2.TabIndex = 13;
            this.textBox2.Text = "15";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(1151, 268);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(233, 38);
            this.textBox3.TabIndex = 17;
            this.textBox3.Text = "5";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(895, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(173, 32);
            this.label5.TabIndex = 16;
            this.label5.Text = "Max Voltage";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(478, 268);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(233, 38);
            this.textBox4.TabIndex = 15;
            this.textBox4.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(178, 268);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 32);
            this.label6.TabIndex = 14;
            this.label6.Text = "Min Voltage";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(1151, 195);
            this.textBox5.Name = "textBox5";
            this.textBox5.Size = new System.Drawing.Size(233, 38);
            this.textBox5.TabIndex = 21;
            this.textBox5.Text = "80";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(802, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(317, 32);
            this.label7.TabIndex = 20;
            this.label7.Text = "%age when VeryCloudy";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(478, 192);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(233, 38);
            this.textBox6.TabIndex = 19;
            this.textBox6.Text = "40";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(217, 192);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(217, 32);
            this.label8.TabIndex = 18;
            this.label8.Text = "% When Cloudy";
            // 
            // textBox7
            // 
            this.textBox7.Location = new System.Drawing.Point(1151, 469);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(233, 38);
            this.textBox7.TabIndex = 25;
            this.textBox7.Text = "14";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(750, 472);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(369, 32);
            this.label9.TabIndex = 24;
            this.label9.Text = "Wind when VeryWindy (m/s)";
            // 
            // textBox8
            // 
            this.textBox8.Location = new System.Drawing.Point(478, 466);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(233, 38);
            this.textBox8.TabIndex = 23;
            this.textBox8.Text = "7";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(123, 466);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(311, 32);
            this.label10.TabIndex = 22;
            this.label10.Text = "Wind when Windy (m/s)";
            // 
            // textBox9
            // 
            this.textBox9.Location = new System.Drawing.Point(1152, 337);
            this.textBox9.Name = "textBox9";
            this.textBox9.Size = new System.Drawing.Size(233, 38);
            this.textBox9.TabIndex = 29;
            this.textBox9.Text = "25000";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(944, 337);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 32);
            this.label11.TabIndex = 28;
            this.label11.Text = "Max Lux";
            // 
            // textBox10
            // 
            this.textBox10.Location = new System.Drawing.Point(478, 331);
            this.textBox10.Name = "textBox10";
            this.textBox10.Size = new System.Drawing.Size(233, 38);
            this.textBox10.TabIndex = 27;
            this.textBox10.Text = "0";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(230, 331);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(114, 32);
            this.label12.TabIndex = 26;
            this.label12.Text = "Min Lux";
            // 
            // textBox11
            // 
            this.textBox11.Location = new System.Drawing.Point(478, 400);
            this.textBox11.Name = "textBox11";
            this.textBox11.Size = new System.Drawing.Size(233, 38);
            this.textBox11.TabIndex = 31;
            this.textBox11.Text = "1";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(223, 400);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(159, 32);
            this.label13.TabIndex = 30;
            this.label13.Text = "Twlight Lux";
            // 
            // textBox12
            // 
            this.textBox12.Location = new System.Drawing.Point(1151, 400);
            this.textBox12.Name = "textBox12";
            this.textBox12.Size = new System.Drawing.Size(233, 38);
            this.textBox12.TabIndex = 33;
            this.textBox12.Text = "4";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(908, 406);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(172, 32);
            this.label14.TabIndex = 32;
            this.label14.Text = "Daylight Lux";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(30, 542);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(652, 32);
            this.label15.TabIndex = 34;
            this.label15.Text = "Boltwood File location (will create ArduinoBWF.txt)";
            // 
            // textBox13
            // 
            this.textBox13.Location = new System.Drawing.Point(727, 535);
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new System.Drawing.Size(657, 38);
            this.textBox13.TabIndex = 35;
            this.textBox13.Click += new System.EventHandler(this.textBox13_Clicked);
            // 
            // chkBWF
            // 
            this.chkBWF.AutoSize = true;
            this.chkBWF.Location = new System.Drawing.Point(309, 618);
            this.chkBWF.Name = "chkBWF";
            this.chkBWF.Size = new System.Drawing.Size(373, 36);
            this.chkBWF.TabIndex = 36;
            this.chkBWF.Text = "Generate BoltWood File?";
            this.chkBWF.UseVisualStyleBackColor = true;
            // 
            // chkHDS10
            // 
            this.chkHDS10.AutoSize = true;
            this.chkHDS10.Location = new System.Drawing.Point(1156, 618);
            this.chkHDS10.Name = "chkHDS10";
            this.chkHDS10.Size = new System.Drawing.Size(374, 36);
            this.chkHDS10.TabIndex = 38;
            this.chkHDS10.Text = "Use HDS10 for Wetness?";
            this.chkHDS10.UseVisualStyleBackColor = true;
            // 
            // chkRG11
            // 
            this.chkRG11.AutoSize = true;
            this.chkRG11.Location = new System.Drawing.Point(765, 618);
            this.chkRG11.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.chkRG11.Name = "chkRG11";
            this.chkRG11.Size = new System.Drawing.Size(315, 36);
            this.chkRG11.TabIndex = 37;
            this.chkRG11.Text = "Use RG11 For Rain?";
            this.chkRG11.UseVisualStyleBackColor = true;
            // 
            // SetupDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1636, 776);
            this.Controls.Add(this.chkHDS10);
            this.Controls.Add(this.chkRG11);
            this.Controls.Add(this.chkBWF);
            this.Controls.Add(this.textBox13);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.textBox12);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.textBox11);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.textBox9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.textBox10);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.textBox7);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBoxComPort);
            this.Controls.Add(this.chkTrace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.picASCOM);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SetupDialogForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Arduino Setup";
            ((System.ComponentModel.ISupportInitialize)(this.picASCOM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.PictureBox picASCOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkTrace;
        private System.Windows.Forms.ComboBox comboBoxComPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox textBox9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox textBox10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox textBox12;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox textBox13;
        private System.Windows.Forms.CheckBox chkBWF;
        private System.Windows.Forms.CheckBox chkHDS10;
        private System.Windows.Forms.CheckBox chkRG11;
    }
}