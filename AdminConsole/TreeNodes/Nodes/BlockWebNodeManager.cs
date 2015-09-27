//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Windows.Forms;
//using DCRF.Core;
//using AdminConsole.TreeNodes.Nodes.Single;
//using System.Threading;

//namespace AdminConsole.TreeNodes.Nodes
//{
//    public delegate void ManagerUnloadDelegate(BlockWebNodeManager sender, TreeNode node);

//    public class BlockWebNodeManager
//    {
//        private ProxyBlockWeb innerWeb = null;
//        private BlockWebNode myWebNode = null;

//        public event ManagerUnloadDelegate Unload;
//        public bool IsConnected = false;
//        public string ErrorMessage = "";

//        public BlockWebNodeManager(string id, string host, int port)
//        {
//            try
//            {
//                innerWeb = new ProxyBlockWeb("AdminConsole ("+r.Next(1000).ToString()+")", id, host, port);
                
//                Thread.Sleep(1000);

//                myWebNode = new BlockWebNode(innerWeb);
//                myWebNode.Unload += new UnloadDelegate(myWebNode_Unload);
//                innerWeb.PeerDisconnected += new NetSockets.Peer.PeerConnectDelegate(innerWeb_PeerDisconnected);
//                myWebNode.id = id;
//                myWebNode.host = host;
//                myWebNode.port = port;

//                IsConnected = true;
//            }
//            catch (Exception exc)
//            {
//                IsConnected = false;
//                innerWeb = null;
//                ErrorMessage = exc.Message;
//            }
//        }

//        void innerWeb_PeerDisconnected(string peerId, bool isConnected)
//        {
//            MessageBox.Show("Peer \"" + peerId + "\" disconnected.", "Disconnect");

//            myWebNode.ExecuteCommand("UNLOAD", null);
//        }

//        void myWebNode_Unload(BlockWebNode sender, TreeNode node)
//        {
//            if (Unload != null)
//            {
//                Unload(this, node);
//            }
//        }

//        public void Attach(TreeNode myNode)
//        {
//            myWebNode.Attach(myNode);
//        }
//    }
//}
