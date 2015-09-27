using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Single;
using DCRF.Core;

namespace AdminConsole.TreeNodes.Nodes.Collection
{
    public class BlocksNode:BaseNode
    {
        public IBlockWeb myWeb = null;

        public BlocksNode(IBlockWeb web)
        {
            myWeb = web;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Blocks";
            myNode.Text = "Blocks";

            addTempNode(myNode);
        }

        protected override void buildChildrenList()
        {
            children.Clear();

            foreach (string id in myWeb.BlockIds)
            {
                children.Add(new BlockNode(myWeb, id));
            }
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "ADD", "REF" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
            else if (cmdKey == "ADD")
            {
                frmAddBlock frm = new frmAddBlock();
                frm.myWeb = myWeb;

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm.resultId != "")
                    {
                        //if node children are populated from server, add new child at the end
                        //else do not add and wait for expansion of the node
                        if (!requiresRefresh(myNode))
                        {
                            children.Add(new BlockNode(myWeb, frm.resultId));

                            //refresh but do not re-connect and re-read blocks, just update UI
                            refresh(myNode, false);
                        }
                        else
                        {
                            refresh(myNode, true);
                        }                        
                    }
                    else
                    {
                        MessageBox.Show("Error Adding Block");
                    }
                }
            }
        }
    }
}
