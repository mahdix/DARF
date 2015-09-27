using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Contract;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace DCRF.Dynamic
{
    public class DynamicBlock: BlockBase
    {
        private const string baseBlockId = "__baseBlock__";

        private DBDefinition definition = null;
        private List<string> baseBlockServices = null;

        public DynamicBlock(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        public override void InitBlock()
        {
            base.InitBlock();  //create innerWeb

            if (definition.BaseType != null)
            {
                innerWeb.AddBlock(definition.BaseType, baseBlockId);

                baseBlockServices = innerWeb[baseBlockId].ProcessRequest("ProcessMetaInfo", BlockMetaInfoType.Services, null, null) as List<string>;
            }
        }

        public DynamicBlock(DBDefinition definition, params object[] args)
            : base((string)args[0], args[1] as IContainerBlockWeb)
        {
            createConnectors(definition.Connectors.ToArray());

            //no service is added because they are executed when a request is made
            this.definition = definition;
        }

        public override object ProcessRequest(string serviceName, params object[] args)
        {
            Dictionary<string, DBServiceDefinition> services = definition.Services;

            //maybe base block has this service
            if (baseBlockServices != null && baseBlockServices.Contains(serviceName))
            {
                return innerWeb[baseBlockId].ProcessRequest(serviceName, args);
            }
            else if (services.ContainsKey(serviceName))
            {
                DBServiceDefinition service = services[serviceName];

                Dictionary<string, object> argValues = new Dictionary<string, object>();

                for (int i = 0; i < args.Length; i++)
                {
                    argValues[service.Args[i]] = args[i];
                }

                object result = null;

                foreach (DBSLineDefinition serviceLine in service.Body)
                {
                    if (serviceLine.LineType == DBSLineType.ProcessRequest)
                    {
                        object lineResult = handleProcessRequestServiceLine(serviceLine.Start, argValues);

                        if (serviceLine.IsReturn) result = lineResult;  //continue to process remaining liens if any
                    }
                    else if (serviceLine.LineType == DBSLineType.AttachEndPoint)
                    {
                        handleAttachEndpointServiceLine(serviceLine.Start, serviceLine.CreateConnector, argValues);
                    }
                }

                return result;
            }
            else
            {
                return base.ProcessRequest(serviceName, args);
            }
        }

        private object getVal(Dictionary<string, object> argValues, object key)
        {
            if (key != null && argValues.ContainsKey(key.ToString()))
            {
                return argValues[key.ToString()];
            }

            return key;
        }

        private void handleAttachEndpointServiceLine(DBSLineObjOrCall ooc, bool create, Dictionary<string, object> argValues)
        {
            string blockId = (string)getVal(argValues, ooc.Address[1]);
            string connectorKey = (string)getVal(argValues, ooc.Address[2]);
            if (blockId == null) blockId = Id;

            if (create)
            {
                blockWeb[blockId].ProcessRequest(
                    "ProcessMetaService",
                    BlockMetaServiceType.CreateConnector,
                    connectorKey,
                    null);
            }

            //if (single)
            //{
            //    blockWeb[blockId].ProcessRequest(
            //        "ProcessMetaService",
            //        BlockMetaServiceType.ClearConnector,
            //        connectorKey,
            //        null);
            //}
                       
            IConnector connector = blockWeb[blockId][connectorKey];

            //we have only one item as an endpoint here
            DBSLineObjOrCall endPointDefinition = ooc.Args[0];
            //string endPointKey = null;

            if ( endPointDefinition.Obj != null )
            {
                connector.AttachEndPoint(getVal(argValues, endPointDefinition.Obj));
            }
            else if (endPointDefinition.isConnectorCall)
            {
                string cblockId = (string)getVal(argValues, endPointDefinition.Address[1]);
                string cconnectorKey = (string)getVal(argValues, endPointDefinition.Address[2]);

                if (cblockId == null) cblockId = this.Id;

                connector.AttachConnectorEndPoint(cblockId, cconnectorKey);
            }
            else
            {
                string cblockId = (string)getVal(argValues, endPointDefinition.Address[1]);
                string cServiceName = (string)getVal(argValues, endPointDefinition.Address[2]);

                if (cblockId == null) cblockId = this.Id;
                connector.AttachEndPoint(cblockId, cServiceName);
            }

            //foreach (DBSLineObjOrCall fixedArg in endPointDefinition.Args)
            //{
            //    DCRF.Contract.Connector.EndPoint ep = createEndPoint(fixedArg, argValues);

            //    connector.AddFixedArg(endPointKey, ep);
            //}
        }

        //private Connector.EndPoint createEndPoint(DBSLineObjOrCall fixedArg, Dictionary<string, object> argValues)
        //{
        //    DCRF.Contract.Connector.EndPoint result = new Connector.EndPoint(blockWeb);

        //    if (fixedArg.Obj != null)
        //    {
        //        result.Value = fixedArg.Obj;
        //    }
        //    else if (fixedArg.isConnectorCall)
        //    {
        //        string cblockId = (string)getVal(argValues, fixedArg.Address[1]);
        //        string cconnectorKey = (string)getVal(argValues, fixedArg.Address[2]);
        //        if (cblockId == null) cblockId = this.Id;

        //        result.BlockId = cblockId;
        //        result.ConnectorKey = cconnectorKey;
        //    }
        //    else
        //    {
        //        string cblockId = (string)getVal(argValues, fixedArg.Address[1]);
        //        string cServiceName = (string)getVal(argValues, fixedArg.Address[2]);
        //        if (cblockId == null) cblockId = this.Id;

        //        result.BlockId = cblockId;
        //        result.ServiceName = cServiceName;
        //    }

        //    foreach (DBSLineObjOrCall fa in fixedArg.Args)
        //    {
        //        DCRF.Contract.Connector.EndPoint ep = createEndPoint(fa, argValues);

        //        result.AddFixedArg(ep);
        //    }

        //    return result;
        //}

        private object handleProcessRequestServiceLine(DBSLineObjOrCall ooc, Dictionary<string, object> argValues)
        {
            if (ooc.Obj != null)
            {
                return getVal(argValues, ooc.Obj);
            }
            else if (ooc.Address == null && ooc.Args != null && ooc.Args.Count == 1)
            {
                return getVal(argValues, ooc.Args[0].Obj);
            }

            List<object> finalArgs = null;

            if (ooc.Args.Count > 0)
            {
                finalArgs = new List<object>();

                foreach (DBSLineObjOrCall ooc2 in ooc.Args)
                {
                    object itemResult = handleProcessRequestServiceLine(ooc2, argValues);

                    finalArgs.Add(itemResult);
                }
            }

            if (!ooc.isConnectorCall)
            {
                string blockId = (string) getVal(argValues, ooc.Address[1]);
                string service = (string)getVal(argValues, ooc.Address[2]);
                if (blockId == null) blockId = Id;

                if (finalArgs == null)
                {
                    return blockWeb[blockId].ProcessRequest(service);
                }
                else
                {
                    return blockWeb[blockId].ProcessRequest(service, finalArgs.ToArray());
                }
            }
            else
            {
                string blockId = (string)getVal(argValues, ooc.Address[1]);
                string connector = (string)getVal(argValues, ooc.Address[2]);
                if (blockId == null) blockId = Id;

                if (finalArgs == null)
                {
                    return blockWeb[blockId][connector].ProcessRequest();
                }
                else
                {
                    return blockWeb[blockId][connector].ProcessRequest(finalArgs.ToArray());
                }
            }
        }

    }
}
