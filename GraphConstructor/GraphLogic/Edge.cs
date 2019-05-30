using System;

namespace GraphConstructor.GraphLogic
{
    public class Edge
    {
        public Node FirstNode { get; set; }
        public Node SecondNode { get; set; }
        public int Weight { get; set; }

        public Edge(Node firstNode, Node secondNode, int weight = 1)
        {
            FirstNode = firstNode;
            SecondNode = secondNode;
            Weight = weight;
        }

        public Node this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return FirstNode;
                    case 1: return SecondNode;
                }
                throw new IndexOutOfRangeException();
            }
        }
    }
}
