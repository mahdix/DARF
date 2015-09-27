using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Collection;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    public class PeerNode : BaseNode
    {
        public IBlockWeb myWeb = null;
        private string peerId = null;
        private string peerAddress = null;

        public PeerNode(IBlockWeb web, string peerId, string peerAddress)
        {
            myWeb = web;
            this.peerId = peerId;
            this.peerAddress = peerAddress;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "EndPoint";
            myNode.Text = string.Format("{0} @ {1}", peerId, peerAddress);
        }

        protected override void buildChildrenList()
        {
            children.Clear();
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            List<string> result = new List<string>() { "DELPEER" };

            return result;
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "DELPEER")
            {
                if (MessageBox.Show("Are you sure you want to disconnect from " + peerId + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    myWeb.Disconnect(peerId);

                    myNode.TreeView.SelectedNode = myNode.Parent;
                    myNode.Parent.Nodes.Remove(myNode);
                }
            }

        }
    }
}