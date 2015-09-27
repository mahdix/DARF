using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.DBC
{
    public class CheckNotNullAttribute: Attribute
    {
        private string parameterName = "";

        public CheckNotNullAttribute(string parameterName)
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
    }
}
