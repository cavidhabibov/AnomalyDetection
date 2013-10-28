using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnomalyDetection
{
    public class Forest
    {
        public Random rnd = new Random();
        public List<Video> Inputs { get; private set; }

        public int NumTree { get; private set; }

        DecisionTree[] Trees;

        public int TestError { get; private set; }
        public double TestAccuracy { get; private set; }

        public Forest()
        {
            this.NumTree = Parameters.NumTree;
            this.Trees = new DecisionTree[this.NumTree];

            for (int i = 0; i < this.NumTree; i++)
            {
                List<Video> videos = FileOperations.ReadDatabaseRandomly(Parameters.TrainPath, Parameters.NumVideosEachTree);
                             
                List<Video> motionVideos = ImageOperations.CreateMotions(videos);
                
                motionVideos.CreateIntegralVideos();

                Console.WriteLine("Training of Tree " + (i + 1) + "/" + this.NumTree + " is started.");

                this.Trees[i] = new DecisionTree(this, motionVideos, i);

                using (StreamWriter writer = File.AppendText(Parameters.GlobalLogPath))
                {
                    writer.WriteLine(this.Trees[i].LogString);
                }

                Console.WriteLine("Tree " + (i + 1) + "/" + this.NumTree + " is trained.");                
            }

            this.Log();

            Console.WriteLine("Log file is created to: " + Parameters.GlobalLogPath);
        }      

        public void Log()
        {
            using (StreamWriter writer = File.AppendText(Parameters.GlobalLogPath))
            {
                writer.WriteLine();
            }
        }
    }
}
