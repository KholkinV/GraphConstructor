using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphConstructor.GraphLogic;

namespace GraphConstructor
{
    class Box : GroupBox
    {
        private Color color;
        public Box(Graph graph)
        {
            DoubleBuffered = true;
            Paint += (sender, args) =>
            {
                DrawEdges(args.Graphics, graph);
                DrawNodes(args.Graphics, graph);
            };
        }

        private void DrawNodes(Graphics g, Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                //var color = node.Visited ? Color.Firebrick : Color.LightYellow;
                if (node.Viewed) color = Color.Gray;
                else if (node.Visited) color = Color.Firebrick;
                else color = Color.LightYellow;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawEllipse(new Pen(Color.Black), new Rectangle(node.Location, new Size(30, 30)));
                g.FillEllipse(new SolidBrush(color), new Rectangle(node.Location, new Size(30, 30)));
                g.DrawString(
                    node.Id.ToString(),
                    new Font("Arial", 12),
                    new SolidBrush(Color.Black),
                    new Point(node.X + 8, node.Y + 7)
                );
                node.Viewed = false;
            }
        }

        private void DrawEdges(Graphics g, Graph graph)
        {
            foreach (var edge in graph.Edges)
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.DrawLine(new Pen(Color.DarkOrange, 2), edge[0].X + 15, edge[0].Y + 15, edge[1].X + 15, edge[1].Y + 15);
                g.DrawString(
                    edge.Weight.ToString(), 
                    new Font("Arial", 10), 
                    new SolidBrush(Color.Black), 
                    new Point((edge.FirstNode.X + edge.SecondNode.X) / 2,
                        (edge.FirstNode.Y + edge.SecondNode.Y) / 2)
                );
            }
        }
    }
}
