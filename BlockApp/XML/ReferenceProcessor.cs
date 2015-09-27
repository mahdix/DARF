//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Xml;
//using System.IO;
//using System.Reflection;

//namespace DCRF.XML
//{
//    public class ReferenceProcessor
//    {
//        /// <summary>
//        /// Key is template key and value is file name of that template
//        /// </summary>
//        private static Dictionary<string, string> referenceFiles = new Dictionary<string, string>();
//        private static Dictionary<string, Stream> cache = new Dictionary<string, Stream>();
        
//        public static void ProcessReference(XmlElement refElement)
//        {
//            string key = refElement.GetAttribute("key");
//            string file = refElement.GetAttribute("file");

//            referenceFiles.Add(key, file);
//        }

//        public static Stream LoadReference(string key)
//        {
//            string fileKey = referenceFiles[key];

//            if (cache.ContainsKey(fileKey))
//            {
//                Stream result = cache[fileKey];
//                result.Seek(0, SeekOrigin.Begin);

//                return result;
//            }

//            Stream resultStream = new FileStream(fileKey, FileMode.Open);

//            cache.Add(fileKey, resultStream);
//            return resultStream;
//        }
//    }
//}
