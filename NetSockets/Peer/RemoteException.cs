using System;
using System.Collections.Generic;
using System.Text;

namespace NetSockets.Peer
{
    public class RemoteException: Exception
    {
        public string ExceptionType = null;

        public RemoteException(string type, string message): base(message)
        {
            ExceptionType = type;
        }
    }
}
