using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Dynamic
{
    public class DBSLineDefinition
    {
        public DBSLineType LineType = DBSLineType.ProcessRequest;
        public bool CreateConnector = false;
        public bool IsReturn = false;
        public DBSLineObjOrCall Start;
    }

    public class DBSLineObjOrCall
    {
        public object Obj = null;
        public bool isConnectorCall = false;
        public List<string> Address = null;  //BlockId.ServiceName or ConnectorName
        public List<DBSLineObjOrCall> Args = new List<DBSLineObjOrCall>();
    }

    public enum DBSLineType
    {
        ProcessRequest,
        AttachEndPoint
    }
}
