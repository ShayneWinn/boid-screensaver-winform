using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoidScreensaver {
    public partial class SettingsForm : Form {
        public SettingsForm() {
            InitializeComponent();
            LoadSettings();
        }

        private void SaveSettings() {
            // Create or get existing Registry subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Boid_ScreenSaver");

            if (key == null) {
                MessageBox.Show("Unable to save settings.",
                    "Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                key.SetValue("text", textBox.Text);
            }
        }

        private void LoadSettings() {
            // Get the value stored in the regestry key
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Boid_ScreenSaver");
            if (key == null) {
                textBox.Text = "C# Screen Saver";
            }
            else {
                textBox.Text = (string)key.GetValue("text");
            }

        }

        private void okButton_Click(object sender, EventArgs e) {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void SettingsForm_Load(object sender, EventArgs e) {

        }
    }
}
