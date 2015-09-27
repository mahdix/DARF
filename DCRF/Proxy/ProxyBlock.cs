using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;
using DCRF.Contract;
using NetSockets.Peer;
using DCRF.Primitive;

namespace DCRF.Proxy
{
    public class ProxyBlock: IBlock
    {
        private ProxyBlockWeb parentWeb = null;
        private string blockId = "";
        private ProxyBlockWeb innerWeb = null;
        private Dictionary<string, ProxyConnector> connectors = new Dictionary<string, ProxyConnector>();

        public ProxyBlock(ProxyBlockWeb parent, string id)
        {
            blockId = id;
            parentWeb = parent;
        }

        public string Id
        {
            get
            {
                return blockId;
            }
        }

        public IConnector this[string key]
        {
            get
            {
                if (!connectors.ContainsKey(key))
                {
                    connectors.Add(key, new ProxyConnector(key, parentWeb, this));
                }

                return connectors[key];
            }
        }

        public IConnector this[string key, string subKey]
        {
            get
            {
                return this[key+"."+subKey];
            }
        }

        public bool HasConnector(string key)
        {
            throw new NotImplementedException();
        }

        public object ProcessRequest(string serviceName, params object[] args)
        {
            if (serviceName == "ProcessMetaService" && (BlockMetaServiceType)args[0] == BlockMetaServiceType.GetInnerWeb)
            {
                //IBlockWeb cannot be serialized and moved through sockets. So we need to handle it here
                initInnerWeb();
                return innerWeb;
            }
            else
            {
                return parentWeb.CallBlockProcessRequest(blockId, serviceName, args);
            }
        }

        public void Dispose()
        {
            if (innerWeb != null)
            {
                innerWeb.Dispose();
            }
        }

        private void initInnerWeb()
        {
            if (innerWeb != null) return;

            string innerWebHost = (string)ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.InnerWebHost, null, null);

            if (innerWebHost == null)
            {
                innerWeb = null;
                return;
            }

            int innerWebPort = (int)ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.InnerWebPort, null, null);
            string innerWebId = (string)ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.InnerWebId, null, null);

            bool connected = parentWeb.PeerManager.Connect(innerWebHost, innerWebPort, innerWebId);

            if ( ! connected )
            {
                innerWeb = null;
                return;
            }

            innerWeb = new ProxyBlockWeb(innerWebId, innerWebHost, innerWebPort, parentWeb.PeerManager);
        }

        public void SetInnerWeb(IBlockWeb web)
        {
            throw new NotImplementedException();
        }


        public IBlockWeb ContainerWeb
        {
            get { throw new NotImplementedException(); }
        }
    }
}