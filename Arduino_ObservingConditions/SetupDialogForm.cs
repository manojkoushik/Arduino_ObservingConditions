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

            ObservingConditions.cloudSlope = (float.Parse(textBox1.Text) - float.Parse(textBox2.Text)) / 100;
            ObservingConditions.cloudyCond = int.Parse(textBox6.Text);
            ObservingConditions.veryCloudyCond = int.Parse(textBox5.Text);

            ObservingConditions.lightSlope = (float.Parse(textBox9.Text) - float.Parse(textBox10.Text)) / (float.Parse(textBox3.Text) - float.Parse(textBox4.Text));

            ObservingConditions.windyCond = float.Parse(textBox8.Text);
            ObservingConditions.veryWindyCond = float.Parse(textBox7.Text);

            ObservingConditions.lightCond = float.Parse(textBox11.Text) * ObservingConditions.lightSlope;
            ObservingConditions.veryLightCond = float.Parse(textBox12.Text) * ObservingConditions.lightSlope;

            ObservingConditions.bwf = textBox13.Text;
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
        }

        private void textBox13_Clicked(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                textBox13.Text = folderBrowserDialog1.SelectedPath + "\\ArdinoBWF.txt";
        }
    }
}