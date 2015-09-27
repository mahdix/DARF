using System;
using System.Collections.Generic;
using System.Text;

namespace NetSockets.Peer
{
    public class PeerMsgDef
    {
        public const int HeartbeatInterval = 5000;
        public const int HeartbeatValidness = 100000;
        public const int SendMessageWaitResponseTimeout = 2000;
        public const int ConnectTimeout = 5000;
        public const int BroadcastMessageWaitResponseTimeout = 10000;

        public const string ConnectMsgCode = "__CONNECT__";
        public const string DisconnectMsgCode = "__DISCONNECT__";
        public const string HeartbeatMsgCode = "__HEARTBEAT__";
    }

    public enum MsgType
    {
        Normal = 0,
        Exception = 1
    }
}
