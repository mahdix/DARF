using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DCRF.Interface;
using AdminConsole.TreeNodes.Nodes.Collection;
using DCRF.Core;
using DCRF.Primitive;

namespace AdminConsole.TreeNodes.Nodes.Single
{
    public class ConnectorNode: BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";  //if this is empty, refer to globalconnectors
        public string myConnectorKey = null;

        public ConnectorNode(IBlockWeb web, string id, string key)
        {
            myWeb = web;
            myId = id;
            myConnectorKey = key;
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "Connector";
            myNode.Text = myConnectorKey;

            addTempNode(myNode);
        }

        protected override void buildChildrenList()
        {
            if (children.Count > 0) return;

            children.Add(new EndPointsNode(myWeb, myId, myConnectorKey));
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            List<string> result = new List<string>() { "ATT", "INVOKE", "QINVOKE", "JINVOKE"};

            //do not show info for global connectors
            if (this.myId != "")
            {
                result.Add("INFO");
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
            else if (cmdKey == "INVOKE" || cmdKey == "QINVOKE")
            {
                frmInvoker frm = new frmInvoker();

                if (myId == "")
                {
                    frm.Text = "Invoke Global Connector: " + myConnectorKey + " in " + myWeb.Id + " @ " + myWeb.Address;
                }
                else
                {
                    frm.Text = "Invoke Connector: " + myConnectorKey + " in " + myId.ToString();
                }

                frm.myWeb = myWeb;
                frm.myId = myId;
                frm.myConnectorKey = myConnectorKey;
                frm.IsQuickInvoke = (cmdKey == "QINVOKE");

                //so multiple invoke service forms can be open at any time
                frm.Show();
            }
            else if (cmdKey == "ATT")
            {
                //maybe this node is not refreshed yet
                if (requiresRefresh(myNode)) refresh(myNode, true);
                //pass this to child node (endpoints)
                (myNode.Nodes[0].Tag as BaseNode).ExecuteCommand(cmdKey, myNode.Nodes[0]);
            }
            else if (cmdKey == "JINVOKE")
            {
                object result = null;

                if (myId != "")
                {
                    //block connector
                    result = myWeb[myId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.InvokeConnector, myConnectorKey, null);
                }
                else
                {
                    //global connector
                    result = null; // myWeb.GetConnector(myConnectorKey).ProcessRequest();
                }

                MessageBox.Show("Connector " + myConnectorKey + " invoked. Result: " + (result == null ? "(null)" : result.ToString()));
            }
            else if (cmdKey == "INFO")
            {
                object result = myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ConnectorInfo, myConnectorKey);
                MessageBox.Show(result == null ? "(null)" : result.ToString(),"Connector Info");
            }
        }      
    }
}
