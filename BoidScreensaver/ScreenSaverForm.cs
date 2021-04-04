using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace BoidScreensaver {
    public partial class ScreenSaverForm : Form {
        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);


        // Attributes
        private Point mouseLocation;
        private Random rand = new Random();
        bool previewMode = false;
        private Graphics graphics;
        private List<Boid> boids;
        private Bitmap bitmap;



        // Constructors

        public ScreenSaverForm() { // basic
            InitializeComponent();
        }

        public ScreenSaverForm(Rectangle Bounds) { // given screen size
            InitializeComponent();
            this.Bounds = Bounds;
            graphics = CreateGraphics();
        }

        public ScreenSaverForm(IntPtr PreviewWndHandle) { // given parent window (preview)
            InitializeComponent();

            // Set the preview window as the parent of this window
            SetParent(this.Handle, PreviewWndHandle);

            // Make this a child window so it will close when the parent dialog closes
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent
            Rectangle ParentRect;
            GetClientRect(PreviewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            previewMode = true;
        }



        // Start up Handler

        private void ScreenSaverForm_Load(object sender, EventArgs e) {
            // Grab the settings from the registry
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Boid_ScreenSaver");
            if(key == null) {
                //textLabel.Text = "C# Screen Saver";
            }
            else {
                //textLabel.Text = (string)key.GetValue("text");
            }

            // create bitmap
            bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            Cursor.Hide();
            //TopMost = true;

            moveTimer.Interval = 30;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

            Stage.Width = Bounds.Width;
            Stage.Height = Bounds.Height;
            Stage.Location = new Point(0, 0);

            boids = new List<Boid>();
            for(int i = 0; i < 100; i++) {
                boids.Add(new Boid(rand.Next(Math.Max(1, Bounds.Width)), rand.Next(Math.Max(1, Bounds.Height)), (float)rand.Next(0, 100) / 100f * (float)Math.PI));               
            }
        }



        // Event Handlers

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e) {
            if (!mouseLocation.IsEmpty) {
                // Terminate if mouse is moved a significant distance
                if (Math.Abs(mouseLocation.X - e.X) > 5 ||
                    Math.Abs(mouseLocation.Y - e.Y) > 5)
                    if (!previewMode)
                        Application.Exit();
            }

            // Update current mouse location
            mouseLocation = e.Location;
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e) {
            if (!previewMode)
                Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e) {
            if (!previewMode)
                Application.Exit();
        }


        
        // Main clock

        private void moveTimer_Tick(object sender, EventArgs e) {
            bitmap.Dispose();
            bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            foreach(Boid boid in boids) {
                DrawBoid(boid);
                UpdateBoid(boid);
            }

            Stage.Image = bitmap;
        }

        private void UpdateBoid(Boid boid) {
            if (boid != null) {
                // move forward
                int xoff = (int)(Math.Cos(boid.angle) * 15);
                int yoff = (int)(Math.Sin(boid.angle) * 15);

                boid.move(xoff, yoff);


                // wrap around screen
                if(boid.x > Bounds.Width) {
                    boid.move(-Bounds.Width, 0);
                }
                if(boid.x < 0) {
                    boid.move(Bounds.Width, 0);
                }
                if (boid.y > Bounds.Height) {
                    boid.move(0, -Bounds.Height);
                }
                if (boid.y < 0) {
                    boid.move(0, Bounds.Height);
                }
            }

        }

        private void DrawBoid(Boid boid) {
            if(boid != null) {
                double xoff = Math.Cos(boid.angle) * 20;
                double yoff = Math.Sin(boid.angle) * 20;

                drawLine(new Point((int)boid.x, (int)boid.y), new Point((int)(boid.x + xoff), (int)(boid.y + yoff)), Color.White);
            }
        }



        // Drawing functions

        private void drawLine(Point a, Point b, Color color) {
            int x0 = a.X;
            int y0 = a.Y;
            int x1 = b.X;
            int y1 = b.Y;

            int deltaX = Math.Abs(x1 - x0);
            int sx = x0 < x1 ? 1 : -1;
            int deltaY = -Math.Abs(y1 - y0);
            int sy = y0 < y1 ? 1 : -1;
            float error = deltaX + deltaY;

            while (true) {
                if(x0 < 0 || y0 < 0) {
                    break;
                }

                if(!(x0 > Bounds.Width || y0 > Bounds.Height))
                    bitmap.SetPixel(x0 % Bounds.Width, y0 % Bounds.Height, color);

                if(x0 == x1 && y0 == y1)
                    break;
                float e2 = 2 * error;
                if (e2 >= deltaY) {
                    error += deltaY;
                    x0 += sx;
                }
                if (e2 <= deltaX) {
                    error += deltaX;
                    y0 += sy;
                }
            }

            Stage.Image = bitmap;
        }


        private void drawTriangle(Point a, Point b, Point c, Color color) {
            // TODO
        }
    }
}
