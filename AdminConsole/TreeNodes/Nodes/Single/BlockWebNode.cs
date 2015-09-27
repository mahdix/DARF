using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Collection;
using DCRF.Core;
using AdminConsole.Code;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    public class BlockWebNode: BaseNode
    {
        private IBlockWeb myWeb = null;
        public string id = "";
        public string host = "";
        public int port = 0;

        //used for unload
        private List<TreeNode> myNodes = new List<TreeNode>();

        public BlockWebNode(IBlockWeb web)
        {
            myWeb = web;

            //TODO: fix web.PeerDisconnected += new NetSockets.Peer.PeerConnectDelegate(web_Disconnected);
        }

        void web_Disconnected(string peerId, bool isConnected, string host, int port)
        {
            ExecuteCommand("UNLOAD", null);
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "BlockWeb";            
            myNode.Text = myWeb.Id + " @ " + myWeb.Address;

            addTempNode(myNode);
            myNode.Collapse();

            myNodes.Add(myNode);
        }

        protected override void buildChildrenList()
        {
            if (children.Count > 0) return;

            children.Add(new PeersNode(myWeb));
            children.Add(new BlocksNode(myWeb));
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            List<string> result = new List<string>();
            result.Add("ADD");
            result.Add("CODE");

            //copy to any of other 3 trees
            result.Add("COPT0");
            result.Add("COPT1");
            result.Add("COPT2");
            result.Add("COPT3");

            result.Add("RELOADWEB");
            result.Add("RELOAD");

            if (myNode.Parent == null)
            {
                result.Add("UNLOAD");
            }

            result.Add("REF");


            return result;
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
            else if (cmdKey == "ADD")
            {
                //pass to first child
                if (requiresRefresh(myNode)) refresh(myNode, true);

                foreach (TreeNode tn in myNode.Nodes)
                {
                    if (tn.Tag is BlocksNode)
                    {
                        (tn.Tag as BlocksNode).ExecuteCommand("ADD", myNode.Nodes[0]);
                        break;
                    }
                }
            }
            else if (cmdKey == "UNLOAD")
            {
                //maybe unload is called more than once
                if (myWeb == null) return;

                //when web is disposed, it disposes all of its blocks and they dispose their
                //related innerweb so this command will be propagated
                myWeb.Dispose();
                myWeb = null;

                if (myNodes.Count > 0)
                {
                    TreeView tv = myNodes[0].TreeView;

                    tv.Invoke(new MethodInvoker(delegate()
                        {
                            foreach (TreeNode node in myNodes)
                            {
                                //maybe parent treeview is disposed
                                if (node != null && node.TreeView != null && !node.TreeView.IsDisposed && node.TreeView.Nodes != null)
                                {
                                    //this will remove all my children too
                                    node.Remove();
                                }
                            }
                        }));
                }

                myNodes.Clear();
                children.Clear();
            }
            else if (cmdKey == "CODE")
            {
                frmCode frm = new frmCode();

                Control container = myNode.TreeView.Parent;

                while (container.Parent != null)
                {
                    container = container.Parent;
                }

                frm.globalState = (container as frmMain).globalCodeState;
                frm.argWeb = myWeb;
                frm.Text = "Script Window - " + myWeb.Id + " @ " + myWeb.Address;

                frm.Show();
            }
            else if (cmdKey == "RELOAD")
            {
                if (MessageBox.Show("Are you sure you want to re-load all Blocks in this BlockWeb?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    myWeb.ReloadBlocks();
                    myNode.Collapse();
                    myNode.Nodes.Clear();
                    refresh(myNode, true);
                }
            }
            else if (cmdKey == "RELOADWEB")
            {
                myNode.Collapse();

                myWeb.Dispose();
                myWeb = null;
                children.Clear();

                foreach (TreeNode node in myNodes)
                {
                    //maybe parent treeview is disposed
                    if (node != null && node.TreeView != null && !node.TreeView.IsDisposed && node.TreeView.Nodes != null)
                    {
                        //this will remove all my children too
                        node.Nodes.Clear();
                        node.Collapse();
                    }
                }

                Control container = myNode.TreeView.Parent;

                while (!(container is ctlWebTree))
                {
                    container = container.Parent;
                }

                ctlWebTree parentCtl = container as ctlWebTree;
                parentCtl.loadBlockWeb(id, host, port, myNodes);
            }
        }

    }
}
