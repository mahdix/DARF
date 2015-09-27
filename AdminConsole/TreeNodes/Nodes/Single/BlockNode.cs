using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using DCRF.Primitive;
using AdminConsole.TreeNodes.Nodes.Collection;
using DCRF.Core;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    class BlockNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";
        public IBlock myBlock = null;
        public BlockHandle info = null;

        private List<TreeNode> myNodes = new List<TreeNode>();

        public BlockNode(IBlockWeb web, string id)
        {
            myWeb = web;
            myId = id;            
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Block";

            if ( info == null ) info = myWeb.GetBlockHandle(myId);

            myNode.Text = myId + "<" + info.ToString() + ">";

            addTempNode(myNode);
            myNodes.Add(myNode);
        }

        protected override void  buildChildrenList()
        {
            //we only need to create children of a block node once
            if (children.Count > 0) return;

            children.Add(new ServicesNode(myWeb, myId));
            children.Add(new ConnectorsNode(myWeb, myId));

            if ( myBlock == null ) myBlock = myWeb[myId] as IBlock;

            IBlockWeb innerWeb = myBlock.ProcessRequest("ProcessMetaService", BlockMetaServiceType.GetInnerWeb, null,null) as IBlockWeb;

            if (innerWeb != null)
            {
                children.Add(new BlockWebNode(innerWeb));
            }
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "COP", "DELB", "INFO", "REF" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
            else if (cmdKey == "COP")
            {
                Clipboard.SetData(DataFormats.Text, myId.ToString());
            }
            else if (cmdKey == "DELB")
            {
                if (MessageBox.Show("Are you sure you want to delete this block?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    myNode.TreeView.SelectedNode = myNode.Parent;
                    if (myBlock == null) myBlock = myWeb[myId] as IBlock;


                    //web will dispose block before deletion
                    //block wil dispose innerWeb before dispose
                    myWeb.DeleteBlock(myId);

                    //deleting from all parent nodes
                    foreach (TreeNode node in myNodes)
                    {
                        //this will remove all my children too
                        node.TreeView.Nodes.Remove(node);
                    }

                    children.Clear();
                }
            }
            else if (cmdKey == "INFO")
            {
                string info = (string)myWeb[myId].ProcessRequest("GetMetaInfo", BlockMetaInfoType.BlockInfo, null, null);
                MessageBox.Show(info, "Block MetaInfo - "+myId.ToString());
            }
        }
    }
}
