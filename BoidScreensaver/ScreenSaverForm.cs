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
            Cursor.Hide();
            //TopMost = true;
        }

        public ScreenSaverForm(Rectangle Bounds) { // given screen size
            InitializeComponent();
            this.Bounds = Bounds;
            graphics = CreateGraphics();
            Cursor.Hide();
            //TopMost = true;
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

            moveTimer.Interval = 30;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

            Stage.Width = Bounds.Width;
            Stage.Height = Bounds.Height;
            Stage.Location = new Point(0, 0);

            boids = new List<Boid>();
            for(int i = 0; i < 100; i++) {
                boids.Add(new Boid(rand.Next(Math.Max(1, Bounds.Width)), rand.Next(Math.Max(1, Bounds.Height)), (double)rand.Next(0, 100) / 100f * 20 - 5, (double)rand.Next(0, 100) / 100f * 20 - 5));               
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
                // Calculate the rules
                Flock(boid, 75, 0.005f);
                Align(boid, 75, 0.07f);
                Avoid(boid, 35, 0.002f);
                AvoidWalls(boid, 1.5f);

                // Move the boid
                boid.Move(10, 15);
            }

        }

        private void Flock (Boid boid, double distance, double power) {
            // Get all neighbors that are within distance
            var neighbors = boids.Where(x => (Math.Pow((x.X - boid.X), 2) + Math.Pow((x.Y - boid.Y), 2)) < distance * distance);
            // Average their locations
            double averageX = neighbors.Sum(x => x.X) / neighbors.Count();
            double averageY = neighbors.Sum(x => x.Y) / neighbors.Count();
            // Calculate the offsets
            double centerOffsetX = averageX - boid.X;
            double centerOffsetY = averageY - boid.Y;

            boid.Xvel += centerOffsetX * power; boid.Yvel += centerOffsetY * power;
        }

        private void Align(Boid boid, double distance, double power) {
            // Get all neighbors that are within distance
            var neighbors = boids.Where(x => (Math.Pow((x.X - boid.X), 2) + Math.Pow((x.Y - boid.Y), 2)) < distance * distance);
            // Average their velocities
            double averageXvel = neighbors.Sum(x => x.Xvel) / neighbors.Count();
            double averageYvel = neighbors.Sum(x => x.Yvel) / neighbors.Count();
            // Calculate the difference
            double deltaXvel = averageXvel - boid.Xvel;
            double deltaYvel = averageYvel - boid.Yvel;
            
            boid.Xvel += deltaXvel * power; boid.Yvel += deltaYvel * power;
        }

        private void Avoid(Boid boid, double distance, double power) {
            // Get all neighbors that are within distance
            var neighbors = boids.Where(x => (Math.Pow((x.X - boid.X), 2) + Math.Pow((x.Y - boid.Y), 2)) < distance * distance);
            double sumClosenessX = 0f;
            double sumClosenessY = 0f;
            foreach(Boid neighbor in neighbors) {
                double closeness = distance - Math.Sqrt((Math.Pow((neighbor.X - boid.X), 2) + Math.Pow((neighbor.Y - boid.Y), 2)));
                sumClosenessX += (boid.X - neighbor.X) * closeness;
                sumClosenessY += (boid.Y - neighbor.Y) * closeness;
            }

            boid.Xvel += sumClosenessX * power; boid.Yvel += sumClosenessY * power;
        }

        private void AvoidWalls(Boid boid, double power) {
            // pad is 5% of the smalles dimention
            double pad = 0.05 * Math.Min(Bounds.Width, Bounds.Height);

            if(boid.X < pad) { // Off the left side of the screen
                boid.Xvel += power;
            }
            if(boid.Y < pad) { // Off the top of the screen
                boid.Yvel += power;
            }
            if(boid.X > Bounds.Width - pad) { // Off the right side of the screen
                boid.Xvel -= power;
            }
            if(boid.Y > Bounds.Height - pad) { // Off the bottom of the screen
                boid.Yvel -= power;
            }
        }

        private void DrawBoid(Boid boid) {
            if(boid != null) {
                double xoff = Math.Cos(boid.GetAngle()) * 20;
                double yoff = Math.Sin(boid.GetAngle()) * 20;

                drawLine(new Point((int)boid.X, (int)boid.Y), new Point((int)(boid.X + xoff), (int)(boid.Y + yoff)), Color.White);
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
                if(!(x0 < 0 || y0 < 0 || x0 >= Bounds.Width || y0 >= Bounds.Height)) {
                    bitmap.SetPixel(x0, y0, color);
                }

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
