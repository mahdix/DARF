//using System;
//using System.Collections.Generic;
//using System.Text;
//using DCRF.Core;

//namespace DCRF.Contract.Impl
//{
//    /// <summary>
//    /// just a helper class to prevent rewriting keys
//    /// </summary>
//    public class BlockConnector<T> : BlockConnectorBase
//    {
//        public BlockConnector(BlockBase parent, string key)
//            : base(parent, key)
//        {
//        }

//        public T ProcessRequest(params object[] args)
//        {
//            return (T)Connector.ProcessRequest(args)[0];
//        }

//        public List<T> ProcessRequestAll(params object[] args)
//        {
//            List<T> result = new List<T>();

//            foreach (object item in Connector.ProcessRequest(args))
//            {
//                result.Add((T)item);
//            }

//            return result;
//        }
//    }
//}
