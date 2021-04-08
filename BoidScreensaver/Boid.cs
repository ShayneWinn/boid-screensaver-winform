using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoidScreensaver {
    /// <summary>
    /// A Boid. Stores location and velicities for a given Boid
    /// </summary>
    class Boid {
        public double X;
        public double Y;
        public double Xvel;
        public double Yvel;

        /// <summary>
        /// Creates a boid given a starting set of parameters
        /// </summary>
        public Boid(double x, double y, double Xvel, double Yvel) {
            this.X = x;
            this.Y = y;
            this.Xvel = Xvel;
            this.Yvel = Yvel;
        }

        /// <summary>
        /// Apply the velocities within a maximum speed
        /// </summary>
        /// <param name="minSpeed"> the slowest a Boid can move </param>
        /// <param name="maxSpeed"> the fastest a Boid can move </param>
        public void Move(double minSpeed, double maxSpeed) {
            double speed = Math.Sqrt((Xvel * Xvel) + (Yvel * Yvel));

            if(speed > maxSpeed) {
                Xvel = (Xvel / speed) * maxSpeed;
                Yvel = (Yvel / speed) * maxSpeed;
            }
            else if (speed < minSpeed) {
                Xvel = (Xvel / speed) * minSpeed;
                Yvel = (Yvel / speed) * minSpeed;
            }

            if (double.IsNaN(Xvel)) {
                Xvel = 0;
            }
            if (double.IsNaN(Yvel)) {
                Yvel = 0;
            }

            X += Xvel;
            Y += Yvel;
        }

        /// <summary>
        /// Gets the distance between the curent boid and the given boid
        /// </summary>
        /// <param name="other"> The boid to find the distance to </param>
        /// <returns> Distance to a given boid </returns>
        public double GetDistance(Boid other) {
            return Math.Sqrt(((other.X - X) * (other.X - X)) + ((other.Y - Y) * (other.Y - Y)));
        }

        /// <summary>
        /// Calculates the angle a boid is moving in
        /// </summary>
        /// <returns> Returns the angle in radians </returns>
        public double GetAngle() {
            return Math.Atan2(Yvel, Xvel);
        }

        /// <summary>
        /// Translates the X and Y position into a Point
        /// </summary>
        /// <returns> Point form of Boid position </returns>
        public Point GetPoint() {
            return new Point((int)X, (int)Y);
        }
    }
}
