using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ASCOM.Utilities;
using ASCOM.Arduino;

namespace ASCOM.Arduino
{
    [ComVisible(false)]					// Form not registered for COM!
    public partial class SetupDialogForm : Form
    {
        public SetupDialogForm()
        {
            InitializeComponent();
            // Initialise current values of user settings from the ASCOM Profile
            InitUI();
        }

        private void cmdOK_Click(object sender, EventArgs e) // OK button event handler
        {
            // Place any validation constraint checks here
            // Update the state variables with results from the dialogue
            ObservingConditions.comPort = (string)comboBoxComPort.SelectedItem;

            ObservingConditions.tl.Enabled = chkTrace.Checked;

            ObservingConditions.updateInterval = (int)numericUpDown1.Value;

            ObservingConditions.cloudySkies = float.Parse(textBox1.Text);
            ObservingConditions.clearSkies = float.Parse(textBox2.Text);
            ObservingConditions.cloudyCond = int.Parse(textBox6.Text);
            ObservingConditions.veryCloudyCond = int.Parse(textBox5.Text);

            ObservingConditions.nightVol = float.Parse(textBox4.Text);
            ObservingConditions.dayVol = float.Parse(textBox3.Text);

            ObservingConditions.nightLux = int.Parse(textBox10.Text);
            ObservingConditions.dayLux = int.Parse(textBox9.Text);

            ObservingConditions.twilightCond = int.Parse(textBox11.Text);
            ObservingConditions.daylightCond = int.Parse(textBox12.Text);

            ObservingConditions.windyCond = float.Parse(textBox8.Text);
            ObservingConditions.veryWindyCond = float.Parse(textBox7.Text);

            ObservingConditions.bwf = textBox13.Text;
            ObservingConditions.bwfEnabled = chkBWF.Checked;
        }

        private void cmdCancel_Click(object sender, EventArgs e) // Cancel button event handler
        {
            Close();
        }

        private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
        {
            try
            {
                System.Diagnostics.Process.Start("http://ascom-standards.org/");
            }
            catch (System.ComponentModel.Win32Exception noBrowser)
            {
                if (noBrowser.ErrorCode == -2147467259)
                    MessageBox.Show(noBrowser.Message);
            }
            catch (System.Exception other)
            {
                MessageBox.Show(other.Message);
            }
        }

        private void InitUI()
        {
            chkTrace.Checked = ObservingConditions.tl.Enabled;
            // set the list of com ports to those that are currently available
            comboBoxComPort.Items.Clear();
            comboBoxComPort.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());      // use System.IO because it's static
            // select the current port if possible
            if (comboBoxComPort.Items.Contains(ObservingConditions.comPort))
            {
                comboBoxComPort.SelectedItem = ObservingConditions.comPort;
            }

            textBox1.Text = ObservingConditions.cloudySkies.ToString();
            textBox2.Text = ObservingConditions.clearSkies.ToString();
            textBox6.Text = ObservingConditions.cloudyCond.ToString();
            textBox5.Text = ObservingConditions.veryCloudyCond.ToString();

            textBox4.Text = ObservingConditions.nightVol.ToString();
            textBox3.Text = ObservingConditions.dayVol.ToString();

            textBox10.Text = ObservingConditions.nightLux.ToString();
            textBox9.Text = ObservingConditions.dayLux.ToString();

            textBox11.Text = ObservingConditions.twilightCond.ToString();
            textBox12.Text = ObservingConditions.daylightCond.ToString();

            textBox8.Text = ObservingConditions.windyCond.ToString();
            textBox7.Text = ObservingConditions.veryWindyCond.ToString();

            textBox13.Text = ObservingConditions.bwf;

            chkBWF.Checked = ObservingConditions.bwfEnabled;


        }

        private void textBox13_Clicked(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                textBox13.Text = folderBrowserDialog1.SelectedPath + "\\ArdinoBWF.txt";
        }
    }
}