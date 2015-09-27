using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Single;
using DCRF.Core;
using DCRF.Primitive;

namespace AdminConsole.TreeNodes.Nodes.Collection
{
    class PeersNode : BaseNode
    {
        public IBlockWeb myWeb = null;

        public PeersNode(IBlockWeb web)
        {
            myWeb = web;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Services";
            myNode.Text = "Peers";

            addTempNode(myNode);
        }

        protected override void buildChildrenList()
        {
            children.Clear();

            Dictionary<string, string> peers = myWeb.GetBlockWebMetaInfo(BlockWebMetaInfoType.PeersInfo, null) as Dictionary<string, string>;
             
            foreach (string peer in peers.Keys)
            {
                string peerAddress = peers[peer];

                children.Add(new PeerNode(myWeb, peer, peerAddress));
            }
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "ADDPEER", "REF" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
            else if (cmdKey == "ADDPEER")
            {
                frmAddBlockWeb frm = new frmAddBlockWeb();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    string peerId = frm.Id;
                    string peerHost = frm.Host;
                    int peerPort = frm.Port;

                    myWeb.Connect(peerHost, peerPort, peerId);

                    refresh(myNode);
                }
            }
        }
    }
}
