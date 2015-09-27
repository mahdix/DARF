using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;
using System.Xml;
using DCRF.Interface;
using DCRF.Primitive;
using DCRF.Contract;
using System.Collections;
using System.IO;
using System.Reflection;

namespace DCRF.XML
{
    public class ActionProcessor
    {
        public static void ProcessSetVariable(XmlElement actionElement, IBlockWeb containerWeb, string blockId)
        {
            string name = actionElement.GetAttribute("name");

            if (actionElement.ChildNodes.Count == 0)
            {
                //in this case the actionElement itself has the required data
                object value = ObjectReader.ReadObject(actionElement);
                NodeProcessor.SetVar(name, value);
            }
            else
            {
                //read first childr as endpoint because variable can only have one value
                List<Connector.EndPoint> endpoints = EndPointExtractor.ExtractArguments(actionElement, containerWeb);
                Connector.EndPoint endpoint = endpoints[0];

                List<object> result = new List<object>();
                endpoint.ProcessRequest(result, null, new ConnectorSysEventArgs(null, null));

                //now save result of endpoint process
                NodeProcessor.SetVar(name, result[0]);
            }
        }

        public static void ProcessSetProperty(XmlElement actionElement, IBlockWeb containerWeb, string blockId)
        {
            string connectorKey = actionElement.GetAttribute("connectorKey");
            object value = ObjectReader.ReadObject(actionElement);
            bool createConnector = false;

            if (actionElement.HasAttribute("create") && actionElement.GetAttribute("create") == "true")
            {
                createConnector = true;
            }

            if (actionElement.HasAttribute("blockId"))
            {
                string aBlockId = actionElement.GetAttribute("blockId");

                if (createConnector)
                {
                    containerWeb[aBlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, connectorKey, null);
                }

                containerWeb[aBlockId][connectorKey].AttachEndPoint(value);
            }
            else
            {
                if (createConnector)
                {
                    containerWeb[blockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, connectorKey, null);
                }

                containerWeb[blockId][connectorKey].AttachEndPoint(value);
            }
        }

        public static void ProcessProcessRequest(XmlElement actionElement, IBlockWeb containerWeb, string blockId)
        {
            string targetBlockId = actionElement.GetAttribute("blockId");

            if (actionElement.HasAttribute("blockId") == false)
            {
                targetBlockId = blockId;
            }

            string service = actionElement.GetAttribute("service");
            List<object> args = new List<object>();

            XmlNode fixedArgsNode = actionElement.SelectSingleNode("fixedArgs");

            if (fixedArgsNode != null)
            {
                List<Connector.EndPoint> endPoints = EndPointExtractor.ExtractArguments(fixedArgsNode as XmlElement, containerWeb);

                foreach (Connector.EndPoint endPoint in endPoints)
                {
                    endPoint.ProcessRequest(args, new object[] { }, new ConnectorSysEventArgs(null, null));
                }
            }

            containerWeb[targetBlockId].ProcessRequest(service, args.ToArray());
        }
    }
}
