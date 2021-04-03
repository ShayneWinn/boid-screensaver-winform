using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BoidScreensaver {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (args.Length > 0) {
                // Create the variables
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;
                
                // Handle the case where arguments are separated by colons
                if(firstArgument.Length > 2) {           
                    secondArgument = firstArgument.Substring(0, 2);
                    firstArgument = firstArgument.Substring(3).Trim();
                }
                else if(args.Length > 1) {
                    secondArgument = args[1];
                }
               
                // Check the arguments
                if(firstArgument == "/c") {         // Configuration
                    Application.Run(new SettingsForm());
                }

                else if(firstArgument == "/p") {    // Preview Mode
                    if (secondArgument == null) {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.",
                            "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new ScreenSaverForm(previewWndHandle));
                }
                else if(firstArgument == "/s") {    // Full-Screen Mode
                    ShowScreenSaver();
                    Application.Run();
                }
                else {                              // Undefined Argument
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "ScreenSaver",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else {      // No Arguments - treat like /c
                Application.Run(new SettingsForm());
            }
        }

        static void ShowScreenSaver() {
            foreach (Screen screen in Screen.AllScreens) {
                ScreenSaverForm screensaver = new ScreenSaverForm(screen.Bounds);
                screensaver.Show();
            }
        }
    }
}
