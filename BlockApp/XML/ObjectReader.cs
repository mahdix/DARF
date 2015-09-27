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
    public class ObjectReader
    {
        /// <summary>
        /// process value for template args and file references
        /// </summary>
        /// <param name="ovalue"></param>
        /// <returns></returns>
        public static object ProcessValue(string value, ref Type type)
        {
            object result = value;

            //if value starts with '#' it refers to a template argument
            if (value.StartsWith("#"))
            {
                string key = value.Substring(1);

                result = NodeProcessor.GetVar(key);
            }

            //if value starts with '@' it referes to a reference
            //if (value.StartsWith("@"))
            //{
            //    //read referenced name file and set stream as a result
            //    result = ReferenceProcessor.LoadReference(value.Substring(1));

            //    type = typeof(Stream);
            //}

            return result;
        }


        public static object ReadObject(XmlElement element)
        {
            return ReadObject(element, null);
        }

        public static object ReadObject(XmlElement element, Type defaultType)
        {
            object result = null;

            //in case that element does not have 'type' attribute and we are not given a default type
            //we suppose "String"
            if (element.HasAttribute("type"))
            {
                string type = element.GetAttribute("type");
                defaultType = Type.GetType(type);
            }

            if (defaultType == null)
            {
                defaultType = typeof(string);
            }

            if (element.GetAttribute("isNull") == "true")
            {
                //keep real type as well as value of null
                return Convert.ChangeType(null, defaultType);
            }

            if (!defaultType.IsArray)  //scalar value
            {
                string value = element.GetAttribute("value");
                object pValue = ProcessValue(value, ref defaultType);

                if (pValue.GetType().IsSubclassOf(defaultType))
                {
                    result = pValue;
                }
                else
                {
                    result = Convert.ChangeType(pValue, defaultType);
                }
            }
            else if (element.HasAttribute("separator"))  //simple array
            {
                Type elementType = defaultType.GetElementType();
                string value = element.GetAttribute("value");
                string separator = element.GetAttribute("separator");

                string[] items = value.Split(new string[] { separator }, StringSplitOptions.None);
                Array typedArray = Array.CreateInstance(elementType, items.Length);

                for (int i = 0; i < items.Length; i++)
                {
                    typedArray.SetValue(Convert.ChangeType(items[i], elementType), i);
                }

                result = typedArray;
            }
            else  //in this case we have an array whose items are children of "element" (complex array)
            {
                Type elementType = defaultType.GetElementType();
                Array typedArray = Array.CreateInstance(elementType, element.ChildNodes.Count);

                for (int i = 0; i < typedArray.Length; i++)
                {
                    object item = ReadObject(element.ChildNodes[i] as XmlElement, elementType);
                    typedArray.SetValue(Convert.ChangeType(item, elementType), i);
                }

                result = typedArray;
            }

            return result;
        }

        public static BlockHandle ReadBlockHandle(XmlElement myElement)
        {
            if (!myElement.HasAttribute("className")) return null;

            BlockHandle result = new BlockHandle();

            result.ClassName = myElement.GetAttribute("className");

            if (myElement.HasAttribute("version"))
            {
                result.BlockVersion = new BlockVersion(myElement.GetAttribute("version"));
            }

            if (myElement.HasAttribute("product"))
            {
                result.Product = myElement.GetAttribute("product");
            }

            return result;
        }
    }
}
