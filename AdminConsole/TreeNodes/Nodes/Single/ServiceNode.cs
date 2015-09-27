using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using DCRF.Core;
using DCRF.Primitive;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    class ServiceNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";
        public string myService = null;

        public ServiceNode(IBlockWeb web, string id, string service)
        {
            myWeb = web;
            myId = id;
            myService = service;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Service";
            myNode.Text = myService;
        }

        protected override void buildChildrenList()
        {
            children.Clear();
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "INVOKE", "QINVOKE", "JINVOKE", "INFO" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "INVOKE" || cmdKey == "QINVOKE")
            {
                frmInvoker frm = new frmInvoker();

                frm.Text = "Invoke Service: " + myService + " in " + myId.ToString();
                frm.myWeb = myWeb;
                frm.myId = myId;
                frm.myService = myService;
                frm.IsQuickInvoke = (cmdKey == "QINVOKE"); 


                //so multiple invoke service forms can be open at any time
                frm.Show();
            }
            else if (cmdKey == "JINVOKE")
            {
                object result = myWeb[myId].ProcessRequest(myService);
                MessageBox.Show("Service "+myService+" invoked. Result: "+(result == null ? "(null)":result.ToString()));
            }
            else if (cmdKey == "INFO")
            {
                object result = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ServiceInfo, myService);
                MessageBox.Show(result == null ? "(null)" : result.ToString(), "Service Info");
            }
        }
    }
}
