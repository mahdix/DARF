using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;
using System.Windows.Forms;
using AdminConsole.TreeNodes.Nodes.Single;
using DCRF.Primitive;

namespace AdminConsole.TreeNodes.Nodes.Collection
{
    public class EndPointsNode : BaseNode
    {
        public IBlockWeb myWeb = null;
        public string myId = "";
        public string myConnectorKey = null;

        public EndPointsNode(IBlockWeb web, string id, string key)
        {
            myWeb = web;
            myId = id;
            myConnectorKey = key;            
        }

        public override void Attach(TreeNode myNode)
        {
            myNode.Tag = this;
            myNode.SelectedImageKey = myNode.ImageKey = "EndPoints";
            myNode.Text = "EndPoints";

            addTempNode(myNode);
        }

        protected override void buildChildrenList()
        {
            children.Clear();

            //we are just allowed to get a list of endpoints description
            string ep = (string)myWeb[myId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.ConnectorEndpoint, myConnectorKey, null);

            children.Add(new EndPointNode(myWeb, myId, myConnectorKey, ep));
        }

        public override List<string> GetCommands(TreeNode myNode)
        {
            return new List<string>() { "ATT", "REF" };
        }

        public override void ExecuteCommand(string cmdKey, TreeNode myNode)
        {
            if (cmdKey == "REF")
            {
                refresh(myNode);
            }
            else if (cmdKey == "ATT")
            {
                doAttachEndPoint(myNode);
            }
        }

        public void doAttachEndPoint(TreeNode myNode)
        {
            doAttachEndPoint(myNode, "", null, null);
        }

        public void doAttachEndPoint(TreeNode myNode, string blockId, string service, string connector)
        {
            ////attach endpoint
            //frmAttachEndPoint ep = new frmAttachEndPoint();

            //if (blockId != "") ep.BlockHandle = blockId;
            //if (service != null)
            //{
            //    ep.Service = service;
            //    ep.IsValue = false;
            //}

            //if (connector != null)
            //{
            //    ep.ChainConnectorKey = connector;
            //    ep.IsValue = false;
            //}

            //ep.Text = "Attach EndPoint for " + myConnectorKey;

            //if (ep.ShowDialog() == DialogResult.OK)
            //{
            //    string epKEy = null;
            //    string desc = "";

            //    if (ep.IsValue)
            //    {
            //        epKEy = myWeb[myId][myConnectorKey].AttachEndPoint(ep.Value);
            //        desc = ep.Value.ToString();
            //    }
            //    else
            //    {
            //        if (ep.ChainConnectorKey == "")
            //        {
            //            epKEy = myWeb[myId][myConnectorKey].AttachEndPoint(ep.BlockHandle, ep.Service);

            //            foreach (object obj in ep.PredefinedValues)
            //            {
            //                DCRF.Contract.Connector.EndPoint fp = new DCRF.Contract.Connector.EndPoint(myWeb);
            //                fp.Value = obj;

            //                myWeb[myId][myConnectorKey].AddFixedArg(epKEy, fp);
            //            }

            //            desc = ep.BlockHandle.ToString() + " : " + ep.Service;
            //        }
            //        else
            //        {
            //            epKEy = myWeb[myId][myConnectorKey].AttachConnectorEndPoint(ep.BlockHandle, ep.ChainConnectorKey);

            //            if (ep.BlockHandle != "")
            //            {
            //                desc = "Connector: " + ep.ChainConnectorKey + " @ " + ep.BlockHandle.ToString();
            //            }
            //            else
            //            {
            //                desc = "GlobalConnector: " + ep.ChainConnectorKey;
            //            }
            //        }
            //    }

            //    children.Add(new EndPointNode(myWeb, myId, myConnectorKey, desc, epKEy));

            //    //refresh but do not re-connect and re-read endpoints, just update UI
            //    refresh(myNode, false);
            //}
        }
    }
}

