using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Attributes;
using DCRF.Core;
using DCRF.Definition;
using DCRF.Interface;

namespace WinBlocks.Controls
{
    [BlockHandle("CoordinatorBlock")]
    public class CoordinatorBlock : BlockBase
    {
        public CoordinatorBlock(string id, IContainerBlockWeb parent)
            : base(id, parent)
        {
        }

        [BlockService]
        public void Test()
        {
            int h = 12;
        }


        [BlockService]
        public object GetUIElement()
        {
            return null;
        }
    }
}
