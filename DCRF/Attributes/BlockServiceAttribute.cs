using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Attributes
{
    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public sealed class BlockServiceAttribute : Attribute
    {
        // This is a positional argument.
        public BlockServiceAttribute()
        {
        }
    }

}
