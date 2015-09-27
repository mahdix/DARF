using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;
using DCRF.Definition;
using DCRF.Primitive;
using DCRF.Helper;

namespace DCRF.Contract
{
    //public delegate void EndPointDelegate(string endpointKey, string connectorKey);

    /// <summary>
    /// This class encapsulates a dependency requirement for a block (Service, Interface or Property). For example
    /// if a block requires existence of a special service (provided by any block), it creates
    /// an instance of this class as one of its connectors, and mentions the name of the 
    /// service required.
    /// </summary>
    public class Connector : IConnector
    {
        #region private fields
        //endpoints require a method to call their block/service. 
        private string connectorKey = null;
        private IBlock containerBlock = null;

        //key is a random string that only the attacher of the endpoint knows. so only he is able to detach his endpoint
        private object epValue = null;
        private string epBlockId = null;
        private string epServiceName = null;
        private string epConnectorKey = null;
        #endregion

        #region constructor

        /// <summary>
        /// wcont: is the container web
        /// For a globalConnector this is the BlockWeb containing this connector
        /// For a Block-level connector this will be BlockWeb of the container Block.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cont"></param>
        /// <param name="wcont"></param>
        public Connector(string key, IBlock bcont)
        {
            connectorKey = key;
            containerBlock = bcont;
        }

        #endregion

        #region Process

        /// <summary>
        /// This result list contains result of call on endpoints
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public object ProcessRequest(params object[] args)
        {
            object result = null;

            ConnectorSysEventArgs eventArgs = new ConnectorSysEventArgs(connectorKey, null, args);
            bool hasCall = SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.InsteadOf, 
                SysEventCode.ConnectorProcessRequest, connectorKey, eventArgs);

            if (hasCall)
            {
                return result;
            }

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.Before, SysEventCode.ConnectorProcessRequest,
                 connectorKey, eventArgs);

            if (epServiceName != null)
            {
                result = containerBlock.ContainerWeb[epBlockId].ProcessRequest(epServiceName, args);
            }
            else if (epConnectorKey != null)
            {
                result = containerBlock.ContainerWeb[epBlockId][epConnectorKey].ProcessRequest(args);
            }
            else
            {
                result = epValue;
            }

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, 
                SysEventCode.ConnectorProcessRequest, SysEventTiming.After, connectorKey, eventArgs);

            return result;
        }

        #endregion

        private void logEvent(string log)
        {
            //checkAndCallConnector(SysEventCode.LogWebEvent, LogType.Connector, log);
        }

        #region Attach/Detach

        public bool AttachConnectorEndPoint(string blockId, string chainConnectorKey)
        {
            logEvent("Attaching chain-endpoint for " + connectorKey + " - " + blockId.ToString() + " " + chainConnectorKey);

            //string key = createRandomKey();

            ConnectorSysEventArgs eventArgs = new ConnectorSysEventArgs(connectorKey, null, blockId);
            eventArgs.EndPointChainConnectorKey = chainConnectorKey;

            bool hasCall = SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.InsteadOf, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            if (hasCall)
            {
                return true;
            }

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.Before, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            epValue = null;
            epBlockId = blockId;
            epConnectorKey = chainConnectorKey;

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.After, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            return true;
        }

        /// <summary>
        /// used to service connectors
        /// </summary>
        /// <param name="connectorKey"></param>
        /// <param name="blockId"></param>
        /// <param name="serviceName"></param>
        public bool AttachEndPoint(string blockId, string serviceName)
        {
            logEvent("Attaching endpoint for " + connectorKey + " - " + blockId + " " + serviceName);

            //string key = createRandomKey();

            ConnectorSysEventArgs eventArgs = new ConnectorSysEventArgs(connectorKey, null);
            eventArgs.EndPointServiceName = serviceName;
            eventArgs.EndPointBlockId = blockId;

            bool hasCall = SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.InsteadOf, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            if (hasCall)
            {
                return true;
            }

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.Before, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            epValue = null;
            epBlockId = blockId;
            epServiceName = serviceName;

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.After, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            return true;
        }

        public bool AttachEndPoint(object value)
        {
            logEvent("Attaching endpoint for " + connectorKey + " - " + (value == null ? "null":value.ToString()));

            //string key = createRandomKey();

            ConnectorSysEventArgs eventArgs = new ConnectorSysEventArgs(connectorKey, null, value);
            bool hasCall = SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.InsteadOf, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            if (hasCall)
            {
                return true;
            }

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.Before, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            epValue = value;
            epServiceName = null;
            epConnectorKey = null;
            epBlockId = null;

            SysEventHelper.FireSysEvent(containerBlock.ContainerWeb, SysEventTiming.After, SysEventCode.AttachEndPoint, 
                connectorKey, eventArgs);

            return true;
        }

        #endregion

        #region Public properties

        public object Value
        {
            get
            {
                return ProcessRequest();
            }
        }

        #endregion

        public bool HasEndPoint(string blockId, string serviceName)
        {
            if (epBlockId == blockId && epServiceName == serviceName)
            {
                return true;
            }

            return false;
            
        }

        public T GetValue<T>(params object[] args)
        {
            object result = ProcessRequest(args);

            if (result == null) return default(T);

            if (typeof(T).IsEnum)
            {
                return (T)Enum.Parse(typeof(T), result.ToString());
            }
            else
            {
                return (T)Convert.ChangeType(result, typeof(T));
            } 
        }

        internal object GetEndPointDescription()
        {
            if (epServiceName != null)
            {
                return "&"+epBlockId+"."+epServiceName;
            }
            else if (epConnectorKey != null)
            {
                return "!"+epBlockId + "." + epConnectorKey;
            }
            else
            {
                if (epValue == null)
                {
                    return "(null)";
                }

                return epValue.ToString();
            }
        }
    }

    
}
