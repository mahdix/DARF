using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Contract
{
    /// <summary>
    /// This is general definition of any object that can be subject of a connector.
    /// One example is a Block but external application can instantiate any class
    /// which is derived from this interface and use it as a connector.
    /// </summary>
    public interface IConnectorSubject
    {
        /// <summary>
        /// Performs a service of the Block and returns the result (if any)
        /// </summary>
        object ProcessRequest(string serviceName, params object[] args);
    }
}
