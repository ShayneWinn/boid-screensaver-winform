using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoidScreensaver {
    class Boid {
        public double x { get; private set; }
        public double y { get; private set; }
        public double angle { get; private set; }

        public Boid() {
            x = 0f;
            y = 0f;
            angle = 0f;
        }

        public Boid(double x, double y, double angle) {
            this.x = x;
            this.y = y;
            this.angle = angle;
        }

        public void move(double x, double y) {
            this.x += x;
            this.y += y;
        }

        public void rotate(double angle) {
            this.angle += angle;
            this.angle %= Math.PI * 2;
        }
    }
}
