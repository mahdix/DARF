using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;
using DCRF.Primitive;
using DCRF.Attributes;
using DCRF.Interface;

namespace DCRF.Helper
{
    public class DCRFHelper
    {
        private static DCRFHelper instance = new DCRFHelper();
        public static DCRFHelper GetInstance()
        {
            return instance;
        }
        
        public object[] ConvertParams(MethodBase method,params object[] args)
        {
            ParameterInfo[] param = method.GetParameters();
            object lastArg = null;
            Type lastArgType = null;
            Type lastParamType = null;
            bool lastParamIsArray = false;

            if (param.Length == 0)
            {
                return new object[0];
            }

            if ( args.Length > 0 )
            {
                lastArg = args[args.Length-1];

                if (lastArg != null)
                {
                    lastArgType = lastArg.GetType();
                }
            }
            if (param.Length > 0)
            {
                lastParamType = param[param.Length - 1].ParameterType;
                lastParamIsArray = lastParamType.IsArray;

                if (lastParamType.IsArray)
                {
                    //Arrays have a get method whose return type of the inner type of array
                    //lastParamType = lastParamType.GetMethod("Get").ReturnParameter.ParameterType;

                    lastParamType = lastParamType.GetElementType();
                }
            }

            List<object> result = new List<object>();

            for(int i=0;i<param.Length-1;i++)
            {
                result.Add(args[i]);
            }

            if (args.Length == 0)
            {
                //if no arg is passed, pass null values
                for (int i = 0; i < param.Length; i++)
                {
                    result.Add(null);
                }
            }
            else if (args.Length == param.Length - 1 && lastParamIsArray)
            {
                //the last parameter is params which is null (no argument exists in the call)
                ArrayList temp = new ArrayList();

                result.Add(temp.ToArray(lastParamType));
            }
            else if (args.Length == param.Length + 1 && (lastArgType == typeof(ConnectorSysEventArgs)))
            {
                result.Add(args[param.Length - 1]);
            }
            else if (args.Length > param.Length)
            {
                //extra parameters must be fitted into an object array
                ArrayList extra = new ArrayList(args.Length - param.Length);
                for (int i = 0; i < extra.Count; i++)
                {
                    extra.Add(args[param.Length + i - 1]);
                }

                result.Add(extra.ToArray(lastParamType));
            }
            else if (args.Length == param.Length)
            {
                //if the last param is not of type X[] 
                //add the last args to the result
                if (lastParamIsArray == false)
                {
                    result.Add(lastArg);
                }
                else
                {
                    //if the last arg type is object[] (and so is last param type) then add it to result normally
                    if (lastArgType != null && lastArgType.IsArray)
                    {
                        //Question: if last param is params object[] and its value in args is an array
                        //what happens?
                        //Answer: arg cannot be array coz param is array of object, so args should be of
                        //object type not arrays of objects
                        result.Add(lastArg);
                    }
                    else
                    {
                        //if not, the last argument should be encapsulated in an object[] and added to result
                        ArrayList temp = new ArrayList(1);
                        temp.Add(lastArg);

                        result.Add(temp.ToArray(lastParamType));
                    }
                }
            }


            return result.ToArray();
        }

        internal static string GetBlockInfo(IBlock blockBase)
        {
            //[BlockComments("Some comments")]
            //[BlockCompany("mahdix")]
            //[BlockFriendlyName("Friend")]
            //[BlockReleaseDate("121212")]

            object[] attributes = blockBase.GetType().GetCustomAttributes(true);
            BlockHandle blockId = null;
            string comments = null;
            string company = null;
            string friendlyName = null;
            string releaseDate = null;

            foreach (object item in attributes)
            {
                if (item is BlockHandleAttribute) blockId = (item as BlockHandleAttribute).BlockId;
                if (item is BlockCommentsAttribute) comments = (item as BlockCommentsAttribute).Text;
                if (item is BlockCompanyAttribute) company = (item as BlockCompanyAttribute).Text;
                if (item is BlockFriendlyNameAttribute) friendlyName = (item as BlockFriendlyNameAttribute).Text;
                if (item is BlockReleaseDateAttribute) releaseDate = (item as BlockReleaseDateAttribute).Date;
            }

            StringBuilder sb = new StringBuilder();
            sb.Append("Unique Handle: ");
            sb.Append(blockBase.Id);
            sb.Append("\r\n");

            sb.Append("Identifier: ");
            if(blockId != null ) sb.Append(blockId.ToString());
            sb.Append("\r\n");

            sb.Append("Friendly Name: ");
            if (friendlyName != null) sb.Append(friendlyName);
            sb.Append("\r\n");

            sb.Append("Comments: ");
            if (comments != null) sb.Append(comments);
            sb.Append("\r\n");


            sb.Append("Company: ");
            if (company != null) sb.Append(company);
            sb.Append("\r\n");


            sb.Append("Release Date: ");
            if (releaseDate != null) sb.Append(releaseDate);
            sb.Append("\r\n");


            return sb.ToString();
        }

        internal static Dictionary<string, string> GetServiceArgsInfo(object blockBase, string serviceName)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            MethodInfo mi = blockBase.GetType().GetMethod(serviceName);

            foreach (ParameterInfo pi in mi.GetParameters())
            {
                result.Add(pi.Name, pi.ParameterType.AssemblyQualifiedName);
            }

            return result;
        }

        internal static string GetMethodInfo(object blockBase, string serviceName)
        {
            MethodInfo mi = blockBase.GetType().GetMethod(serviceName);

            string returnType = mi.ReturnType.Name;
            StringBuilder sb = new StringBuilder();

            foreach (ParameterInfo pi in mi.GetParameters())
            {
                string pType = pi.ParameterType.Name;

                if (pi.ParameterType.IsArray) pType += "[]";

                sb.Append(string.Format("{0}{1} {2},", (pi.IsOut ? "out " : (pi.IsRetval ? "ref " : "")),
                    pType, pi.Name));
            }

            string args = sb.ToString();

            if (mi.GetParameters().Length > 0)
            {
                args = args.Substring(0, args.Length - 1);
            }

            return string.Format("{0} {1}({2})", returnType, serviceName, args);
        }

        internal static string GetConnectorInfo(object blockBase, string connectorKey)
        {
            //foreach (BlockConnectorAttribute con in blockBase.GetType().GetCustomAttributes(typeof(BlockConnectorAttribute), true))
            //{
            //    if (con.ConnectorKey == connectorKey)
            //    {
            //        return con.Signature;
            //    }
            //}

            //throw new Exception("Invalid Connector Key: " + connectorKey);
            return "Not Implemented";
        }
    }
}
