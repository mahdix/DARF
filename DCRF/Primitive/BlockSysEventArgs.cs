using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Primitive
{
    public class BlockSysEventArgs
    {
        public BlockSysEventArgs(string id, string serviceName, object[] args, object result)
        {
            BlockId = id;
            ServiceName = serviceName;
            Result = result;
            Arguments = args;
        }

        public string BlockId = null;
        public string ServiceName = null;
        public object Result = null;
        public object[] Arguments = null;
    }
}
