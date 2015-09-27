using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Interface;

namespace WinBlocks
{
    [BlockHandle("MultiProcess")]
    public class MultiProcess: BlockBase
    {
        public MultiProcess(string id, IContainerBlockWeb blockWeb)
            : base(id, blockWeb)
        {
        }

        public override void InitConnectors()
        {
            base.InitConnectors();

            createConnectors("Call");
        }

        [BlockService]
        public object GetUIElement()
        {
            return null;
        }

        [BlockService]
        public object Process()
        {
            List<object> args = new List<object>();
            int i = 0;

            while (internalConnectors.ContainsKey("Arg" + i.ToString()))
            {
                args.Add(this["Arg" + i.ToString()].ProcessRequest());
                i++;
            }

            return this["Call"].ProcessRequest(args.ToArray());            
        }
    }
}
