using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoidScreensaver {
    class Boid {
        public double X;
        public double Y;
        public double Xvel;
        public double Yvel;

        public Boid(double x, double y, double Xvel, double Yvel) {
            this.X = x;
            this.Y = y;
            this.Xvel = Xvel;
            this.Yvel = Yvel;
        }

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

        public double GetDistance(Boid other) {
            return Math.Sqrt(((other.X + X) * (other.X + X)) + ((other.Y + Y) * (other.Y + Y)));
        }

        public double GetAngle() {
            return Math.Atan2(Yvel, Xvel);
        }
    }
}
