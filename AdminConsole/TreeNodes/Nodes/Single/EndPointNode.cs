using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Collection;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    public class EndPointNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";
        public string myConnectorKey = null;
        public string ep = null;

        public EndPointNode(IBlockWeb web, string id, string key, string ep)
        {
            myWeb = web;
            myId = id;
            myConnectorKey = key;
            this.ep = ep;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "EndPoint";
            myNode.Text = ep;
        }

        protected override void buildChildrenList()
        {
            children.Clear();
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            List<string> result = new List<string>() { "REF"};

            return result;
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
        }
    }
}