using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;

namespace AnomalyDetection
{
    public static class Parameters
    {
        public static DateTime StartingTime = DateTime.Now;

        public static int Width = 238;
        public static int Height = 158;
        
        public static int NumVideosEachTree = 30;
        public static int NumTree = 8;
        public static int UsedFeatureCountAtEveryNode = 100;
        public static int MaxDepth = 20;
        public static double DiffThreshold = 0.01;

        public static string GlobalLogPath = GetLogPath();

        //public static string TrainPath = @"C:\Users\Celebi\Desktop\UCSDped1\Train";
        public static string TrainPath = @"C:\Users\Celebi\Desktop\tra";
        public static string TestPath = @"C:\Users\Celebi\Desktop\UCSDped1\Test";

        public static string Log()
        {
            string rVal = "";

            foreach (FieldInfo field in typeof(Parameters).GetFields())
            {
                if (field.FieldType == typeof(int[]))
                {
                    string temp = "{ ";

                    int[] tempArr = (int[])field.GetValue(null);

                    for (int i = 0; i < tempArr.Length; i++)
                    {
                        temp += tempArr[i].ToString() + ", ";
                    }

                    temp += "}";

                    rVal += String.Format("{0} = {1}" + Environment.NewLine, field.Name, temp);
                }
                else
                {
                    rVal += String.Format("{0} = {1}" + Environment.NewLine, field.Name, field.GetValue(null));
                }
            }

            return rVal;
        }

        public static string GetLogPath()
        {
            int counter = 0;

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\";

            string rVal = "";
            do
            {
                rVal = desktopPath + "log" + counter.ToString("D3") + ".txt";
                counter++;
            }
            while (File.Exists(rVal));

            return rVal;
        }
    }
}
