using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BlockFriendlyNameAttribute : Attribute
    {
        private string txt = null;

        public BlockFriendlyNameAttribute(string t)
        {
            txt = t;
        }

        public string Text
        {
            get
            {
                return txt;
            }
        }
    }
}
