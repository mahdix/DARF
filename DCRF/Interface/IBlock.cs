using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DCRF.Primitive;
using System.Collections;
using DCRF.Core;
using DCRF.Definition;
using DCRF.Contract;
using System.Xml;

namespace DCRF.Interface
{
	/// <summary>
	/// Describes the common behavior of Blocks.
    /// Blocks must support multiple calls to dispose method. This is because the failover mechanism in Block brokers
    /// may call dispose method multiple times
    /// There will be a remote implementation of this interface
	/// </summary>
    public interface IBlock : IDisposable, IConnectorSubject
	{
        /// <summary>
        /// Unique identifier associated to this very block instance
        /// </summary>
        string Id
        {
            get;
        }

        /// <summary>
        /// Retrieve or create a connector
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        IConnector this[string key]
        {
            get;
        }

        //IConnector this[string key, string subKey]
        //{
        //    get;
        //}

        IBlockWeb ContainerWeb { get; }

        //bool HasConnector(string connectorKey);

        /// <summary>
        /// called by xml processor
        /// TODO: remove this method as it is really a special case and should not be declared in this place
        /// </summary>
        /// <param name="web"></param>
        //void SetInnerWeb(IBlockWeb web);
    }
}
