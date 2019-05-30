using GraphConstructor.GraphLogic;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphConstructor
{
    class MainForm : Form
    {
        private readonly Graph _graph;
        private readonly Box _box;
        private readonly Algorithms _algorithm = new Algorithms();
        private readonly ToolStripMenuItem _alg;
        private readonly ToolStripMenuItem _startNode;

        public MainForm(Graph graph)
        {
            BackColor = Color.LightGray;
            _graph = graph;
            _box = new Box(graph);
            AddListeners(_box);
            Controls.Add(_box);

            _alg = new ToolStripMenuItem("Algorithm");
            var bfs = new ToolStripMenuItem("BFS") { Checked = true, CheckOnClick = true };
            var dfs = new ToolStripMenuItem("DFS") { CheckOnClick = true };
            var dijkstra = new ToolStripMenuItem("Dijkstra") {CheckOnClick = true}; 
            _alg.DropDownItems.Add(bfs);
            _alg.DropDownItems.Add(dfs);
            _alg.DropDownItems.Add(dijkstra);
            _alg.DropDownItemClicked += (sender, args) => SubMenuClick(sender, args.ClickedItem);

            _startNode = new ToolStripMenuItem("Start node") { CheckOnClick = true };
            graph.OnNodeAdd += (n) =>
            {
                var item = new ToolStripMenuItem(n.Id.ToString()) { CheckOnClick = true };
                _startNode.DropDownItems.Add(item);
            };
            graph.OnNodeRemove += () => _startNode.DropDownItems.RemoveAt(_startNode.DropDownItems.Count - 1);
            _startNode.DropDownItemClicked += (sender, args) => SubMenuClick(sender, args.ClickedItem);

            var startButton = new ToolStripMenuItem("Start");
            startButton.Click += (sender, args) => StartAlgorithm();

            var menuStrip = new MenuStrip();
            menuStrip.Items.Add(_alg);
            menuStrip.Items.Add(_startNode);
            menuStrip.Items.Add(startButton);
            Controls.Add(menuStrip);

            Load += (sender, args) => OnSizeChanged(EventArgs.Empty);
            SizeChanged += (sender, args) =>
            {
                _box.Location = new Point(0, 70);
                _box.Size = new Size(ClientSize.Width, ClientSize.Height - 70);
                _box.BackColor = Color.AliceBlue;
            };

        }

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        private void SubMenuClick(object sender, ToolStripItem clickedItem)
        {
            var baseMenuItem = sender as ToolStripMenuItem;
            baseMenuItem?.DropDownItems
                .OfType<ToolStripMenuItem>()
                .ToList()
                .ForEach(item =>
                {
                    if (item != clickedItem)
                        item.Checked = false;
                });
        }

        private void AddListeners(GroupBox groupBox)
        {
            var onHold = false;
            var source = new Node(0);
            var destination = new Node(0);
            groupBox.MouseDown += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    if (_graph.GetX(args.X, args.Y) == null)
                    {
                        _graph.Add(new Node(_graph.NodesCount)
                        {
                            X = args.X,
                            Y = args.Y
                        });
                    }
                    else
                    {
                        source = _graph.GetX(args.X, args.Y);
                        onHold = true;
                    }
                }
                else if (args.Button == MouseButtons.Right)
                    _graph.Remove(_graph.GetX(args.X, args.Y));
                groupBox.Invalidate();
            };

            groupBox.MouseUp += (sender, args) =>
            {
                if (args.Button == MouseButtons.Left)
                {
                    destination = _graph.GetX(args.X, args.Y);
                    if (source != null && destination != null && source != destination && onHold)
                        _graph.Connect(source, destination);
                    groupBox.Invalidate();
                    onHold = false;
                }
            };
        }

        private void StartAlgorithm()
        {
            _algorithm.OnVisit += () => _box.Invalidate();
            _algorithm.OnView += () => _box.Invalidate();

            var index = GetCheckedItem(_startNode);
            if (_graph.NodesCount == 0) return;
            var node = index == null ? _graph.Nodes.First() : _graph.Nodes[int.Parse(index.Text)];

            Task task = null;
            switch (GetCheckedItem(_alg).Text)
            {
                    case("BFS"):
                        task = new Task(() => _algorithm.BreadthSearch(node));
                        break;
                    case("DFS"):
                        task = new Task(() => _algorithm.DepthSearch(node));
                        break;
                    case("Dijkstra"):
                        task = new Task(() => _algorithm.Dijkstra(node, _graph));
                        break;
            }

            task.ContinueWith(Refresh());
            task.Start();

        }

        private Action<Task> Refresh()
        {
            var act = new Action<Task>((s) =>
            {
                _graph.Nodes.ForEach(n =>
                {
                    n.Viewed = false;
                    n.Visited = false;
                });
                _box.Invalidate();
            });
            return act;
        }

        private ToolStripMenuItem GetCheckedItem(ToolStripMenuItem item)
        {
            return item?.DropDownItems
                .OfType<ToolStripMenuItem>()
                .ToList()
                .FirstOrDefault(i => i.Checked);
        }
    }
}
