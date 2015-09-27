using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DCRF.Helper;
using DCRF.Primitive;
using DCRF.Core;
using DCRF.Interface;

namespace DCRF.Interface
{
    /// <summary>
    /// This interface shows behavior of a framework which acts as Block broker.
    /// Loads Blocks from a source.
    /// </summary>
    public interface IBlockBroker
    {
        /// <summary>
        /// Failover broker is a helper broker so if this broker could not perform the given operation, it calls failover
        /// This is used so a BlockWeb can have multiple brokers transparently: e.g. local, remote and runtime broker
        /// </summary>
        IBlockBroker FailoverBroker
        {
            get;
            set;
        }

        /// <summary>
        /// List of available blocks in the broker internal repository
        /// </summary>
        List<BlockHandle> Blocks
        {
            get;
        }

        /// <summary>
        /// Finds and then loads a Block into memory
        /// </summary>
        /// <param name="handle">block identifier</param>
        /// <param name="args">parameters to pass to block constructor</param>
        /// <returns></returns>
        IContainedBlock LoadBlock(BlockHandle handle, params object[] args);

        /// <summary>
        /// Called when we want to release resources that are associated to a Block
        /// </summary>
        /// <param name="comp"></param>
        void DisposeBlock(IBlock comp);
        
        /// <summary>
        /// Sets folder of the repository so Block broker can find folders of Blocks when 
        /// Needs to load them
        /// </summary>
        /// <param name="options"></param>
        //void SetupBroker(IBlockBrokerOptions options);

        /// <summary>
        /// Clear internal cache of the broker. 
        /// Used when external BlockWeb wants to reload its blocks.
        /// </summary>
        void ClearCache();
    }
}
