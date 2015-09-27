using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Primitive
{
    [Serializable]
    public class MissingValue
    {
        public static MissingValue Value = new MissingValue();

        public override string ToString()
        {
            return string.Empty;
        }
    }
}
