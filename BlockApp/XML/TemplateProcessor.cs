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
    public class TemplateProcessor
    {
        /// <summary>
        /// input parameters specify the context in which a template is being processed
        /// </summary>
        /// <param name="element"></param>
        /// <param name="containerWeb"></param>
        /// <param name="blockId"></param>
        /// <param name="connector"></param>
        public static bool ProcessTemplateFile(XmlElement element, IBlockWeb containerWeb, string blockId, IConnector connector)
        {
            if (element.HasAttribute("template"))
            {
                //general structure to refer to a template is: FilePath@Tag[id] where File is a reference file name (optional)
                //tag is tagname (optional, default is same as current element) and id is identifier of the template tag
                //the simplest style is: id
                //another example: file1@blockWeb[aaa]
                //file extension is not required: xda
                pushTemplateArgs(element, containerWeb);
                
                string templateRef = element.GetAttribute("template");

                bool isRemoteTemplate = templateRef.Contains("@");
                bool hasTemplateTagName = templateRef.Contains("[");

                string templateTagName = null;
                string templateId = null;
                string templateFilePath = null;

                if (isRemoteTemplate)
                {
                    string[] fileAndId = templateRef.Split('@');

                    templateFilePath = fileAndId[0] + XMLLoader.DefaultExtension;
                    templateRef = fileAndId[1];
                }

                if (hasTemplateTagName)
                {
                    int idx = templateRef.IndexOf("[");

                    templateTagName = templateRef.Substring(0, idx);
                    templateId = templateRef.Substring(idx + 1, templateRef.Length - idx - 2);
                }
                else
                {
                    templateTagName = element.Name;
                    templateId = templateRef;
                }

                XmlDocument templateDocument = element.OwnerDocument;

                if (isRemoteTemplate)
                {
                    Stream file = new FileStream(templateFilePath, FileMode.Open);
                    templateDocument = new XmlDocument();
                    templateDocument.Load(file);
                }

                string xpath = string.Format("//{0}[@id='{1}']", templateTagName, templateId);
                XmlElement templateNode = templateDocument.SelectSingleNode(xpath) as XmlElement;

                //maybe the target element has a template too
                ProcessTemplateFile(templateNode, containerWeb, blockId, connector);

                if (templateTagName.EndsWith("endpoint", StringComparison.InvariantCultureIgnoreCase))
                {
                    NodeProcessor.ProcessNode(templateNode, containerWeb, blockId, connector);
                }
                else
                {
                    NodeProcessor.ProcessNodeChildren(templateNode, containerWeb, blockId, connector);
                }
                
                return true;
            }

            return false;
        }

        private static void pushTemplateArgs(XmlElement parentElement, IBlockWeb containerWeb, string tag = "templateArgs")
        {
            XmlElement myElement = parentElement.SelectSingleNode(tag) as XmlElement;

            if (myElement == null) return;

            List<Connector.EndPoint> endPoints = EndPointExtractor.ExtractArguments(myElement, containerWeb);

            List<object> tempList = new List<object>();
            Hashtable result = new Hashtable();

            ConnectorSysEventArgs eventArgs = new ConnectorSysEventArgs(null, null);

            for (int i = 0; i < myElement.ChildNodes.Count; i++)
            {
                XmlElement argElement = myElement.ChildNodes[i] as XmlElement;

                string argKey = argElement.GetAttribute("argKey");

                endPoints[i].ProcessRequest(tempList, null, eventArgs);

                result[argKey] = tempList[0];
                tempList.Clear();
            }

            if (result.Count > 0)
            {
                foreach(string key in result.Keys)
                {
                    NodeProcessor.SetVar(key, result[key]);
                }
            }
        }
    }
}
