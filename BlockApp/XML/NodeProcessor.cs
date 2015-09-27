using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DCRF.Core;
using DCRF.Primitive;
using DCRF.Interface;
using System.Collections;
using DCRF.Contract;

namespace DCRF.XML
{
    public class NodeProcessor
    {
        private static Dictionary<string, object> variables = new Dictionary<string, object>();
        public static IBlockBroker DefaultBroker = null;

        public static object ProcessNode(XmlElement element, IBlockWeb blockWeb, string blockId, IConnector connector)
        {
            switch (element.Name)
            {
                case "blockWeb":
                    {
                        string id = null;
                        if (element.HasAttribute("id"))
                        {
                            id = element.GetAttribute("id");
                        }

                        BlockWeb newWeb = null;

                        if ( blockId != null )
                        {
                            newWeb = (BlockWeb) blockWeb[blockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.GetInnerWeb, 
                                null, null);
                        }
                        else
                        {
                            newWeb = new BlockWeb(id, DefaultBroker);
                        }

                        TemplateProcessor.ProcessTemplateFile(element, newWeb, blockId, connector);
                        ProcessNodeChildren(element, newWeb, blockId, connector);

                        return newWeb;
                    }
                case "actions":
                    {
                        TemplateProcessor.ProcessTemplateFile(element, blockWeb, blockId, connector);

                        ProcessNodeChildren(element, blockWeb, blockId, connector);

                        break;
                    }
                case "blocks":
                    {
                        TemplateProcessor.ProcessTemplateFile(element, blockWeb, blockId, connector);

                        ProcessNodeChildren(element, blockWeb, blockId, connector);

                        break;
                    }
                case "block":
                    {
                        BlockHandle cid = ObjectReader.ReadBlockHandle(element);
                        string id = null;

                        if (element.HasAttribute("id"))
                        {
                            id = element.GetAttribute("id");
                        }
                        else
                        {
                            id = "ctl_" + (Guid.NewGuid().ToString());
                        }

                        blockWeb.AddBlock(cid, id);

                        TemplateProcessor.ProcessTemplateFile(element, blockWeb, id, connector);

                        ProcessNodeChildren(element, blockWeb, id, connector);

                        //(blockWeb[id] as IContainedBlock).OnAfterLoad();

                        break;
                    }
                case "attachEndPoint":
                    {
                        EndPointExtractor.ProcessAttachEndPointsAction(element, blockWeb, blockId);
                        
                        break;
                    }
                case "processRequest":
                    {
                        ActionProcessor.ProcessProcessRequest(element, blockWeb, blockId);

                        break;
                    }
                case "setProperty":
                    {
                        ActionProcessor.ProcessSetProperty(element, blockWeb, blockId);

                        break;
                    }
                case "setVariable":
                    {
                        ActionProcessor.ProcessSetVariable(element, blockWeb, blockId);

                        break;
                    }
                case "serviceEndPoint":
                case "valueEndPoint":
                case "connectorEndPoint":
                    {
                        EndPointExtractor.ProcessAttachEndPoint(blockWeb, connector, element, blockId);
 
                        break;
                    }
            }

            return null;
        }

        public static void ProcessNodeChildren(XmlElement element, IBlockWeb blockWeb, string blockId, IConnector connector)
        {
            foreach (XmlNode actionNode in element.ChildNodes)
            {
                if (actionNode is XmlElement)
                {
                    NodeProcessor.ProcessNode(actionNode as XmlElement, blockWeb, blockId, connector);
                }
            }
        }

        public static void SetVar(string key, object value)
        {
            variables[key] = value;
        }
        public static object GetVar(string key)
        {
            if (variables.ContainsKey(key)) return variables[key];

            return key;
        }
    }
}
