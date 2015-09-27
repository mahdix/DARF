using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;

namespace DCRF.Contract.Impl
{
    /// <summary>
    /// classes for using in blocks (and not globalConnectors)/.
    /// So block author is not forced to write repeating keys and ...
    /// </summary>
    public class BlockConnectorBase
    {
        protected BlockBase parent = null;
        protected string key = null;

        public BlockConnectorBase(BlockBase parent, string key)
        {
            this.parent = parent;
            this.key = key;

            //force create connector without accessing internal state of the block
            IConnector temp = parent[key];
        }

        public Connector Connector
        {
            get
            {
                return (parent[key] as Connector);
            }
        }
    }
}
