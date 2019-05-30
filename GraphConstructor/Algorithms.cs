using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using GraphConstructor.GraphLogic;

namespace GraphConstructor
{
    public class DijkstraData
    {
        public int Price { get; set; }
        public Node Previous { get; set; }
        public List<Node> Path { get; set; }
    }
    
    public class Algorithms
    {
        public event Action OnVisit;
        public event Action OnView;

        public void DepthSearch(Node startNode)
        {
            var stack = new Stack<Node>();
            var visited = new HashSet<Node>();
            stack.Push(startNode);

            while (stack.Count != 0)
            {
                var node = stack.Pop();
                if (visited.Contains(node)) continue;
                visited.Add(node);
                node.Visited = true;
                OnVisit?.Invoke();
                foreach (var nextNode in node.IncidentNodes)
                {
                    stack.Push(nextNode);
                }
                Thread.Sleep(500);
            }
        }

        public void BreadthSearch(Node startNode)
        {
            var queue = new Queue<Node>();
            var visited = new HashSet<Node>();
            queue.Enqueue(startNode);

            while (queue.Count != 0)
            {
                var node = queue.Dequeue();
                if (visited.Contains(node)) continue;
                visited.Add(node);
                node.Visited = true;
                OnVisit?.Invoke();
                foreach (var nextNode in node.IncidentNodes)
                {
                    queue.Enqueue(nextNode);
                }
                Thread.Sleep(500);
            }
        }

        public Dictionary<Node, DijkstraData> Dijkstra(Node startNode, Graph graph)
        {
            var notVisited = graph.Nodes.ToList();
            var track = new Dictionary<Node, DijkstraData>
            {
                [startNode] = new DijkstraData {Previous = null, Price = 0}
            };

            while (notVisited.Count > 0)
            {
                Node toOpen = null;
                var bestPrice = int.MaxValue;
                foreach (var v in notVisited)
                {
                    if (track.ContainsKey(v) && track[v].Price < bestPrice)
                    {
                        toOpen = v;
                        bestPrice = track[v].Price;
                    }
                }

                if (toOpen == null) return null;
                toOpen.Visited = true;
                OnVisit?.Invoke();
                foreach (var e in graph.Edges.Where(z => z.FirstNode == toOpen || z.SecondNode == toOpen))
                {
                    var currentPrice = track[toOpen].Price + e.Weight;
                    var nextNode = e.FirstNode == toOpen ? e.SecondNode : e.FirstNode;
                    if (!nextNode.Visited)
                    {
                        nextNode.Viewed = true;
                        OnView?.Invoke();
                    }
                    if (!track.ContainsKey(nextNode) || track[nextNode].Price > currentPrice)
                        track[nextNode] = new DijkstraData {Price = currentPrice, Previous = toOpen}; 
                    Thread.Sleep(500);
                }

                notVisited.Remove(toOpen);
                Thread.Sleep(750);
            }

            foreach (var pair in track)
            {
                var node = pair.Key;
                var path = new List<Node>();
                var end = track[node].Previous;
                while (end != null)
                {
                    path.Add(end);
                    end = track[end].Previous;
                }

                path.Reverse();
                track[node].Path = path;
            }

            return track;
        }
    }
}
