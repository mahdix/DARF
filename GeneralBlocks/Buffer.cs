using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Interface;

namespace GeneralBlocks
{
    public class Buffer: BlockBase
    {
        public Buffer(string id, IContainerBlockWeb blockWeb)
            : base(id, blockWeb)
        {
        }

        public override void InitConnectors()
        {
            base.InitConnectors();

            createConnectors("Value");
        }

        [BlockService]
        public object GetValue()
        {
            return this["Value"].GetValue<object>();
        }
    }
}
