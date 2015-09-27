using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using DCRF.Primitive;
using DCRF.Helper;
using DCRF.Interface;
using DCRF.Definition;
using DCRF.Attributes;
using DCRF.Contract;
using System.Xml;

namespace DCRF.Core
{
    public abstract class BlockBase : IBlock, IContainerBlock, IContainedBlock
    {
        #region Private fields
        public static bool MakeInnerWebsRemotable = false;

        private bool enableLog = false;

        protected IContainerBlockWeb blockWeb = null;
        /// <summary>
        /// Key is a string and value is a Connector object.
        /// This is protected so that actual blocks implementations will be able to 
        /// Create and initialize their connectors (properties, events, references, ...)
        /// </summary>
        protected Dictionary<string, Connector> internalConnectors = new Dictionary<string, Connector>();

        private List<string> serviceCache = null;
        private Dictionary<string, MethodBase> serviceMethodCache = null;

        /// <summary>
        /// If a block is composed of several internal blocks, this innerWeb is used to host
        /// child blocks.
        /// </summary>
        protected BlockWeb innerWeb = null;

        /// <summary>
        /// The unique id assigned to this block in the web when loading
        /// </summary>
        protected string myId = null;

        #endregion

        #region Constructor

        public BlockBase(string id, IContainerBlockWeb parent)
        {
            myId = id;
            blockWeb = parent;
        }

        #endregion

        #region Properties

        public string Id
        {
            get
            {
                return myId;
            }
        }

        #endregion

        #region Initialization and dispose methods
        /// <summary>
        /// Initializes Block - this method is called after component is being BlockWebd.
        /// </summary>
        public virtual void InitBlock()
        {
            innerWeb = new BlockWeb(null, null, blockWeb as BlockWeb, PlatformType.Neutral, this, MakeInnerWebsRemotable);
        }

        public virtual void InitConnectors()
        {
        }

        public IBlockWeb ContainerWeb 
        {
            get
            {
                return blockWeb;
            }
        }


        public virtual void Dispose()
        {
            //dispose innerweb if not null
            if (innerWeb != null)
            {
                innerWeb.Dispose();
            }
        }

        #endregion

        #region Protected helper methods and properties

        protected void createConnectors(params string[] keys)
        {
            foreach (string key in keys)
            {
                internalConnectors.Add(key, new Connector(key, this));
            }
        }


        /// <summary>
        /// This provides an interface to outer world to attach and detach their endpoints to my connectors.
        /// So I can invoke them when required.
        /// We return IConnector. This decision was made to let connectors call events of a block for example
        /// when connector wants to invoke "EndPointAttached" event, we want to be able to call handlers in block 
        /// in addition to blockweb.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IConnector this[string key]
        {
            get
            {
                logEvent(LogType.Connector, "Get Connector: "+key+" on " + Id.ToString());

                if (!internalConnectors.ContainsKey(key))
                {
                    //we return null in case that no connector exists. this makes logic more clean and predictable.
                    //in case some other piece of code wants to create a connector in this block,
                    //we can provide a block service for that task
                    return null;
                }

                return internalConnectors[key];
            }
        }

        //public IConnector this[string key, string subKey]
        //{
        //    get
        //    {
        //        return this[key + "." + subKey];
        //    }
        //}

        public IBlock GetInnerWebBlock(string handle)
        {
            if (innerWeb != null)
            {
                //only look into descendant items
                return innerWeb[handle, -1];
            }

            return null;
        }
        #endregion

        #region meta services for management applications

        [BlockService]
        public object ProcessMetaInfo(BlockMetaInfoType type, string itemNam, object[] args)
        {
            switch (type)
            {
                //TODO: remove one line methods which are used here and move the code into this method
                case BlockMetaInfoType.ConnectorEndpoint: return internalConnectors[itemNam].GetEndPointDescription();
                case BlockMetaInfoType.ConnectorInfo: return DCRFHelper.GetConnectorInfo(this, itemNam);
                case BlockMetaInfoType.ConnectorKeys: return getConnectorKeys();
                case BlockMetaInfoType.InnerWebHost: return getInnerWebHost(); 
                case BlockMetaInfoType.InnerWebId: return getInnerWebId(); 
                case BlockMetaInfoType.InnerWebPort: return getInnerWebPort();
                case BlockMetaInfoType.ServiceInfo: return DCRFHelper.GetMethodInfo(this, itemNam); ; 
                case BlockMetaInfoType.Services: return getServices();
                case BlockMetaInfoType.BlockInfo: return DCRFHelper.GetBlockInfo(this);
                case BlockMetaInfoType.ServiceArgsInfo: return DCRFHelper.GetServiceArgsInfo(this, itemNam);
            }

            return null;
        }

        [BlockService]
        public object ProcessMetaService(BlockMetaServiceType type, string itemName, object[] args)
        {
            switch (type)
            {
                case BlockMetaServiceType.DisableLog: enableLog = false; return "Log Disabled"; 
                case BlockMetaServiceType.EnableLog: enableLog = true; return "Log Enabled";

                //this cannot be called for a proxied block. It is handled in ProxyBlock class
                case BlockMetaServiceType.GetInnerWeb: return innerWeb;
                case BlockMetaServiceType.InvokeConnector: return processLocalConnector(itemName, args);
                case BlockMetaServiceType.CreateConnector: createConnectors(itemName); break;
            }

            return null;
        }

        private List<string> getServices()
        {
            if (serviceCache == null)
            {
                serviceMethodCache = BlockHelper.GetServices(this);
                serviceCache = new List<string>();

                foreach (MethodBase mb in serviceMethodCache.Values)
                {
                    serviceCache.Add(mb.Name);
                }
            }

            return serviceCache;            
        }

        private string getInnerWebHost()
        {
            //innerWeb does not exist or is not remotable
            if (innerWeb == null || innerWeb.Address == "" ) return null;

            return innerWeb.Host;
        }

        private int getInnerWebPort()
        {
            if (innerWeb == null) return -1;

            return innerWeb.Port;
        }

        private string getInnerWebId()
        {
            if (innerWeb == null) return null;

            return innerWeb.Id;
        }

        private ICollection<string> getConnectorKeys()
        {
            //connectors are created in localConnecotrs upon block initialization
            return new List<string>(internalConnectors.Keys);
        }

        private object processLocalConnector(string connectorKey, params object[] args)
        {
            if (internalConnectors.ContainsKey(connectorKey))
            {
                return internalConnectors[connectorKey].ProcessRequest(args);
            }

            return null;
        }

        #endregion

        public IBlock GetParentWebBlock(string handle)
        {
            if (blockWeb != null)
            {
                return blockWeb[handle, 1];
            }

            return null;
        }

        /// <summary>
        /// This method is overriden in a dynamic block
        /// </summary>
        /// <param name="serviceName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public virtual object ProcessRequest(string serviceName, params object[] args)
        {
            try
            {
                object result = null;

                //force refresh service cache
                this.getServices();

                BlockSysEventArgs eventArgs = new BlockSysEventArgs(Id, serviceName, args, result);
                bool hasCall = SysEventHelper.FireSysEvent(blockWeb, SysEventTiming.InsteadOf, SysEventCode.ProcessRequest, this.Id, eventArgs);

                if (hasCall)
                {
                    return eventArgs.Result;
                }                

                if (!serviceCache.Contains(serviceName))
                {
                    throw new Exception("Invalid Service Name: " + Id + "." + serviceName);
                }

                SysEventHelper.FireSysEvent(blockWeb, SysEventTiming.Before, SysEventCode.ProcessRequest, this.Id, eventArgs); 

                result = BlockHelper.ProcessRequest(serviceMethodCache, this, serviceName, args);

                //this call is time consuming so we set a flag for it
                logEvent(LogType.ProcessRequest, "Done Process - " + serviceName + " on " + Id.ToString() + " - Result = " + (result == null ? "(null)" : result.ToString()));

                SysEventHelper.FireSysEvent(blockWeb, SysEventTiming.After, SysEventCode.ProcessRequest, this.Id, eventArgs); 

                return result;
            }
            catch (Exception exc)
            {
                logEvent(LogType.Exception, "ProcessRequest Exception for "+serviceName+" : "+exc.Message);
                throw;
            }
        }

        public virtual object OnBeforeMigration(ref bool cancelOperation)
        {
            return null;
        }

        public virtual void OnAfterMigration(object state)
        {
        }

        public object OnBeforeReload(ref bool cancelOperation)
        {
            return null;
        }

        public void OnAfterReload(object state)
        {
        }

        private void logEvent(LogType type, string log)
        {
            if (enableLog)
            {
                //blockWeb.GetConnector(SysEventCode.LogBlockEvent).ProcessRequest(type, log);
            }
        }
    }
}
