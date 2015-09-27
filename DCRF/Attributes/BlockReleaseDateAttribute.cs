using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class BlockReleaseDateAttribute: Attribute
    {
        private string date = "";

        public BlockReleaseDateAttribute(string t)
        {
            date = t;
        }

        public string Date
        {
            get
            {
                return date;
            }
        }
    }
}
