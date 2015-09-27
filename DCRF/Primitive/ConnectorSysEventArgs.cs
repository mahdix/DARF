using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract;

namespace DCRF.Primitive
{
    /// <summary>
    /// Contains structure of arguments that will be passed to connector related sysevents (for example endpointattached).
    /// Some fields will have value only in the case of appropriate events
    /// EndPoint attach/detach, processrequest
    /// </summary>
    public class ConnectorSysEventArgs
    {
        public ConnectorSysEventArgs (string connectorKey, string endpointKey, string blockId,
            string serviceName, string chainConnector,
            IConnectorSubject subject,object epArg, object[] args, List<object> result)
        {
            ConnectorKey = connectorKey;
            EndPointKey = endpointKey;
            Result = result;
            EndPointBlockId = blockId;
            EndPointSubject = subject;
            EndPointChainConnectorKey = chainConnector;
            EndPointServiceName = serviceName;
            EndPointValue = epArg;
            RequestArgs = args;
        }

        public ConnectorSysEventArgs(string connectorKey, object[] args, List<object> result)
            : this(connectorKey, null,null,null,null,null,null, args, result)
        {
        }

        public ConnectorSysEventArgs InnerSysArgs
        {
            get
            {
                if (RequestArgs != null && RequestArgs.Length > 0 && (RequestArgs[0] is ConnectorSysEventArgs))
                {
                    return (RequestArgs[0] as ConnectorSysEventArgs);
                }

                return null;
            }
        }

        //public ConnectorSysEventArgs(string connectorKey, string endpointKey, Guid blockId, string chainConnector)
        //    : this(connectorKey, endpointKey, blockId, null, chainConnector, null, null, null, null) 
        //{
        //}

        //public ConnectorSysEventArgs(string connectorKey, string endpointKey, Guid blockId,
        //    string serviceName)
        //    : this(connectorKey, endpointKey, blockId, serviceName, null, null, null, null, null)
        //{
        //}

        public ConnectorSysEventArgs(string connectorKey, string endpointKey, 
            IConnectorSubject subject, string serviceName)
            : this(connectorKey, endpointKey, null, serviceName, null, subject, null, null, null)
        {
        }

        public ConnectorSysEventArgs(string connectorKey, string endpointKey, object[] requestArgs)
            : this(connectorKey, endpointKey, null, null, null, null, null, requestArgs, null)
        {
        }

        public ConnectorSysEventArgs(string connectorKey, string endpointKey, object endpointValue)
            : this(connectorKey, endpointKey, null, null, null, null, endpointValue, null, null)
        {
        }

        public ConnectorSysEventArgs(string connectorKey, string endpointKey)
            : this(connectorKey, endpointKey, null, null, null, null, null, null, null)
        {
        }
        
        public string ConnectorKey = null;
        public string EndPointKey = null;
        public string EndPointBlockId = null;
        public string EndPointServiceName = null;
        public string EndPointChainConnectorKey = null;
        public object EndPointValue = null;
        public IConnectorSubject EndPointSubject = null;
        public List<object> Result = null;
        public object[] RequestArgs = null;


        /// <summary>
        /// if set to true, operation is cancelled after calling remaining sysevent handlers is finished
        /// </summary>
        public bool CancelOperation = false;

        /// <summary>
        /// If set to true will cancel operation at the moment without continuing calling remaining handlers
        /// </summary>
        //public bool FastCancelOperation = false;

    }
}
