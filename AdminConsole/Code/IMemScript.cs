using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DCRF.Interface;

namespace AdminConsole.Code
{
    public interface IMemScript
    {
        void main(IMemScriptHelper helper, IBlockWeb blockWeb);
    }
}
