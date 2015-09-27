using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Primitive
{
    public enum BlockMetaInfoType
    {
        Services,
        ServiceInfo,
        ServiceArgsInfo,
        ConnectorEndpoint,
        ConnectorInfo,
        InnerWebHost,
        InnerWebPort,
        InnerWebId,
        ConnectorKeys,
        BlockInfo        
    }

    public enum BlockMetaServiceType
    {
        DisableLog = 0,
        EnableLog = 1,
        InvokeConnector,
        GetInnerWeb,
        CreateConnector,
    }

    public enum BlockWebMetaInfoType
    {
        PeersInfo,
        Platform
    }

}
