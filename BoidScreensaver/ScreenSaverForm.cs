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

        public struct Settings {
            public bool Flock; // Should the boids flock together?
            public bool Align; // Should the boids align?
            public bool Avoid; // Should the boids avoid collisions?

            public bool AvoidWalls; // Should the boids avoid walls?

            public double FlockStrength; // How strong is the Flocking?
            public double AlignStrength; // How strong is the Aligning?
            public double AvoidStrength; // How strong is the Avoiding?
        }

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
        private Settings settings;

        private double largeRadius;
        private double smallRadius;
        private double minSpeed;
        private double maxSpeed;
        private double boidSize;
        private double flockPower;
        private double alignPower;
        private double avoidPower;


        /// <summary>
        /// Static function used to get a new Settings full of default values
        /// </summary>
        /// <returns> ScreenSaverForm.Settings </returns>
        public static Settings DEFAULT_SETTINGS() {
            Settings settings;
            settings.Flock = true;
            settings.Align = true;
            settings.Avoid = true;
            settings.AvoidWalls = true;
            settings.FlockStrength = 50;
            settings.AlignStrength = 50;
            settings.AvoidStrength = 50;

            return settings;
        }


        //  /==========================\
        // || Constructors / Modifiers ||
        //  \==========================/


        /// <summary>
        /// Used to construct a simple ScreenSaverForm
        /// </summary>
        public ScreenSaverForm() { // basic
            InitializeComponent();
            Cursor.Hide();
            //TopMost = true;
        }

        /// <summary>
        /// Used to construct a ScreenSaver within a given Bounds
        /// </summary>
        /// <param name="Bounds"> Rectangle containing width and height maximums </param>
        public ScreenSaverForm(Rectangle Bounds) { // given screen size
            InitializeComponent();
            this.Bounds = Bounds;
            graphics = CreateGraphics();
            Cursor.Hide();
            //TopMost = true;
        }

        /// <summary>
        /// Used to construct a ScreenSaverForm that has a parent window. Used in settings and preview
        /// </summary>
        /// <param name="PreviewWndHandle"></param>
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

        /// <summary>
        /// Changes the current active settings
        /// </summary>
        /// <param name="settings"></param>
        public void ChangeSettings(Settings settings) {
            this.settings = settings;
            flockPower = 0.01 * (settings.FlockStrength / 100);
            alignPower = 0.14 * (settings.AlignStrength / 100);
            avoidPower = 0.004 * (settings.AvoidStrength / 100);
        }


        //  /==================\
        // || Startup Handeler ||
        //  \==================/


        /// <summary>
        /// Called just after the window is created, Calculates settings and creates boids
        /// </summary>
        private void ScreenSaverForm_Load(object sender, EventArgs e) {
            settings = SettingsForm.LoadSettings();

            // Calculate other settings
            boidSize = Math.Min(Bounds.Width, Bounds.Height) / 50;
            minSpeed = Math.Min(Bounds.Width, Bounds.Height) / 100;
            maxSpeed = Math.Min(Bounds.Width, Bounds.Height) / 75;
            largeRadius = Math.Min(Bounds.Width, Bounds.Height) / 15;
            smallRadius = largeRadius / 2;
            flockPower = 0.01 * (settings.FlockStrength / 100);
            alignPower = 0.14 * (settings.AlignStrength / 100);
            avoidPower = 0.001 * (settings.AvoidStrength / 100);

            // Create bitmap
            bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            // Set clock speed
            moveTimer.Interval = 30;
            moveTimer.Tick += new EventHandler(moveTimer_Tick);
            moveTimer.Start();

            // Set bounds
            Stage.Width = Bounds.Width;
            Stage.Height = Bounds.Height;
            Stage.Location = new Point(0, 0);

            // Create Boids
            boids = new List<Boid>();
            for(int i = 0; i < 100; i++) {
                boids.Add(new Boid(rand.Next(Math.Max(1, Bounds.Width)), rand.Next(Math.Max(1, Bounds.Height)), (double)rand.Next(-100, 100) / 100f * maxSpeed, (double)rand.Next(-100, 100) / 100f * maxSpeed));               
            }
        }


        //  /=================\
        // || Event Handelers ||
        //  \=================/


        /// <summary>
        /// Called when a mouse moves on the screen. Closes the Application
        /// </summary>
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

        /// <summary>
        /// Called when a mouse button is clicked. Closes the Application
        /// </summary>
        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e) {
            if (!previewMode)
                Application.Exit();
        }

        /// <summary>
        /// Called when a key is pressed. Closes the applicaation
        /// </summary>
        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e) {
            if (!previewMode)
                Application.Exit();
        }


        //  /===================\
        // || Main Clock Events ||
        //  \===================/


        /// <summary>
        /// Called every 30 miliseconds. Clears the old bitmap and updates all boids
        /// </summary>
        private void moveTimer_Tick(object sender, EventArgs e) {
            bitmap.Dispose();
            bitmap = new Bitmap(Bounds.Width, Bounds.Height);

            foreach(Boid boid in boids) {
                DrawBoid(boid);
                UpdateBoid(boid);
            }

            Stage.Image = bitmap;
        }

        /// <summary>
        /// Updates a given boid
        /// </summary>
        /// <param name="boid"> Boid to update and calculate movement for </param>
        private void UpdateBoid(Boid boid) {
            if (boid != null) {
                // OPTIMIZATION:
                // grab neighbors
                var neighbors = boids.Where(x => boid.GetDistance(x) < largeRadius && x != boid);
                if (neighbors.Count() > 0) {
                    // average EVERYTHING
                    double averageX = 0; double averageY = 0;           // Flock
                    double averageXvel = 0; double averageYvel = 0;     // Align
                    double sumClosenessX = 0; double sumClosenessY = 0; // Avoid

                    // go through every neighbor
                    foreach (Boid neighbor in neighbors) {
                        // Flock
                        averageX += neighbor.X;
                        averageY += neighbor.Y;

                        // Align
                        averageXvel += neighbor.Xvel;
                        averageYvel += neighbor.Yvel;

                        // Avoid
                        if (boid.GetDistance(neighbor) < smallRadius) { 
                            double closeness = largeRadius - boid.GetDistance(neighbor);
                            sumClosenessX += (boid.X - neighbor.X) * closeness;
                            sumClosenessY += (boid.Y - neighbor.Y) * closeness;
                        }
                    }

                    // Calculate the rules
                    if (settings.Flock) {
                        averageX = averageX / neighbors.Count();
                        averageY = averageY / neighbors.Count();
                        double centerOffsetX = averageX - boid.X;
                        double centerOffsetY = averageY - boid.Y;
                        boid.Xvel += centerOffsetX * flockPower; boid.Yvel += centerOffsetY * flockPower;
                    }

                    if (settings.Align) {
                        averageXvel = averageXvel / neighbors.Count();
                        averageYvel = averageYvel / neighbors.Count();
                        double deltaXvel = averageXvel - boid.Xvel;
                        double deltaYvel = averageYvel - boid.Yvel;
                        boid.Xvel += deltaXvel * alignPower; boid.Yvel += deltaYvel * alignPower;
                    }

                    if (settings.Avoid) {
                        boid.Xvel += sumClosenessX * avoidPower; boid.Yvel += sumClosenessY * avoidPower;
                    }
                }


                if (settings.AvoidWalls)
                    AvoidWalls(boid);
                else
                    WrapWorld(boid);


                // Move the boid
                boid.Move(minSpeed, maxSpeed);
            }

        }

        /// <summary>
        /// Applies the flocking mechanic to a given boid
        ///     finds all boids in a given distance and moves towards the center of them
        /// </summary>
        /// <param name="boid"> Boid to modify </param>
        /// <param name="distance"> The distance to search </param>
        /// <param name="power"> Used to lessen the effects and balance the behaviours </param>
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

        /// <summary>
        /// Applies the aligning mechanic to a given boid
        ///     finds all boids in a given distance and tries to move in the same distance as the average
        /// </summary>
        /// <param name="boid"> Boid to modify </param>
        /// <param name="distance"> The distance to search </param>
        /// <param name="power"> Used to lessen the effects and balance the behaviours </param>
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

        /// <summary>
        /// Applies the avoiding mechanic to a given boid
        ///     finds all boids in a given distance and moves away from them based on how close they are
        /// </summary>
        /// <param name="boid"> Boid to modify </param>
        /// <param name="distance"> The distance to search </param>
        /// <param name="power"> Used to lessen the effects and balance the behaviours </param>
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

        /// <summary>
        /// Changes the velocity of a boid to keep it within the bounds
        /// </summary>
        /// <param name="boid"> Boid to modify </param>
        private void AvoidWalls(Boid boid) {
            // pad is 5% of the smalles dimention
            double pad = 0.1 * Math.Min(Bounds.Width, Bounds.Height);
            double power = pad / 100;

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

        /// <summary>
        /// Teleports a boid located outside of the bounds to the opposite side of the world
        /// </summary>
        /// <param name="boid"> Boid to modify </param>
        private void WrapWorld(Boid boid) {
            if(boid.X < 0) {
                boid.X += Bounds.Width;
            }
            else if(boid.X > Bounds.Width) {
                boid.X -= Bounds.Width;
            }

            if(boid.Y < 0) {
                boid.Y += Bounds.Height;
            }
            else if(boid.Y > Bounds.Height) {
                boid.Y -= Bounds.Height;
            }
        }

        /// <summary>
        /// Draws a given boid based on it's velocities
        ///     Currently draws a white line a given length
        /// </summary>
        /// <param name="boid"> Boid to draw </param>
        private void DrawBoid(Boid boid) {
            if(boid != null) {
                double xoff = Math.Cos(boid.GetAngle()) * boidSize;
                double yoff = Math.Sin(boid.GetAngle()) * boidSize;

                drawLine(new Point((int)boid.X, (int)boid.Y), new Point((int)(boid.X + xoff), (int)(boid.Y + yoff)), Color.White);
            }
        }


        //  /====================\
        // || Graphics Functions ||
        //  \====================/


        /// <summary>
        /// Draws a line pixel by pixel using Bresenham's line algorithm. 
        /// </summary>
        /// <param name="a"> starting Point </param>
        /// <param name="b"> ending Point </param>
        /// <param name="color"> Color to draw the line </param>
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
    }
}
