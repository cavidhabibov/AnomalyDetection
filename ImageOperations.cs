using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public static class ImageOperations
    {
        public static byte[] GetDiff(byte[] seq1, byte[] seq2, byte threshold = 8)
        {
            if (seq1.Length != seq2.Length)
                throw new Exception("lengths must agree");

            byte[] rVal = new byte[seq1.Length];

            for (int i = 0; i < rVal.Length; i++)
            {
                rVal[i] = Math.Abs(seq1[i] - seq2[i]) > threshold ? (byte)255 : (byte)0;
            }

            return rVal;
        }

        public static List<Video> CreateMotions(List<Video> videos)
        {
            List<Video> rVal = new List<Video>();

            foreach (var video in videos)
            {
                Video motionVideo = new Video();
                motionVideo.DirectoryName = "Motion_" + video.DirectoryName ;

                for (int i = 0; i < video.Length - 1; i++)
                {
                    byte[] motionFrame = GetDiff(video.Images[i], video.Images[i + 1]);
                    motionVideo.Images.Add(motionFrame);
                }
                rVal.Add(motionVideo);
            }

            return rVal;
        }
       
    }
}
