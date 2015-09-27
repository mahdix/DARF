using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;

namespace DCRF.Contract.Impl
{
    /// <summary>
    /// just a helper class to prevent rewriting keys
    /// </summary>
    public class BlockProperty<T> : BlockConnectorBase
    {
        public BlockProperty(BlockBase parent, string key)
            : base(parent, key)
        {
        }

        public void SetValue(T value)
        {
            Connector.AttachEndPoint(value);
        }

        public T GetValue()
        {
            return (T)Connector.ProcessRequest();
        }

        //public List<T> GetValues()
        //{
        //    List<T> result = new List<T>();

        //    foreach (object item in Connector.ProcessRequest())
        //    {
        //        result.Add((T)item);
        //    }

        //    return result;
        //}
    }
}
