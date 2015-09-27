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
    public class ConnectorsNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";

        public ConnectorsNode(IBlockWeb web, string id)
        {
            myWeb = web;
            myId = id;            
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Connectors";
            myNode.Text = "Connectors";

            addTempNode(myNode);
        }

        protected override void buildChildrenList()
        {
            ICollection<string> cs = (ICollection<string>)myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ConnectorKeys, null, null);

            foreach (string c in cs)
            {
                children.Add(new ConnectorNode(myWeb, myId, c));
            }
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "REF" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF") refresh(myNode);
        }
    }
}
