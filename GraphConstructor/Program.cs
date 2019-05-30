using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphConstructor.GraphLogic;

namespace GraphConstructor
{
    static class Program
    {
        static void Main()
        {
            var graph = new Graph();
            Application.Run(new MainForm(graph) {ClientSize = new Size(800, 500)});
        }
    }
}
