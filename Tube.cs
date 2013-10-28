using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public class Tube
    {
        public Rectangle Rect { get; set; }
        static Random rnd = new Random();
        
        public int Index { get; set; }
        public int Length { get; set; }
        public double NormalizationScalar { get; set; }

        public double Threshold { get; set; }   

        public Tube(int maxEnd)
        {
            this.Rect = new Rectangle();

            int small, big;

            int x1 = 0;
            int x2 = 0;

            while (x1 == x2)
            {
                x1 = rnd.Next(maxEnd);
                x2 = rnd.Next(maxEnd);
            }

            small = Math.Min(x1, x2);
            big = Math.Max(x1, x2);

            this.Index = small;
            this.Length = big - small;

            this.NormalizationScalar = Math.Sqrt(Math.Pow(this.Rect.Width, 2) + Math.Pow(this.Rect.Height, 2) + Math.Pow(this.Length, 2));
        }
    }
}
