using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface.Helper;
using DCRF.Interface.Primitive;

namespace DCRF.Interface.Core
{
    /// <summary>
    /// This interface shows behavior of a framework which acts as component broker.
    /// Loads components from repository.
    /// </summary>
    public interface IComponentBroker
    {
        /// <summary>
        /// Failover broker is a helper broker so if this broker could not perform the given operation, it calls failover
        /// This is used so a site can have multiple brokers transparently: e.g. local, remote and runtime broker
        /// </summary>
        IComponentBroker FailoverBroker
        {
            get;
        }

        /// <summary>
        /// Loads a component into memory
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        IComponent LoadComponent(CID handle);

        /// <summary>
        /// Called when we want to release resources that are associated to a component
        /// </summary>
        /// <param name="comp"></param>
        void DisposeComponent(IComponent comp);
        
        /// <summary>
        /// Configs broker to find common libraries when loading components
        /// </summary>
        /// <param name="files">Key is fileName and value is filePath which is used in Assembly.LoadFile</param>
        void SetGeneralLibrariesPath(Hashtable files);

        /// <summary>
        /// Sets folder of the repository so component broker can find folders of components when 
        /// Needs to load them
        /// </summary>
        /// <param name="options"></param>
        void SetupBroker(IComponentBrokerOptions options);
    }
}
