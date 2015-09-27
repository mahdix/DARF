using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Primitive
{
    public enum LogType
    {
        HeartBeat = -2,
        PeerManager = -1,
        Connector,
        ProcessRequest,
        BlockWeb,
        AddBlock,
        DeleteBlock,
        EndPoint,
        PeerConnection,
        PeerRequest,
        MigrateBlock,
        Exception,
        General
    }
}
