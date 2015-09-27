using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.DBC
{
    public class CheckLessThanAttribute: Attribute
    {
        private string parameterName = "";
        private int target = 0;

        public CheckLessThanAttribute(string parameterName,int target)
        {
            this.parameterName = parameterName;
        }

        public string ParameterName
        {
            get
            {
                return parameterName;
            }
        }

        public int Target
        {
            get
            {
                return target;
            }
        }
    }
}
