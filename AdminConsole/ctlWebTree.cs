using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AdminConsole.TreeNodes.Nodes;
using AdminConsole.TreeNodes.Nodes.Single;
using AdminConsole.TreeNodes.Nodes.Collection;
using System.Collections;
using System.Threading;
using DCRF.Core;
using DCRF.Interface;

namespace AdminConsole
{
    public partial class ctlWebTree : UserControl
    {
        public List<ctlWebTree> trees = null;
        public int Index = -1;
        private static Random r = new Random();
        public IBlockWeb container = null;

        public ctlWebTree()
        {
            InitializeComponent();
        }

        public ImageList ImageList
        {
            set
            {
                innerTree.ImageList = value;
            }
        }

        public ImageList MenuImageList
        {
            set
            {
                contextMenuStrip1.ImageList = value;
            }
        }

        private void treeTopLeft_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            (e.Node.Tag as BaseNode).OnExpand(e.Node);
        }

        private void treeTopLeft_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;

            if (node.Tag is ServiceNode || node.Tag is BlockNode)
            {
                DoDragDrop(node, DragDropEffects.Link);
            }
        }

        private void treeTopLeft_DragDrop(object sender, DragEventArgs e)
        {
            TreeView tree = sender as TreeView;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = tree.PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = tree.GetNodeAt(pt);
                TreeNode sourceNode = (TreeNode)e.Data.GetData("System.Windows.Forms.TreeNode");

                if (destinationNode.Tag is ConnectorNode)
                {
                    destinationNode = destinationNode.Nodes[0];
                }

                if (!(destinationNode.Tag is EndPointsNode)) return;

                EndPointsNode epn = destinationNode.Tag as EndPointsNode;

                if (sourceNode.Tag is ServiceNode)
                {
                    ServiceNode service = sourceNode.Tag as ServiceNode;

                    epn.doAttachEndPoint(destinationNode, service.myId, service.myService, null);
                }
                else if (sourceNode.Tag is BlockNode)
                {
                    BlockNode block = sourceNode.Tag as BlockNode;

                    epn.doAttachEndPoint(destinationNode, block.myId, null, null);
                }
                else if (sourceNode.Tag is ConnectorNode && sourceNode != destinationNode.Parent)
                {
                    ConnectorNode connector = sourceNode.Tag as ConnectorNode;

                    epn.doAttachEndPoint(destinationNode, connector.myId, null, connector.myConnectorKey);
                }
            }
        }

        private void treeTopLeft_DragOver(object sender, DragEventArgs e)
        {
            TreeView tree = sender as TreeView;

            if (e.Data.GetDataPresent("System.Windows.Forms.TreeNode", false))
            {
                Point pt = tree.PointToClient(new Point(e.X, e.Y));
                TreeNode destinationNode = tree.GetNodeAt(pt);

                if (destinationNode != null)
                {
                    if (destinationNode.Tag is ConnectorNode)
                    {
                        destinationNode.Expand();
                    }

                    tree.SelectedNode = destinationNode;
                }
            }
        }

        private void treeTopLeft_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void treeTopLeft_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode activeNode = innerTree.GetNodeAt(e.X, e.Y);
                List<string> cmdList = new List<string>();

                if (activeNode == null)
                {
                    return;
                }
                else
                {
                    innerTree.SelectedNode = activeNode;

                    BaseNode node = activeNode.Tag as BaseNode;

                    cmdList = node.GetCommands(activeNode);
                }

                contextMenuStrip1.Items.Clear();

                foreach (string key in cmdList)
                {
                    string title = GetTitle(key, innerTree.SelectedNode);

                    ToolStripItem item = contextMenuStrip1.Items.Add(title);
                    item.ImageKey = key;
                    item.Tag = key;
                    item.Click += new EventHandler(item_Click);
                }
            }
        }

        private string GetTitle(string key, TreeNode myNode)
        {
            if (key == "REF") return "Refresh";
            if (key == "ADD") return "Add New Block...";
            if (key == "ATT") return "Attach EndPoint...";
            if (key == "DET") return "Detach Endpoint";
            if (key == "INVOKE") return "Invoke...";
            if (key == "COP") return "Copy ID to Clipboard";
            if (key == "QINVOKE") return "Quick Invoke...";
            if (key == "UNLOAD") return "Unload BlockWeb";
            if (key == "DELB") return "Delete Block";
            if (key == "CODE") return "New Script Window...";
            if (key == "RELOAD") return "Reload Blocks from Disk";
            if (key == "JINVOKE") return "Just Invoke";
            if (key == "INFO") return "Get MetaInfo";
            if (key == "RELOADWEB") return "Reload BlockWeb";
            if (key == "ADDNEW") return "Add New BlockWeb...";
            if (key == "ADDPEER") return "Connect to...";
            if (key == "DELPEER") return "Disconnect from peer";

            if (key.StartsWith("COPT"))
            {
                int deltaIdx = int.Parse(key.Replace("COPT", ""));

                ctlWebTree target = trees[(Index + deltaIdx) % 4];

                switch (target.Name)
                {
                    case "treeTopLeft": return "Copy to Top Left Tree";
                    case "treeTopRight": return "Copy to Top Right Tree";
                    case "treeBottomLeft": return "Copy to Bottom Left Tree";
                    case "treeBottomRight": return "Copy to Bottom Right Tree";
                }
            }

            return key;
        }

        void item_Click(object sender, EventArgs e)
        {
            TreeNode activeNode = innerTree.SelectedNode;
            BaseNode node = activeNode.Tag as BaseNode;

            string cmd = (sender as ToolStripItem).Tag.ToString();

            try
            {
                if (cmd.StartsWith("COPT"))
                {
                    int deltaIndex = int.Parse(cmd.Replace("COPT", ""));
                    ctlWebTree otherTree = trees[(Index + deltaIndex) % 4];

                    otherTree.AttachBlockWeb(node as BlockWebNode);
                }
                else
                {
                    node.ExecuteCommand(cmd, activeNode);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show("Error executing command: " + exc.Message + "\r\nTrace: " + exc.StackTrace);
            }
        }

        private void AttachBlockWeb(BlockWebNode blockWebNode)
        {
            TreeNode node = innerTree.Nodes.Add("");
            blockWebNode.Attach(node);
        }

        private void innerTree_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                TreeNode activeNode = e.Node;

                if (activeNode == null) return;

                if (activeNode.Nodes.Count == 0)
                {
                    (activeNode.Tag as BaseNode).ExecuteCommand("INVOKE", activeNode);
                }
            }
        }

        public void DisposeWebs()
        {
            ICollection nodes = innerTree.Nodes;

            //unload all existing blockwebs
            foreach (TreeNode node in nodes)
            {
                BaseNode blockWebNode = node.Tag as BaseNode;

                blockWebNode.ExecuteCommand("UNLOAD", null);
            }
        }

        public void AddBlockWeb()
        {
            frmAddBlockWeb frm = new frmAddBlockWeb();

            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TreeNode node = innerTree.Nodes.Add("...loading " + frm.Id+ "...");

                loadBlockWeb(frm.Id, frm.Host, frm.Port, new List<TreeNode>() { node });
            }
        }

        public void loadBlockWeb(string id, string host, int port, List<TreeNode> parents)
        {
            Thread tempThread = new Thread(new ThreadStart(delegate()
            {
                try
                {
                    if (!container.Connect(host, port, id))
                    {
                        throw new Exception();
                    }
                    
                    BlockWebNode myWebNode = new BlockWebNode(container.GetPeer(id));
                    myWebNode.id = id;
                    myWebNode.host = host;
                    myWebNode.port = port;

                    innerTree.Invoke(new MethodInvoker(delegate()
                    {
                        foreach (TreeNode node in parents)
                        {
                            myWebNode.Attach(node);
                        }

                        imgLoading.Visible = false;
                    }));
                }
                catch (Exception exc)
                {
                    imgLoading.Invoke(new MethodInvoker(delegate()
                    {
                        imgLoading.Visible = false;

                        //if failed, detach treenodes from their parents
                        foreach (TreeNode node in parents)
                        {
                            if (node.Parent == null)
                            {
                                node.TreeView.Nodes.Remove(node);
                            }
                            else
                            {
                                node.Parent.Nodes.Remove(node);
                            }
                        }

                        MessageBox.Show("Error Connecting: " + exc.Message);
                    }));
                }
            }));

            imgLoading.Visible = true;
            tempThread.Start();
        }

        private void ctlWebTree_Load(object sender, EventArgs e)
        {
            imgLoading.Left = (Width - imgLoading.Width) / 2;
            imgLoading.Top = (Height - imgLoading.Height) / 2;
        }

        private void ctlWebTree_Resize(object sender, EventArgs e)
        {
            imgLoading.Left = (Width - imgLoading.Width) / 2;
            imgLoading.Top = (Height - imgLoading.Height) / 2;
        }
    }
}
