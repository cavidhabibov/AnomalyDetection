using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace AnomalyDetection
{
    public static class FileOperations
    {
        public static Random rnd = new Random();

        public static void WriteImage(byte[] pixels, string fileName)
        {
            int width = 238;
            int height = 158;
            int stride = width;

            // Define the image palette
            BitmapPalette myPalette = BitmapPalettes.Halftone256;

            // Creates a new empty image with the pre-defined palette

            BitmapSource image = BitmapSource.Create(
                width,
                height,
                96,
                96,
                PixelFormats.Gray8,
                myPalette,
                pixels,
                stride);

            FileStream stream = new FileStream(fileName, FileMode.Create);
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Interlace = PngInterlaceOption.On;
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(stream);
        }

        public static void WriteVideos(List<Video> videos, string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            foreach (var video in videos)
            {
                string subDir = dir + @"\" + video.DirectoryName;
                if (!Directory.Exists(subDir))
                    Directory.CreateDirectory(subDir);

                int counter = 0;
                for (int i = 0; i < video.Images.Count; i++)
                {
                    counter++;
                    string fileName = subDir + @"\" + counter.ToString("D3") + ".png";
                    if(!File.Exists(fileName))
                        WriteImage(video.Images[i], fileName);                    
                }
            }
        }

        public static byte[] GetByteArray(string fileName)
        {
            byte[] rVal;

            Stream imageStreamSource = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            TiffBitmapDecoder decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource = decoder.Frames[0];

            int height = bitmapSource.PixelHeight;
            int width = bitmapSource.PixelWidth;

            int bytesPerPixel = bitmapSource.Format.BitsPerPixel / 8;

            int stride = width * bytesPerPixel;

            rVal = new byte[height * stride];
            bitmapSource.CopyPixels(rVal, stride, 0);

            return rVal;
        }

        public static List<Video> ReadDatabase(string dirName)
        {
            List<Video> rVal = new List<Video>();

            string[] directories = Directory.GetDirectories(dirName);

            for (int i = 0; i < directories.Length; i++)
            {
                rVal.Add(new Video(directories[i]));
            }

            return rVal;
        }

        public static List<Video> ReadDatabaseRandomly(string dirName, int count)
        {
            List<Video> rVal = new List<Video>();

            List<string> directories = Directory.GetDirectories(dirName).ToList();

            for (int i = 0; i < count; i++)
            {
                int rndIndex = rnd.Next(directories.Count);
                string randomlyDirectory = directories.ElementAt(rndIndex);
                Video vid = new Video(directories[rndIndex]);
                Video subVid = vid.GetSubVideo(100);
                rVal.Add(subVid);
                //directories.RemoveAt(rndIndex);
            }

            return rVal;
        }
    }
}
