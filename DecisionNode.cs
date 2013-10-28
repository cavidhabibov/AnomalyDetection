using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public class DecisionNode
    {
        public DecisionTree Owner { get; private set; }
        public DecisionNode Parent { get; private set; }
        public List<Video> Inputs { get; private set; }
         
        public int Level { get; private set; }
        public Tube SplitFeature { get; set; }
        public DecisionNode[] Branches { get; private set; }

        public bool IsRoot
        {
            get { return Parent == null; }
        }

        public bool IsLeaf
        {
            get { return Branches == null || Branches.Length == 0; }
        }

        public DecisionNode(DecisionTree owner, DecisionNode parent, List<Video> inputs)
        {
            this.Owner = owner;
            this.Parent = parent;
            this.Inputs = inputs;

            if (IsRoot)
                this.Level = 0;
            else
                this.Level = this.Parent.Level + 1;
        }

        private Tube[] CreateFeatureCandidates()
        {
            Tube[] rVal = new Tube[Parameters.UsedFeatureCountAtEveryNode];

            for (int i = 0; i < Parameters.UsedFeatureCountAtEveryNode; i++)
            {
                rVal[i] = new Tube(199);
            }
            return rVal;
        }

        public static double GetBestDistance(double[] volumes, Tube feature)
        {
            double rVal = 0;
            
            int counter = 0;
            double diff = double.MaxValue;

            double mid = volumes.Average(); // best place to start

            double mean0, mean1;

            List<double>[] parts = new List<double>[2];
            parts[0] = new List<double>();
            parts[1] = new List<double>();
            
            do
            {
                for (int i = 0; i < volumes.Length; i++)
                {
                    if (volumes[i] < mid)
                        parts[0].Add(volumes[i]);
                    else
                        parts[1].Add(volumes[i]);
                }
                
                mean0 = parts[0].Count > 0 ? parts[0].Average() : 0;
                mean1 = parts[1].Count > 0 ? parts[1].Average() : 0;

                mid = (mean0 + mean1) / 2;
                feature.Threshold = mid;

                rVal = mean1 - mean0;

                counter++;
            }
            while (counter < 4 && diff > Parameters.DiffThreshold);

            rVal = rVal / feature.NormalizationScalar;

            return rVal;
        }

        public Tube GetBestFeature(Tube[] featureCandidates)
        {
            double[][] volumes = new double[featureCandidates.Length][];

            for (int i = 0; i < featureCandidates.Length; i++)
			{
                volumes[i] = new double[this.Inputs.Count];

                for (int j = 0; j < this.Inputs.Count; j++)
			    {
                    volumes[i][j] = this.Inputs[j].GetVolume(featureCandidates[i]);
			    }            
            }

            double[] distances = new double[featureCandidates.Length];

            for (int i = 0; i < featureCandidates.Length; i++)
            {
                distances[i] = GetBestDistance(volumes[i], featureCandidates[i]);
            }

            int index = distances.MaxIndex();

            return featureCandidates[index];
        }

        private DecisionNode[] Split(Tube tb)
        {
            this.Branches = new DecisionNode[2];

            List<Video>[] partitions = new List<Video>[2];
            partitions[0] = new List<Video>();
            partitions[1] = new List<Video>();

            for (int i = 0; i < this.Inputs.Count; i++)
            {
                if (this.Inputs[i].ApplyTube(tb))
                    partitions[0].Add(this.Inputs[i]);
                else
                    partitions[1].Add(this.Inputs[i]);
            }

            if (partitions[0].Count == 0 || partitions[1].Count == 0)
                return null;

            this.Branches[0] = new DecisionNode(this.Owner, this, partitions[0]);
            this.Branches[1] = new DecisionNode(this.Owner, this, partitions[1]);

            this.Inputs = null;
            this.SplitFeature = tb;

            return this.Branches;
        }

        public DecisionNode[] Split()
        {
            if (this.Level > Parameters.MaxDepth)
            {
                return null;
            }

            Tube[] features = CreateFeatureCandidates();
            Tube bestTube = GetBestFeature(features);

            return Split(bestTube);
        }
        
    //    private void CreateNodeImage()
    //    {
    //        double[,] accumulativeMatrix = new double[28, 28];
    //        for (int i = 0; i < this.Inputs.Length; i++)
    //        {
    //            accumulativeMatrix.Add(this.Inputs[i].OriginalGrayscaleMatrix);
    //        }

    //        for (int i = 0; i < accumulativeMatrix.GetLength(0); i++)
    //        {
    //            for (int j = 0; j < accumulativeMatrix.GetLength(1); j++)
    //            {
    //                accumulativeMatrix[i, j] /= this.Inputs.Length;
    //            }
    //        }

    //        Bitmap bmp = Visualizer.Visualize(accumulativeMatrix, this.SplitFeature.Rectangle0);

    //        if (!Directory.Exists(Parameters.GlobalImagePath))
    //            Directory.CreateDirectory(Parameters.GlobalImagePath);

    //        string path;
    //        if (!this.IsRoot)
    //            path = Parameters.GlobalImagePath + "\\" + this.Owner + "_Level_" + this.Level + "_This_" + this.NodeID + "_Parent_" + this.Parent.NodeID + "_NumberOfItems_" +
    //                        this.Inputs.Length + "_NumberRatio_" + (double)this.Inputs.Length / Parameters.NumImagesEachTree + "_Entropy_" + this.CurrentEntropy +
    //                        "_EntropyRatio_" + this.CurrentEntropy / this.Owner.Root.CurrentEntropy + ".png";
    //        else
    //            path = Parameters.GlobalImagePath + "\\" + this.Owner + "_Level_" + this.Level + "_This_" + this.NodeID + "_NumberOfItems_" +
    //                        this.Inputs.Length + "_NumberRatio_" + (double)this.Inputs.Length / Parameters.NumImagesEachTree + "_Entropy_" + this.CurrentEntropy +
    //                        "_EntropyRatio_" + this.CurrentEntropy / this.Owner.Root.CurrentEntropy + ".png";

    //        bmp.Save(path);
    //    }

    //    public DecisionNode PassToChild(Digit p)
    //    {
    //        bool temp = p.ComputeTestFeature(this.SplitFeature);

    //        if (temp == false)
    //            return this.Branches[0];
    //        return this.Branches[1];
    //    }

    //    public double[] GetHistogram(Digit[] inputs)
    //    {
    //        double[] rVal = new double[10];

    //        for (int i = 0; i < inputs.Length; i++)
    //        {
    //            rVal[inputs[i].DigitValue]++;
    //        }

    //        for (int i = 0; i < rVal.Length; i++)
    //        {
    //            rVal[i] = rVal[i] / inputs.Length;
    //        }

    //        return rVal;
    //    }
       
    }
}
