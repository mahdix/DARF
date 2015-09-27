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
    class ServicesNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";

        public ServicesNode(IBlockWeb web, string id)
        {
            myWeb = web;
            myId = id;
            
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Services";
            myNode.Text = "Services";

            addTempNode(myNode);
        }

        protected override void  buildChildrenList()
        {
            children.Clear();

            List<string> services = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.Services, null, null) as List<string>;
            foreach (string service in services)
            {
                children.Add(new ServiceNode(myWeb, myId, service));
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
