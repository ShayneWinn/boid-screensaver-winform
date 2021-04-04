using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoidScreensaver {
    class Boid {
        private Point position;
        private float rotation;

        public Boid() {
            position = new Point(0, 0);
            rotation = 0f;
        }

        public Boid(Point position) {
            this.position = new Point(position.X, position.Y);
            rotation = 0f;
        }

        public Boid(Point position, float rotation) {
            this.position = new Point(position.X, position.Y);
            this.rotation = rotation;
        }

        public float getRotation() {
            return rotation;
        }

        public Point getPosition() {
            return position;
        }

        public void move(int x, int y) {
            position.X = position.X + x;
            position.Y = position.Y + y;
        }
    }
}
