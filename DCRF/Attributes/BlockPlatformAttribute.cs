using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Definition;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BlockPlatformAttribute : Attribute
    {
        private PlatformType pType = PlatformType.Neutral;

        public BlockPlatformAttribute(PlatformType type)
        {
            pType = type;
        }

        public PlatformType Platform
        {
            get
            {
                return pType;
            }
        }
    }
}
