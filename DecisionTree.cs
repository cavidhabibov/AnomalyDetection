using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnomalyDetection
{
    public class DecisionTree
    {
        public Forest Owner { get; private set; }
        public List<Video> Inputs { get; private set; }
        public DecisionNode Root { get; private set; }
        public int TreeIndex { get; private set; }

        public string LogString { get; private set; }

        private List<DecisionNode> NodeJobList = new List<DecisionNode>();

        public DecisionTree(Forest owner, List<Video> trainVideos, int treeIndex)
        {
            this.Owner = owner;
            this.Inputs = trainVideos;
            this.TreeIndex = treeIndex;

            this.Root = new DecisionNode(this, null, this.Inputs);

            this.NodeJobList.Add(this.Root);

            FillTree();

            this.Log();

            this.Inputs = null;
        }

        private void FillTree()
        {
            while (this.NodeJobList.Count > 0)
            {
                DecisionNode currentNode = this.NodeJobList[0];

                DecisionNode[] branches = currentNode.Split();
                if (branches != null)
                {
                    this.NodeJobList.AddRange(branches);
                }

                this.NodeJobList.RemoveAt(0);
            }
        }

        public void Log()
        {
            LogString = "";
        }

        public override string ToString()
        {
            return "Tree " + this.TreeIndex.ToString();
        }
    }
}
