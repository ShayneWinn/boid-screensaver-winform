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
        public static string AVOIDWALLS_BOOL = "avoidWalls";
        public static string FLOCK_BOOL = "flock";
        public static string ALIGN_BOOL = "align";
        public static string AVOID_BOOL = "avoid";

        private ScreenSaverForm.Settings settings;
        private ScreenSaverForm childForm;

        public SettingsForm() {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e) {
            IsMdiContainer = true;
            childForm = new ScreenSaverForm(BoidsBox.Handle);
            childForm.Show();

            settings = LoadSettings();
            UpdateSettings();
            childForm.ChangeSettings(settings);
        }

        private void SaveSettings() {
            // Create or get existing Registry subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Boid_ScreenSaver");

            if (key == null) {
                MessageBox.Show("Unable to save settings.",
                    "Settings", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else {
                // Save fields
                key.SetValue(AVOIDWALLS_BOOL, infiniteWorld_checkBox.Checked);
                key.SetValue(FLOCK_BOOL, Flock_checkBox.Checked);
                key.SetValue(ALIGN_BOOL, align_checkBox.Checked);
                key.SetValue(AVOID_BOOL, avoid_checkBox.Checked);
            }
        }

        public static ScreenSaverForm.Settings LoadSettings() {
            ScreenSaverForm.Settings settings = ScreenSaverForm.DEFAULT_SETTINGS();
            // Open registry key
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Boid_ScreenSaver");
            if (key != null) { // if the key exists
                // Load actual
                try {
                    settings.AvoidWalls = Convert.ToBoolean(key.GetValue(AVOIDWALLS_BOOL));
                    settings.Flock = Convert.ToBoolean(key.GetValue(FLOCK_BOOL));
                    settings.Align = Convert.ToBoolean(key.GetValue(ALIGN_BOOL));
                    settings.Avoid = Convert.ToBoolean(key.GetValue(AVOID_BOOL));
                }
                catch {}
            }
            return settings;
        }

        private void UpdateSettings() {
            infiniteWorld_checkBox.Checked = settings.AvoidWalls;
            Flock_checkBox.Checked = settings.Flock;
            align_checkBox.Checked = settings.Align;
            avoid_checkBox.Checked = settings.Avoid;

            flockStrength_bar.Value = (int)settings.FlockStrength;
            alignStrength_bar.Value = (int)settings.AlignStrength;
            avoidStrength_bar.Value = (int)settings.AvoidStrength;
        }

        private void okButton_Click(object sender, EventArgs e) {
            SaveSettings();
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void infiniteWorld_checkBox_CheckedChanged(object sender, EventArgs e) {
            settings.AvoidWalls = infiniteWorld_checkBox.Checked;
            childForm.ChangeSettings(settings);
        }

        private void Flock_checkBox_CheckedChanged(object sender, EventArgs e) {
            settings.Flock = Flock_checkBox.Checked;
            childForm.ChangeSettings(settings);
        }

        private void align_checkBox_CheckedChanged(object sender, EventArgs e) {
            settings.Align = align_checkBox.Checked;
            childForm.ChangeSettings(settings);
        }

        private void avoid_checkBox_CheckedChanged(object sender, EventArgs e) {
            settings.Flock = avoid_checkBox.Checked;
            childForm.ChangeSettings(settings);
        }

        private void flockStrength_bar_Scroll(object sender, EventArgs e) {
            settings.FlockStrength = flockStrength_bar.Value;
            childForm.ChangeSettings(settings);
        }

        private void alignStrength_bar_Scroll(object sender, EventArgs e) {
            settings.AlignStrength = alignStrength_bar.Value;
            childForm.ChangeSettings(settings);
        }

        private void avoidStrength_bar_Scroll(object sender, EventArgs e) {
            settings.AvoidStrength = avoidStrength_bar.Value;
            childForm.ChangeSettings(settings);
        }

        private void defaultsButton_Click(object sender, EventArgs e) {
            settings = ScreenSaverForm.DEFAULT_SETTINGS();
            UpdateSettings();
        }
    }
}
