using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes;
using System.Threading;
using AdminConsole.TreeNodes.Nodes.Collection;
using System.Collections;
using AdminConsole.TreeNodes.Nodes.Single;
using AdminConsole.Code;
using DCRF.Core;
using NetSockets.Peer;
using DCRF.Definition;

namespace AdminConsole
{
    public partial class frmMain : Form
    {
        private List<ctlWebTree> trees = null;
        private ctlWebTree defaultTree = null;
        private BlockWeb dummyWeb = null;

        //this is public to be useful for blockweb nodes
        public Hashtable globalCodeState = new Hashtable();
        private int counter = 0;


        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            trees = new List<ctlWebTree>();
            trees.Add(treeTopLeft);
            trees.Add(treeTopRight);
            trees.Add(treeBottomLeft);
            trees.Add(treeBottomRight);

            treeTopLeft.Index = 0;
            treeTopRight.Index = 1;
            treeBottomLeft.Index = 2;
            treeBottomRight.Index = 3;

            treeTopLeft.trees = treeTopRight.trees = treeBottomRight.trees = treeBottomLeft.trees = trees;
            treeTopLeft.ImageList = treeTopRight.ImageList = treeBottomRight.ImageList = treeBottomLeft.ImageList = imgNodes;
            treeTopLeft.MenuImageList = treeTopRight.MenuImageList = treeBottomRight.MenuImageList = treeBottomLeft.MenuImageList = imgMenuItems;

            defaultTree = treeTopLeft;

            int random = new Random().Next(1000);
            dummyWeb = new BlockWeb("AdminConsole (" + random.ToString() + ")", null, null, PlatformType.Neutral, null, true);

            //dummyWeb.GetConnector(SysEventCode.PeerDisconnected).AttachEndPoint( .PeerDisconnected += new PeerConnectDelegate(dummyWeb_PeerDisconnected);

            treeTopLeft.container = treeTopRight.container = treeBottomRight.container = treeBottomLeft.container = dummyWeb;
        }

        void dummyWeb_PeerDisconnected(string peerId, bool isConnected, string host, int port)
        {
            this.Invoke(new MethodInvoker(delegate()
                {
                    MessageBox.Show("Peer \"" + peerId + "\" disconnected");
                }));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }
        
        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (ctlWebTree tree in trees)
            {
                tree.DisposeWebs();
            }

            dummyWeb.Dispose();
        }


        private void newCodeWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCode frm = new frmCode();
            frm.globalState = globalCodeState;
            frm.argWeb = null;
            frm.Text = "Script Window #" + (++counter).ToString();

            frm.Show();
        }

        private void topLeftTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeTopLeft.AddBlockWeb();
        }

        private void topRightTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeTopRight.AddBlockWeb();
        }

        private void bottomLeftTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeBottomLeft.AddBlockWeb();
        }

        private void bottomRightTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeBottomRight.AddBlockWeb();
        }
    }
}
