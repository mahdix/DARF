using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Contract
{
    /// <summary>
    /// Connector can be used in different scenarios. 
    /// 1 - When owner (Block) requires an input from external world. For example properties.
    /// 2 - When owner (Block) requires to notify external worl that something has happened: events
    /// 3 - When owner (Block) requires a service from outer worls: Dependancy
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// returns false if attach was not done
        /// </summary>
        /// <param name="blockId"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        bool AttachEndPoint(string blockId, string serviceName);
        bool AttachEndPoint(object value);
        bool AttachConnectorEndPoint(string blockId, string chainConnectorKey);

        //void DetachEndPoint();

        //void AddFixedArg(string endpointKey, DCRF.Contract.Connector.EndPoint arg);
        
        /// <summary>
        /// This method is used to a caller of the connector can check whether this call will result in 
        /// an infinite loop.
        /// </summary>
        /// <param name="blockHandle"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        bool HasEndPoint(string blockHandle, string serviceName);

        object ProcessRequest(params object[] args);

        /// <summary>
        /// we will need to rewrite logic here and not call GetValue(T,obj[]) because this will result in infinite loop
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="args"></param>
        /// <returns></returns>
        T GetValue<T>(params object[] args);
    }

    //public interface IGlobalConnector : IConnector
    //{
    //}
}
