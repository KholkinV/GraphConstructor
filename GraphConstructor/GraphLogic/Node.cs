using System;
using System.Collections.Generic;
using System.Drawing;

namespace GraphConstructor.GraphLogic
{
    public class Node : IComparable
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Point Location => new Point(X, Y);
        public bool Visited { get; set; }
        public bool Viewed { get; set; }

        public readonly List<Node> IncidentNodes = new List<Node>();

        public void Connect(Node anotherNode)
        {
            IncidentNodes.Add(anotherNode);
            anotherNode.IncidentNodes.Add(this);
        }

        public void Disconnect(Node anotherNode)
        {
            IncidentNodes.Remove(anotherNode);
            anotherNode.IncidentNodes.Remove(this);
        }

        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Node) obj).Id);
        }

        public Node(int id)
        {
            Id = id;
        }
    }
}
