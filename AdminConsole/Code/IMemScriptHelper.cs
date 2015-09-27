using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace AdminConsole.Code
{
    public interface IMemScriptHelper
    {
        Hashtable State { get; }
        Hashtable GlobalState { get; }
        void Write(string s);
        void WriteLine(string s);
    }
}
