using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Interface;

namespace GeneralBlocks
{

    public class ConnectorConditioner: BlockBase
    {
        public ConnectorConditioner(string id, IContainerBlockWeb blockWeb)
            : base(id, blockWeb)
        {
        }

        public override void InitConnectors()
        {
            base.InitConnectors();

            createConnectors("TargetService", "Condition");

        }

        [BlockService]
        public object Invoke(params object[] args)
        {
            bool isSatisfied = this["Condition"].GetValue<bool>(args);

            if (isSatisfied)
            {
                return this["TargetService"].GetValue<object>(args);
            }

            return null;
        }


    }
}
