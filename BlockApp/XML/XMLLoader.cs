using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DCRF.Core;
using DCRF.Primitive;
using DCRF.Interface;
using System.Collections;

namespace DCRF.XML
{
    public class XMLLoader
    {
        public static string DefaultExtension = ".xda";

        public static IBlockWeb LoadBlockWeb(string filePath, string blckWebId, IBlockBroker defaultBroker,
            Hashtable globalArgs = null)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath + DefaultExtension);

            NodeProcessor.DefaultBroker = defaultBroker;

            XmlElement rootElement = doc.SelectSingleNode("//blockWeb[@id='" + blckWebId + "']") as XmlElement;

            if (globalArgs != null)
            {
                foreach (string key in globalArgs.Keys)
                {
                    NodeProcessor.SetVar(key, globalArgs[key]);
                }
            }

            return NodeProcessor.ProcessNode(rootElement, null, null, null) as IBlockWeb;
        }
    }
}
