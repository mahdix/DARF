using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract;
using DCRF.Interface;
using DCRF.Primitive;

namespace DCRF.Definition
{
    public class SysEventCode
    {
        //BlockWeb Level Events
        public const string LoadBlock = "LoadBlock";
        public const string AddBlock = "AddBlock";
        public const string DeleteBlock = "DeleteBlock";

        public const string PeerConnect = "PeerConnect";
        public const string PeerDisconnect = "PeerDisconnect";

        public const string LogWebEvent = "LogWebEvent";
        public const string LogBlockEvent = "LogBlockEvent";
        
        //Block-Level Events
        //these events can cause infinite call loop so disabled currently
        public const string ProcessRequest = "ProcessRequest";
        public const string AttachEndPoint = "AttachEndPoint";
        public const string DetachEndPoint = "DetachEndPoint";
        public const string ConnectorProcessRequest = "ConnectorProcessRequest";

        //common  between block and bw
        public const string ExceptionOccured = "ExceptionOccured";

        public static string Join(string timing, string eventCode)
        {
            if (timing == null || timing == "")
            {
                return eventCode;
            }

            return timing + "." + eventCode;
        }

        public static string Join(string timing, string eventCode, string blockIdOrConnectorKey)
        {
            if (timing == null || timing == "")
            {
                return eventCode;
            }

            return timing + "." + eventCode + (blockIdOrConnectorKey == null ? "": ("."+blockIdOrConnectorKey));
        }
    }

    public class SysEventTiming
    {
        public const string Before = "B";
        public const string InsteadOf = "I";
        public const string After = "A";
    }

    public class SysEventHelper
    {
        public static string CoordinatorBlockID = "_coordinator_";

        public static bool FireSysEvent(IBlockWeb blockWeb, string timing, string eventCode, string blockIdOrConnectorKey, object eventArgs)
        {
            bool madeACall = false;

            if (blockWeb[CoordinatorBlockID] != null && blockIdOrConnectorKey != CoordinatorBlockID)
            {
                if (blockIdOrConnectorKey != null)
                {
                    string eventKey = SysEventCode.Join(timing, eventCode, blockIdOrConnectorKey);

                    if (blockWeb[CoordinatorBlockID][eventKey] != null)
                    {
                        blockWeb[CoordinatorBlockID][eventKey].ProcessRequest(eventArgs);
                        madeACall = true;
                    }
                }

                string eventKey2 = SysEventCode.Join(timing, eventCode);

                if (blockWeb[CoordinatorBlockID][eventKey2] != null)
                {
                    blockWeb[CoordinatorBlockID][eventKey2].ProcessRequest(eventArgs);
                    madeACall = true;
                }
            }

            return madeACall;
        }
    }
}
