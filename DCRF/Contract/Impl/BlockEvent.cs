using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using DCRF.Core;

namespace DCRF.Contract.Impl
{
    /// <summary>
    /// just a helper class to prevent rewriting keys
    /// </summary>
    public class BlockEvent: BlockConnectorBase
    {

        public BlockEvent(BlockBase parent, string key): base(parent, key)
        {
        }

        public void Raise(params object[] args)
        {
            Connector.ProcessRequest(args);
        }
    }
}
