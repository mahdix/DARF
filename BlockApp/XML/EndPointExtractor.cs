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
    public class EndPointExtractor
    {
        public static List<Connector.EndPoint> ExtractArguments(XmlElement fixedArgsElement, IBlockWeb containerWeb)
        {
            //TemplateProcessor.ProcessTemplateFile(fixedArgsElement, containerWeb, null);

            List<Connector.EndPoint> result = new List<Connector.EndPoint>();

            foreach (XmlElement argEndPoint in fixedArgsElement.ChildNodes)
            {
                result.Add(extractArgument(argEndPoint, containerWeb));
            }

            return result;
        }

        private static Connector.EndPoint extractArgument(XmlElement argEndPoint, IBlockWeb containerWeb)
        {
            if (argEndPoint.GetAttribute("isMissing") == "true")
            {
                return null;
            }
            else if (argEndPoint.Name == "serviceEndPoint" || argEndPoint.Name == "serviceArg")
            {
                string endpointBlockId = argEndPoint.GetAttribute("blockId");
                string endpointService = argEndPoint.GetAttribute("service");

                Connector.EndPoint ep = new Connector.EndPoint(containerWeb);
                ep.BlockId = endpointBlockId;
                ep.ServiceName = endpointService;

                if (argEndPoint.HasAttribute("targetType"))
                {
                    ep.TargetType = Type.GetType(argEndPoint.GetAttribute("targetType"));
                }

                addFixedArgsToEndPoint(containerWeb, argEndPoint, ep);

                return ep;
            }
            else if (argEndPoint.Name == "connectorEndPoint" || argEndPoint.Name == "connectorArg")
            {
                string endpointBlockId = argEndPoint.GetAttribute("blockId");
                string endPointConnectorKey = argEndPoint.GetAttribute("connectorKey");

                Connector.EndPoint ep = new Connector.EndPoint(containerWeb);
                ep.BlockId = endpointBlockId;
                ep.ConnectorKey = endPointConnectorKey;

                if (argEndPoint.HasAttribute("targetType"))
                {
                    ep.TargetType = Type.GetType(argEndPoint.GetAttribute("targetType"));
                }

                addFixedArgsToEndPoint(containerWeb, argEndPoint, ep);

                return ep;
            }
            else if (argEndPoint.Name == "valueEndPoint" || argEndPoint.Name == "valueArg")
            {
                object value = ObjectReader.ReadObject(argEndPoint);
                Connector.EndPoint ep = new Connector.EndPoint(containerWeb);
                ep.Value = value;

                return ep;
            }

            throw new InvalidDataException();
        }

        public static void ProcessAttachEndPointsAction(XmlElement actionElement, IBlockWeb containerWeb, string blockId)
        {
            string connectorKey = actionElement.GetAttribute("connectorKey");
            IConnector connector = null;

            bool hasTargetBlockId = actionElement.HasAttribute("blockId");
            string targetBlockId = actionElement.GetAttribute("blockId");
            bool createConnector = false;

            if (actionElement.HasAttribute("create") && actionElement.GetAttribute("create") == "true")
            {
                createConnector = true;
            }

            if (hasTargetBlockId)
            {
                if (createConnector)
                {
                    containerWeb[targetBlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, 
                        connectorKey, null);
                }

                connector = containerWeb[targetBlockId][connectorKey];
            }
            else
            {
                if (createConnector)
                {
                    containerWeb[blockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector,
                        connectorKey, null);
                }

                connector = containerWeb[blockId][connectorKey];
            }

            foreach (XmlElement endpointElement in actionElement.ChildNodes)
            {
                EndPointExtractor.ProcessAttachEndPoint(containerWeb, connector, endpointElement, blockId);
            }
        }
        
        private static void addFixedArgsToEndPoint(IBlockWeb containerWeb, XmlElement argEndPoint, Connector.EndPoint ep)
        {
            if (argEndPoint.SelectSingleNode("fixedArgs") != null)
            {
                List<Connector.EndPoint> subFixedArgs = ExtractArguments(argEndPoint.SelectSingleNode("fixedArgs") as XmlElement, containerWeb);

                foreach (Connector.EndPoint subFixedArg in subFixedArgs)
                {
                    ep.AddFixedArg(subFixedArg);
                }
            }
        }

        public static void ProcessAttachEndPoint(IBlockWeb containerWeb, IConnector connector, XmlElement endpointElement, string blockId)
        {
            string endpointKey = null;

            if (endpointElement.Name == "valueEndPoint")
            {
                if (!TemplateProcessor.ProcessTemplateFile(endpointElement, containerWeb, blockId, connector))
                {
                    object value = ObjectReader.ReadObject(endpointElement);
                    endpointKey = connector.AttachEndPoint(value);
                }
            }
            else if (endpointElement.Name == "serviceEndPoint")
            {
                if (!TemplateProcessor.ProcessTemplateFile(endpointElement, containerWeb, blockId, connector))
                {
                    string endpointBlockId = endpointElement.GetAttribute("blockId");
                    string endpointService = endpointElement.GetAttribute("service");

                    if (!endpointElement.HasAttribute("blockId")) endpointBlockId = blockId;

                    endpointKey = connector.AttachEndPoint(endpointBlockId, endpointService);
                }
            }
            else if (endpointElement.Name == "connectorEndPoint")
            {
                if (!TemplateProcessor.ProcessTemplateFile(endpointElement, containerWeb, blockId, connector))
                {
                    string endpointBlockId = endpointElement.GetAttribute("blockId");
                    string endPointConnectorKey = endpointElement.GetAttribute("connectorKey");

                    if (!endpointElement.HasAttribute("blockId")) endpointBlockId = blockId;

                    endpointKey = connector.AttachConnectorEndPoint(endpointBlockId, endPointConnectorKey);
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            //now process fixed args
            XmlElement fixedArgsElement = endpointElement.SelectSingleNode("fixedArgs") as XmlElement;

            if (fixedArgsElement != null && endpointKey != null)
            {
                List<Connector.EndPoint> fixedArgs = EndPointExtractor.ExtractArguments(fixedArgsElement, containerWeb);

                foreach (Connector.EndPoint endPoint in fixedArgs)
                {
                    connector.AddFixedArg(endpointKey, endPoint);
                }
            }
        }
    }
}
