using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Helper;
using DCRF.Primitive;
using System.Threading;
using System.Collections;
using DCRF.Interface;
using DCRF.DBC;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using DCRF.Definition;
using DCRF.Attributes;
using NetSockets.Peer;
using DCRF.Contract;
using DCRF.Proxy;
using System.Xml;

namespace DCRF.Core
{
    public class BlockWeb : IContainerBlockWeb
    {
        #region private fields

        private static Random random = new Random();  //used to create random id for web
        private Dictionary<string, IContainedBlock> innerBlocks = new Dictionary<string, IContainedBlock>();

        /// <summary>
        /// BlockWeb does not load/dispose Blocks. It's brokers job.
        /// </summary>
        private IBlockBroker blockBroker = null;
        private IContainerBlock containerBlock = null;  //the block that has hosted this web, we refer to this if cannot find something

        /// <summary>
        /// A unique ID to be used as PeerID when connecting multiple BlockWebs to each-other
        /// </summary>
        private string myId = null;
        private PlatformType platform = PlatformType.Neutral;

        //private Dictionary<string, Connector> globalConnectors = new Dictionary<string, Connector>();
        private Dictionary<string, IBlockWeb> peers = new Dictionary<string, IBlockWeb>();
      
        #endregion

        #region Public Properties

        public string Id
        {
            get
            {
                return myId;
            }
        }

        public int BlockCount
        {
            get
            {

                return innerBlocks.Count;
            }
        }

        public string Address
        {
            get
            {
                if (peerManager == null) return "";
                return peerManager.Address;
            }
        }

        public string Host
        {
            get
            {
                if (peerManager == null) return "";
                return peerManager.Host;
            }
        }

        public int Port
        {
            get
            {
                if (peerManager == null) return -1;
                return peerManager.Port;
            }
        }

        public IList<string> BlockIds
        {
            get
            {
                return new List<string>((IEnumerable<string>)innerBlocks.Keys);
            }
        }

        public object GetBlockWebMetaInfo(BlockWebMetaInfoType type, string itemName)
        {
            switch (type)
            {
                //case BlockWebMetaInfoType.GlobalConnectorKeys: return new List<string>((IEnumerable<string>)globalConnectors.Keys);
                //case BlockWebMetaInfoType.GlobalConnectorEndpoints: return globalConnectors[itemName].GetEndPointsDescription();
                case BlockWebMetaInfoType.PeersInfo:
                    {
                        Dictionary<string, string> result = new Dictionary<string, string>();

                        foreach (string item in peers.Keys)
                        {
                            result.Add(item, peers[item].Address);
                        }

                        return result;
                    }
                case BlockWebMetaInfoType.Platform: return platform;
            }

            return null;
        }

        #endregion

        #region Constructor

        private void construct(string id, BlockWeb containerWeb, IBlockBroker broker, PlatformType platform, 
            IContainerBlock cont, bool isRemotable)
        {
            this.platform = platform;

            if (containerWeb != null)
            {
                blockBroker = containerWeb.blockBroker;
            }
            else if (broker != null)
            {
                blockBroker = broker;
            }

            if (id != null)
            {
                this.myId = id;
            }
            else
            {
                this.myId = random.Next().ToString();
            }

            containerBlock = cont;

            if ( isRemotable ) startServer();
        }

        public BlockWeb(string id = null, IBlockBroker broker = null, BlockWeb containerWeb = null,  
            PlatformType platform = PlatformType.Neutral, IContainerBlock containerBlock = null, bool isRemotable = false)
        {
            construct(id, containerWeb, broker, platform, containerBlock, isRemotable);
        }


        #endregion

        #region Block Add/Delete

        /// <summary>
        /// Loads a block and adds it to the runtime blocks. CID is block definition and result string is an identifier
        /// to point to the runtime instance of the block.
        /// </summary>
        /// <param name="id">contains basic info (id, ver, prod) used to load the Block</param>
        /// input handle just</param>
        /// <returns>A handle to newly instantiated block</returns>
        /// 
        public virtual string AddBlock(BlockHandle handle, string identifier=null)
        {
            IContainedBlock comp = null;

            if (identifier == null)
            {
                identifier = Guid.NewGuid().ToString();
            }
            
            BlockWebSysEventArgs eventArgs = new BlockWebSysEventArgs(this, handle);

            SysEventHelper.FireSysEvent(this, SysEventTiming.Before, SysEventCode.LoadBlock, identifier, eventArgs);

            if (blockBroker != null)
            {
                comp = blockBroker.LoadBlock(handle, identifier, this);
            }

            logEvent(LogType.AddBlock, "Load done - Block: " + handle.ToString() + " with ID = " + identifier);
            SysEventHelper.FireSysEvent(this, SysEventTiming.After, SysEventCode.LoadBlock, identifier, eventArgs);

            bool doneCall = SysEventHelper.FireSysEvent(this, SysEventTiming.InsteadOf, SysEventCode.AddBlock, identifier, eventArgs);
            if (doneCall)
            {
                return identifier;
            }

            logEvent(LogType.AddBlock, "Adding - Block: " + handle.ToString() + " with ID = " + identifier);

            SysEventHelper.FireSysEvent(this, SysEventTiming.Before, SysEventCode.AddBlock, identifier, eventArgs);

            if (comp == null)
            {
                throw new System.Exception("Could not load the given handle: "+handle.ToString());
            }

            //TODO: check platform from attributes of the block
            initBlock(comp);

            logEvent(LogType.AddBlock, "Init Done - Block: " + handle.ToString() + " with ID = " + identifier);

            eventArgs.BlockId = identifier;
            SysEventHelper.FireSysEvent(this, SysEventTiming.After, SysEventCode.AddBlock, identifier, eventArgs);

            return identifier;
        }

        /// <summary>
        /// here we initialize the newly created Block and route it throught the default event pipeline
        /// </summary>
        /// <param name="comp"></param>
        protected virtual void initBlock(IContainedBlock comp)
        {
            Check.Ensure(comp != null);
            Check.Ensure(!innerBlocks.ContainsKey(comp.Id), "Block id cannot be the same as any of existing blocks: " + comp.Id);

            innerBlocks.Add(comp.Id, comp);

            try
            {
                //perform initialization steps
                comp.InitBlock();
                comp.InitConnectors();
            }
            catch (System.Exception exc)
            {
                bool throwException = true;

                //if (globalConnectors.ContainsKey(SysEventCode.ExceptionOccured))
                //{
                //    try
                //    {
                //        logEvent(LogType.Exception, exc.Message);
                //    }
                //    catch(Exception e)
                //    {
                //        try
                //        {
                //            logEvent(LogType.Exception, "(DoubleException) " + e.Message);
                //        }
                //        catch (Exception e2)
                //        {
                //            throw e2;
                //        }
                //    }
                //}

                if (throwException) throw exc;
            }
        }

        public virtual void DeleteBlock(string id)
        {
            logEvent(LogType.DeleteBlock, "DeleteBlock Begin - Handle: " + id.ToString());

            IContainedBlock comp = innerBlocks[id];

            BlockWebSysEventArgs eventArgs = new BlockWebSysEventArgs(this, id);
            bool hasCall = SysEventHelper.FireSysEvent(this, SysEventTiming.InsteadOf, SysEventCode.DeleteBlock, id, eventArgs);
            if (hasCall) return;

            SysEventHelper.FireSysEvent(this, SysEventTiming.Before, SysEventCode.DeleteBlock, id, eventArgs);

            comp.Dispose();
            innerBlocks.Remove(id);
            blockBroker.DisposeBlock(comp);

            SysEventHelper.FireSysEvent(this, SysEventTiming.After, SysEventCode.DeleteBlock, id, eventArgs);

            logEvent(LogType.DeleteBlock, "DeleteBlock Done - Handle: " + id.ToString());
        }

        #endregion

        #region Block Search

        //public List<string> FindBlocks(BlockHandle handle)
        //{
        //    List<string> result = new List<string>();

        //    //TODO: rewrite according to new attributes
        //    //foreach (Guid key in innerBlocks.Keys)
        //    //{
        //    //    if (innerBlocks[key].BlockInfo.BlockID.Equals(handle))
        //    //    {
        //    //        result.Add(key);
        //    //    }
        //    //}

        //    return result;
        //}

        public IBlock this[string id]
        {
            get
            {
                IBlock result = this[id, 1];

                if (result == null)
                {
                    result = this[id, -1];
                }

                return result;
            }
        }
       
        /// <summary>
        /// We send out IBlocks to the outer world.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IBlock this[string id, int direction]
        {
            get
            {
                //1st priority: check internal blocks
                if (innerBlocks.ContainsKey(id))
                {
                    return innerBlocks[id];
                }

                //direction = 1 means look for upper levels 
                if (direction == 1)
                {
                    //2nd priority: check container's web if it has a block with this handle
                    if (containerBlock != null)
                    {
                        IBlock externalBlock = containerBlock.GetParentWebBlock(id);

                        if (externalBlock != null)
                        {
                            return externalBlock;
                        }
                    }
                }

                if (direction == -1)
                {
                    //3rd priority: chceck descendants' hierarchy
                    foreach (IContainedBlock block  in innerBlocks.Values)
                    {
                        IBlock result = block.GetInnerWebBlock(id);

                        if (result != null) return result;
                    }
                }

                //4th priority: check peers
                foreach (string peerId in peers.Keys)
                {
                    if (peers[peerId].BlockIds.Contains(id))
                    {
                        return peers[peerId][id];
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// comma separated list of tags - returns list of blocks that have ALL of these tags
        /// </summary>
        /// <param name="lookupTags"></param>
        /// <returns></returns>
        //public List<string> FindBlocks(string lookupTag)
        //{
        //    List<string> result = new List<string>();

        //    foreach (IContainedBlock block in innerBlocks.Values)
        //    {
        //        //TODO: update according to new attributes
        //        //if (block.BlockInfo.Tags.Contains(lookupTag))
        //        //{
        //        //    result.Add(block.Id);
        //        //}
        //    }

        //    //EW lookup


        //    return result;
        //}

        public void ReloadBlocks()
        {
            //first clear broker cache
            blockBroker.ClearCache();

            string[] blockIds = new string[innerBlocks.Count];
            innerBlocks.Keys.CopyTo(blockIds, 0);

            //reload all blocks
            foreach (string id in blockIds)
            {
                BlockHandle cid = getBlockId(id);
                IContainedBlock currentBlock = innerBlocks[id];
                bool cancelOperation = false;

                object blockState = currentBlock.OnBeforeReload(ref cancelOperation);

                if (!cancelOperation)
                {
                    currentBlock.Dispose();
                    currentBlock = null;

                    IContainedBlock newBlock = blockBroker.LoadBlock(cid, id, this);
                    newBlock.OnAfterReload(blockState);

                    innerBlocks[id] = newBlock;
                }
            }

        }

        #endregion

        #region Distribution Support
        private PeerManager peerManager = null;

        private void startServer()
        {
            peerManager = new PeerManager(this.myId, new PeerLogDelegate(intLogEvent));

            peerManager.SetHandler(MsgCode.CallBlockWebMethod, new MessageHandlerDelegate(onCallWebMethodRequest));
            peerManager.SetHandler(MsgCode.CallBlockMethod, new MessageHandlerDelegate(onCallBlockMethodRequest));

            peerManager.PeerDisconnected += new PeerConnectDelegate(peerManager_PeerDisconnected);
            peerManager.PeerConnected += new PeerConnectDelegate(peerManager_PeerDisconnected);
        }

        void peerManager_PeerDisconnected(string peerId, bool isConnected, string peerHost, int peerPort)
        {
            logEvent(LogType.PeerConnection, "Peer " + peerId + " " + (isConnected ? "connected" : "disconnected"));

            BlockWebPeerSysEventArgs eventArgs = new BlockWebPeerSysEventArgs(this, peerId, peerHost, peerPort, isConnected);

            if (isConnected == false)
            {
                if (peers.ContainsKey(peerId)) peers.Remove(peerId);

                SysEventHelper.FireSysEvent(this, SysEventTiming.After, SysEventCode.PeerDisconnect, Id, eventArgs);
            }
            else
            {
                SysEventHelper.FireSysEvent(this, SysEventTiming.Before, SysEventCode.PeerConnect, null, eventArgs); 

                if (!peers.ContainsKey(peerId))
                {
                    peers.Add(peerId, new ProxyBlockWeb(peerId, peerHost, peerPort, peerManager));

                    SysEventHelper.FireSysEvent(this, SysEventTiming.After, SysEventCode.PeerConnect, null, eventArgs); 
                }
            }
        }

        public bool Connect(string host, int port, string peerId)
        {
            if (!peerManager.HasPeer(peerId))
            {
                logEvent(LogType.PeerConnection, "Connecting to " + peerId + " at " + host + ":"+port.ToString());

                bool connected = peerManager.Connect(host, port, peerId);

                if (connected)
                {
                    peers.Add(peerId, new ProxyBlockWeb(peerId, host, port, peerManager));
                }

                return connected;
            }

            return true;
        }

        public bool Connect(IBlockWeb web, string peerId)
        {
            if (web.Id != peerId) return false;

            peers.Add(peerId, web);

            //also inform other peer
            web.Connect(this, Id);

            return true;
        }

        public IBlockWeb GetPeer(string peerId)
        {
            return peers[peerId];
        }

        public void Disconnect(string peerId)
        {
            logEvent(LogType.PeerConnection, "Disconnecting from " + peerId);

            peerManager.Disconnect(peerId);
            peers.Remove(peerId);
        }

        public BlockHandle GetBlockHandle(string id)
        {
            return getBlockId(id);
        }

        //TODO: there is a copy of this in simpleBlockBroker and runtime, merge them in a single place
        private BlockHandle getBlockId(string id)
        {
            IContainedBlock block = innerBlocks[id];
            object[] result = block.GetType().GetCustomAttributes(typeof(BlockHandleAttribute), true);

            if (result.Length == 1)
            {
                return (result[0] as BlockHandleAttribute).BlockId;
            }

            throw new System.Exception("BlockId has problem");
        }

        public bool MigrateBlock(string id, string peerId)
        {
            logEvent(LogType.MigrateBlock, "MigrateBlock Begin - ID = "+id.ToString()+ " to "+peerId);

            bool cancelOperation = false;
            IContainedBlock comp = innerBlocks[id];
            BlockHandle blockId = getBlockId(id);
            object customData = comp.OnBeforeMigration(ref cancelOperation);

            //the block does not support migration
            if (cancelOperation) return false;

            //we do not send startupArgs when migrating.We suppose that it is reflected in the properties of the block
            List<object> response = peerManager.SendMessage(peerId, MsgCode.CallBlockWebMethod, "MigrateBlock", id, blockId, customData);

            if (response == null || response.Count ==0) return false;

            //if response contains id of this block, means success
            if (response[0].Equals(id))
            {
                this.DeleteBlock(id);

                //just to make sure block is deleted (in DeleteBlock onBeforeDelte can cancel oepration)
                if ( innerBlocks.ContainsKey(id) )
                {
                    innerBlocks.Remove(id);
                }

                //blockAccessors.Remove(guid);
                logEvent(LogType.MigrateBlock, "MigrateBlock Done OK - ID = " + id.ToString() + " to " + peerId);

                return true;
            }

            logEvent(LogType.MigrateBlock, "MigrateBlock Done Fail - ID = " + id.ToString() + " to " + peerId);

            return false;
        }


        private void onCallBlockMethodRequest(PeerSocket sender, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {            
            object result = null;
            string blockId = (string)data[0];
            string methodName = data[1].ToString();
            string serviceName = "";
            //object[] args = (object[])data[2];

            BlockHandle info = getBlockId(blockId);

            logEvent(LogType.PeerRequest, "Peer " + senderId + " sent request: " + msgCode);
            logEvent(LogType.ProcessRequest, "onCallBlockMethodRequest - Name = " + methodName + " ID=" + info.ToString());

            try
            {
                //others are arguments
                switch (methodName)
                {
                    case "ProcessRequest":
                        {
                            serviceName = data[2].ToString();
                            object[] args = (object[])data[3];

                            logEvent(LogType.ProcessRequest, "onCallBlockMethodRequest - Name = " + methodName + " ID=" + info.ToString() + " service=" + serviceName);

                            result = this[blockId].ProcessRequest(serviceName, args);

                            break;
                        }
                }
            }
            catch (System.Exception exc)
            {
                logEvent(LogType.Exception,"Error Handling Call on Block Service (" + methodName + ","+serviceName+"): " + exc.Message);
                throw;
            }

            sender.SendMessageAsync(msgCode, msgId, result);
        }

        private void onCallWebMethodRequest(PeerSocket sender, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {            
            object result = null;
            string methodName = data[0].ToString();

            logEvent(LogType.PeerRequest, "Peer " + senderId + " sent request: " + msgCode);
            logEvent(LogType.BlockWeb, "onCallMethodRequest - Name = " + methodName);

            try
            {
                //others are arguments
                switch (methodName)
                {
                    case "AddBlock": result = AddBlock((BlockHandle)data[1], (string)data[2]); break;
                    case "GetBlockId": result = getBlockId((string)data[1]); break;
                    case "DeleteBlock": DeleteBlock((string)data[1]); break;
                    case "ReloadBlocks": ReloadBlocks(); break;
                    case "Disconnect": Disconnect((string)data[1]); break;
                    case "Dispose": Dispose(); return;
                    case "GetBlockWebMetaInfo": result = GetBlockWebMetaInfo((BlockWebMetaInfoType)data[1], (string)data[2]); break;
                    case "Connect": result = Connect((string)data[1], (int)data[2], (string)data[3]); break;
                    case "BlockHandles": result = BlockIds; break;
                    case "BlockCount": result = BlockCount; break;
                    case "MigrateBlock":
                        {
                            //create given block and send migrate-done 
                            string id = (string)data[0];
                            BlockHandle blockId = (BlockHandle)data[1];
                            object customData = data[2];

                            string response = AddBlock(blockId);
                            innerBlocks[response].OnAfterMigration(customData);

                            result = response;
                            break;
                        }
                }
            }
            catch (System.Exception exc)
            {
                logEvent(LogType.Exception, "Error Handling Call on Web Methods (" + methodName + "): " + exc.Message);
                throw;
            }

            sender.SendMessageAsync(msgCode, msgId, result);
        }

        #endregion

        #region IDisposable Members

        public virtual void Dispose()
        {
            if (peerManager != null)
            {
                peerManager.Disconnect();
                peerManager.Dispose();
                peerManager = null;
            }

            //also dispose all blocks
            foreach (IContainerBlock block in innerBlocks.Values)
            {
                block.Dispose();
            }
        }

        #endregion

        #region private methods

        private void intLogEvent(int type, string log)
        {
            logEvent((LogType)type, log);
        }

        private void logEvent(LogType type, string log)
        {
            //if (globalConnectors.ContainsKey(SysEventCode.LogWebEvent))
            //{
            //    GetConnector(SysEventCode.LogWebEvent).ProcessRequest(type, log);
            //}
        }
        #endregion
        
    }
}
