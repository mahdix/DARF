using System;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using DCRF.Primitive;
using DCRF.Core;
using DCRF.Helper;
using DCRF.Interface;
using DCRF.Attributes;

namespace DCRF.Helper
{
    public class BlockHelper
    {
        /// <summary>
        /// A helper method which treats serviceName as a method name in the Block and 
        /// calls the method with the provided arguments
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="serviceName"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object ProcessRequest(Dictionary<string, MethodBase> serviceMethods, IBlock instance, string serviceName, params object[] args)
        {
            //for services with the same name we should provide a list of arg types - this cannot work
            //maybe method takes object and we passed classA this way we won't be able to find the method           
            //but we can differ method with different number of arguments
            string key = serviceName;
            MethodBase mi = serviceMethods[key];

            if (mi == null)
            {
                throw new Exception("Cannot find service " + serviceName + " with specified number of arguments (" + args.Length.ToString() + ")");
            }
          
            try
            {
                object[] param = DCRFHelper.GetInstance().ConvertParams(mi,args);

                return mi.Invoke(instance, param); 
            }
            catch (Exception exc)
            {
                string message = exc.Message;
                Exception e = exc.InnerException;
                while (e != null)
                {
                    message += "(Inner: " + e.Message + ") ";
                    e = e.InnerException;
                }

                throw new Exception("Error executing service: " + message);
            }
        }

        internal static Dictionary<string, MethodBase> GetServices(IBlock instance)
        {
            return GetServices(instance, null);
        }

        internal static Dictionary<string, MethodBase> GetServices(IBlock instance, string name)
        {
            Dictionary<string, MethodBase> serviceMethods = new Dictionary<string, MethodBase>();

            foreach (MethodBase method in instance.GetType().GetMethods())
            {
                object[] attrs = method.GetCustomAttributes(typeof(BlockServiceAttribute), true);

                if (attrs.Length == 1 && (method.Name == name || name == null))
                {
                    if ( serviceMethods.ContainsKey(method.Name) ) throw new Exception("Invalid Service: Repeated name: "+method.Name);

                    serviceMethods.Add(method.Name, method);
                }
            }

            return serviceMethods;
        }
    }
}
