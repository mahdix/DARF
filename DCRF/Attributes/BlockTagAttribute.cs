using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class BlockTagAttribute: Attribute
    {
        private string txt = null;

        public BlockTagAttribute(string t)
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
