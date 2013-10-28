using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnomalyDetection
{
    public class Video
    {
        public static Random rnd = new Random();

        public string DirectoryName { get; set; }

        public List<byte[]> Images { get; set; }
        public List<double[]> IntegralImages { get; set; }
       
        public int Length { 
            get{
                return this.Images.Count;
            }
        }

        public Video()
        {
            this.Images = new List<byte[]>();
        }

        public Video(string directoryName)
        {
            DirectoryInfo di = new DirectoryInfo(directoryName);

            this.DirectoryName = di.Name;

            this.Images = new List<byte[]>();
            string[] files = Directory.GetFiles(directoryName);

            for (int i = 0; i < files.Length; i++)
                this.Images.Add(FileOperations.GetByteArray(files[i]));
        }

        public Video GetSubVideo(int length)
        {
            Video rVal = new Video();

            int rndStart = rnd.Next(this.Length - length);

            for (int i = 0; i < length; i++)
            {
                byte[] temp = new byte[Parameters.Width * Parameters.Height];
                Array.Copy(this.Images[i+rndStart], temp, temp.Length);
                rVal.Images.Add(temp);
            }

            rVal.DirectoryName = this.DirectoryName + "_sub_" + rndStart + "_length_" + length;

            return rVal;
        }

        public double GetVolume(Tube tb)
        {
            double startArea = this.IntegralImages[tb.Index].GetArea(tb.Rect);
            double endArea = this.IntegralImages[tb.Index + tb.Length].GetArea(tb.Rect);

            return endArea - startArea;            
        }

        public bool ApplyTube(Tube tb)
        {
            if (this.GetVolume(tb) > tb.Threshold)
                return true;
            else
                return false;
        }

        public void CreateIntegralVideo()
        {
            this.IntegralImages = new List<double[]>();

            double[] temp = SumFrames(new double[Parameters.Width * Parameters.Height], this.Images[0]);
            this.IntegralImages.Add(temp);

            for (int i = 1; i < this.Images.Count; i++)
            {
                temp = SumFrames(this.IntegralImages[i - 1], this.Images[i]);
                this.IntegralImages.Add(temp);
            }

            //NormalizeIntegralVideo();
        }

        private double[] SumFrames(double[] prev, byte[] current)
        {
            double[] rVal = new double[Parameters.Width * Parameters.Height];

            for (int i = 0; i < Parameters.Height; i++)
                rVal.SetPixel(0, i, current.GetPixel(0, i));

            for (int i = 0; i < Parameters.Height; i++)
            {
                for (int j = 1; j < Parameters.Width; j++)
                {
                    double temp = rVal.GetPixel(j - 1, i) + current.GetPixel(j, i);
                    rVal.SetPixel(j, i, temp);
                }
            }

            for (int j = 0; j < Parameters.Width; j++)
            {
                for (int i = 1; i < Parameters.Height; i++)
                {
                    double temp = rVal.GetPixel(j, i - 1) + rVal.GetPixel(j, i);
                    rVal.SetPixel(j, i, temp);
                }
            }

            for (int i = 0; i < rVal.Length; i++)
            {
                rVal[i] = rVal[i] + prev[i];
            }

            return rVal;
        }

        private void NormalizeIntegralVideo()
        {
            double scalar = ((double)1 / this.IntegralImages[this.IntegralImages.Count - 1][Parameters.Width * Parameters.Height - 1]);

            for (int i = 0; i < this.IntegralImages.Count; i++)
            {
                for (int j = 0; j < this.IntegralImages[i].Length; j++)
                {
                    this.IntegralImages[i][j] = this.IntegralImages[i][j] * scalar;
                }
            }
        }
    }
}
