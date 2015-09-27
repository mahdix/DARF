using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;
using DCRF.Definition;
using DCRF.Primitive;
using NetSockets.Peer;
using DCRF.Contract;

namespace DCRF.Proxy
{
    /// <summary>
    /// This is a special BlockWeb that is actually a proxy on a remote BlockWeb.
    /// It has nothing inside itself, all requests are sent to the remote web.
    /// </summary>
    public class ProxyBlockWeb : IBlockWeb
    {
        private string peerAddress = null;
        private PeerManager peerManager = null;
        private string peerId = "";
        private Dictionary<string, ProxyConnector> globalConnectors = new Dictionary<string, ProxyConnector>();
        private Dictionary<string, ProxyBlock> blocks = new Dictionary<string, ProxyBlock>();

        public event PeerConnectDelegate Disconnected;

        public ProxyBlockWeb(string _peerId, string peerHost, int peerPort, PeerManager pm)
        {
            peerAddress = string.Format("{0}:{1}", peerHost, peerPort);
            peerId = _peerId;

            peerManager = pm;

            peerManager.PeerDisconnected += new PeerConnectDelegate(peerManager_PeerDisconnected);
        }

        void peerManager_PeerDisconnected(string peerId, bool isConnected, string host, int port)
        {
            if (peerId == this.peerId && isConnected == false && host != null)
            {
                Disconnected(peerId, false, host, port);
            }
        }

        public PeerManager PeerManager
        {
            get
            {
                return peerManager;
            }
        }

        public IList<string> BlockIds
        {
            get
            {
                List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "BlockHandles");

                if (result == null) return null;

                return (result[0] as IList<string>);
            }
        }

        public BlockHandle GetBlockHandle(string id)
        {
            List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "GetBlockId", id);

            if (result == null) return null;

            return (result[0] as BlockHandle);
        }

        public object GetBlockWebMetaInfo(BlockWebMetaInfoType type, string itemName)
        {
            List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "GetBlockWebMetaInfo", type, itemName);

            if (result == null) return null;

            return (result[0]);
        }

        public bool Connect(string targetHost, int targetPort, string targetPeerId)
        {
            List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "Connect", targetHost, targetPort, targetPeerId);

            if (result == null) return false;

            return (bool)result[0];
        }

        public int BlockCount
        {
            get
            {
                List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "BlockCount");

                if (result == null) return -1;

                return ((int)result[0]);
            }
        }

        public string AddBlock(BlockHandle handle, string identifier)
        {
            List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "AddBlock", handle, identifier);

            if (result == null) return null;

            return (string)result[0];
        }

        public void DeleteBlock(string id)
        {
            peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "DeleteBlock", id);
        }

        public void Disconnect(string targetPeerId)
        {
            peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "Disconnect", targetPeerId);
        }

        public IBlock this[string handle]
        {
            get
            {
                return this[handle, 1];
            }
        }

        public IBlock this[string handle, int direction]
        {
            get
            {
                if (!blocks.ContainsKey(handle))
                {
                    blocks.Add(handle, new ProxyBlock(this, handle));
                }

                return blocks[handle];
            }
        }

        //public void CreateConnector(string key)
        //{
        //    throw new NotImplementedException();
        //}
        
        //public IConnector GetConnector(string key, string subKey)
        //{
        //    return GetConnector(key + "." + subKey);
        //}

        //public IConnector GetConnector(string key)
        //{
        //    if (!globalConnectors.ContainsKey(key))
        //    {
        //        globalConnectors.Add(key, new ProxyConnector(key, this, null));
        //    }

        //    return globalConnectors[key];
        //}

        public string Address
        {
            get
            {
                return peerAddress;
            }
        }

        public string Id
        {
            get
            {
                return peerId;
            }
        }

        public void Dispose()
        {
            //we can not dispose a remote web
            //we just can disconnect
            foreach (ProxyBlock block in blocks.Values)
            {
                block.Dispose();
            }

            peerManager.Disconnect(peerId);
        }

        #region to be implemented later
        public List<string> FindBlocks(Primitive.BlockHandle handle)
        {
            throw new NotImplementedException();
        }

        public List<string> FindBlocks(string tag)
        {
            throw new NotImplementedException();
        }

        public bool MigrateBlock(string id, string peerId)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region helper methods for ProxyBlock

        public object CallBlockProcessRequest(string id, string serviceName, params object[] args)
        {
            List<object> result = peerManager.SendMessage(peerId, MsgCode.CallBlockMethod, id, "ProcessRequest", serviceName, args);

            if (result == null) return null;

            return result[0];
        }

        //public object CallConnectorMethod(string blockId, string connectorKey, string methodName, params object[] args)
        //{
        //    List<object> result = peerManager.SendMessage(peerId, MsgCode.CallConnectorMethod, blockId, connectorKey, methodName, args);

        //    if (result == null) return null;

        //    return result[0];
        //}

        #endregion

        public void ReloadBlocks()
        {
            peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "ReloadBlocks");
        }


        public IBlockWeb GetPeer(string peerId)
        {
            throw new NotImplementedException();
        }
        

        public bool Connect(IBlockWeb web, string peerId)
        {
            throw new NotImplementedException();
        }


        public IBlockBroker Broker
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
