using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public static class Extensions
    {
        public static byte GetPixel(this byte[] array, int x, int y)
        {
            return array[y * Parameters.Width + x];
        }

        public static void SetPixel(this byte[] array, int x, int y, byte value)
        {
            array[y * Parameters.Width + x] = value;
        }

        public static double GetPixel(this double[] array, int x, int y)
        {
            return array[y * Parameters.Width + x];
        }

        public static void SetPixel(this double[] array, int x, int y, double value)
        {
            array[y * Parameters.Width + x] = value;
        }

        public static double  GetArea(this double[] array, Rectangle rect)
        {
            double A = (rect.A.X - 1 < 0 || rect.A.Y - 1 < 0) ? 0.0 : array.GetPixel(rect.A.X - 1, rect.A.Y - 1);
            double B = (rect.B.Y - 1 < 0) ? 0.0 : array.GetPixel(rect.B.X, rect.B.Y - 1);
            double C = (rect.C.X - 1 < 0) ? 0.0 : array.GetPixel(rect.C.X - 1, rect.C.Y);
            double D = array.GetPixel(rect.D.X, rect.D.Y);

            return D + A - B - C;
        }

        public static int MaxIndex(this double[] sequence)
        {
            int maxIndex = 0;
            double maxValue = sequence[0];

            for (int i = 1; i < sequence.Length; i++)
            {
                if (sequence[i] > maxValue)
                {
                    maxValue = sequence[i];
                    maxIndex = i;
                }
            }

            return maxIndex;
        }

        public static void CreateIntegralVideos(this List<Video> videos)
        {
            for (int i = 0; i < videos.Count; i++)
            {
                videos[i].CreateIntegralVideo();
            }
        }

    }
}
