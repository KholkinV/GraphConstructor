using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GraphConstructor.GraphLogic
{
    public class Graph
    {
        public List<Node> Nodes = new List<Node>();
        public List<Edge> Edges = new List<Edge>();
        public event Action<Node> OnNodeAdd;
        public event Action OnNodeRemove; 

        public Node this[int index] => Nodes[index];

        public int NodesCount => Nodes.Count;

        public void Add(Node n)
        {
            Nodes.Add(n);
            OnNodeAdd?.Invoke(n);
        }

        public Node Get(int x, int y)
        {
            return (from v in Nodes where v.X == x && v.X == y select v).FirstOrDefault();
        }

        public Node GetX(int x, int y)
        {
            var r = new Rectangle(new Point(x - 40, y - 40), new Size(40, 40));
            return Nodes.FirstOrDefault(node => r.Contains(new Point(node.X, node.Y)));
        }

        public void Remove(Node n)
        {
            Nodes.Remove(n);
            OnNodeRemove?.Invoke();
            Nodes.Sort();

            for (var i = 0; i < NodesCount; i++)
                Nodes[i].Id = i;

            Disconnect(n);
        }

        public void Connect(Node n1, Node n2)
        {
            if (Edges.Any(edge => (edge[0] == n1 && edge[1] == n2) || (edge[0] == n2 && edge[1] == n1)))
            {
                return;
            }
            Edges.Add(new Edge(n1, n2));
            n1.Connect(n2);
        }

        public void Disconnect(Node n)
        {
            var garbage = new Stack<Edge>();

            foreach (var edge in Edges)
            {
                if (edge[0] != n && edge[1] != n) continue;
                garbage.Push(edge);
                n.Disconnect(edge[0] == n ? edge[1] : edge[0]);
            }

            while (garbage.Count > 0)
                Edges.Remove(garbage.Pop());

        }


    }
}
