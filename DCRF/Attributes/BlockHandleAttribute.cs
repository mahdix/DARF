using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Primitive;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BlockHandleAttribute: Attribute
    {
        private BlockHandle id = null;

        public BlockHandleAttribute(string clsName)
        {
            id = new BlockHandle(clsName);
        }

        public BlockHandleAttribute(string clsName, string product)
        {
            id = new BlockHandle(clsName, product);
        }

        public BlockHandleAttribute(string clsName, string product, int major, int minor, int build, int revision)
        {
            id = new BlockHandle(clsName, new BlockVersion(major, minor, build, revision), product);
        }

        public BlockHandle BlockId
        {
            get
            {
                return id;
            }
        }
    }
}
