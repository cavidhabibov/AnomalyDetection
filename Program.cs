using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace AnomalyDetection
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Video> videos = FileOperations.ReadDatabaseRandomly(Parameters.TrainPath, Parameters.NumVideosEachTree);
            //FileOperations.WriteVideos(videos, @"C:\Users\Celebi\Desktop\A");
            List<Video> motionVideos = ImageOperations.CreateMotions(videos);
            //FileOperations.WriteVideos(motionVideos, @"C:\Users\Celebi\Desktop\B");

            motionVideos.CreateIntegralVideos();

            Tube tb = new Tube(99);
            double result1 = motionVideos[0].GetVolume(new Tube(99));

            Console.WriteLine(result1);

            //for (int i = tb.Index; i < tb.Index + tb.Length; i++) { 
            //    for(int j = tb.Rect.
            //}

            //using (StreamWriter writer = File.CreateText(Parameters.GlobalLogPath))
            //{
            //    writer.WriteLine(Parameters.Log());
            //}

            //Forest fr = new Forest();

            //using (StreamWriter writer = File.AppendText(Parameters.GlobalLogPath))
            //{
            //    writer.WriteLine();
            //    writer.WriteLine("Finished at: " + DateTime.Now);
            //}
        }         
    }
}
