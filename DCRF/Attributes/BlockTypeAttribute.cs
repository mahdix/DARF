using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Definition;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BlockTypeAttribute: Attribute
    {
        private BlockType bType = BlockType.MultipleInstance;

        public BlockTypeAttribute(BlockType type)
        {
            bType = type;
        }

        public BlockType Type
        {
            get
            {
                return bType;
            }
        }
    }
}
