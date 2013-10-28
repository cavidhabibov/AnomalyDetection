using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public class Rectangle
    {
        public static Random rnd = new Random();

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Area { get; private set; }

        public Point A { get; private set; }
        public Point B { get; private set; }
        public Point C { get; private set; }
        public Point D { get; private set; }

        public Rectangle ()
	    {
            int imageWidth = Parameters.Width;
            int imageHeight = Parameters.Height;

            while ((this.Width == 0 || this.Height == 0))
            {
                int x1, y1, x2, y2;

                do
                {
                    x1 = rnd.Next(imageWidth);
                    y1 = rnd.Next(imageHeight);
                    x2 = rnd.Next(imageWidth);
                    y2 = rnd.Next(imageHeight);
                }
                while (x1 == x2 && y1 == y2);

                int xMin = Math.Min(x1, x2);
                int yMin = Math.Min(y1, y2);

                int xMax = Math.Max(x1, x2);
                int yMax = Math.Max(y1, y2);

                this.A = new Point(xMin, yMin);
                this.D = new Point(xMax, yMax);

                this.Width = this.D.X - this.A.X;
                this.Height = this.D.Y - this.A.Y;

                this.B = new Point(this.A.X + (int)this.Width, this.A.Y);
                this.C = new Point(this.A.X, this.A.Y + (int)this.Height);
            }

            this.Area = this.Width * this.Height;
	    }

        public Rectangle(int x, int y, int Width, int Height)
        {
            this.A = new Point(x, y);
            this.B = new Point(x + Width, y);
            this.C = new Point(x, y + Height);
            this.D = new Point(x + Width, y + Height);

            this.Width = Width;
            this.Height = Height;
            this.Area = this.Width * this.Height;
        }
    }
}
