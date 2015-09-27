using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Helper;
using DCRF.Primitive;
using System.Collections;
using DCRF.Core;
using DCRF.Definition;
using NetSockets.Peer;
using DCRF.Contract;

namespace DCRF.Interface
{
    /// <summary>
    /// </summary>
    public interface IBlockWeb: IDisposable
    {
        /// <summary>
        /// A list of Ids of blocks loaded in this web.
        /// </summary>
        IList<string> BlockIds
        {
            get;
        }

        //IBlockBroker Broker { get; } 

        /// <summary>
        /// Sometimes some blocks need to make a decision according to identifier of another block from which there is only
        /// a ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        BlockHandle GetBlockHandle(string id);

        /// <summary>
        /// Number of blocks in this web.
        /// </summary>
        int BlockCount
        {
            get;
        }

        /// <summary>
        /// Returns metainfo about BlockWeb. Currently used in AdminConsole application.
        /// for example: GlobalConnectorKeys and EndPoints, Platform and Peers' information
        /// </summary>
        /// <param name="type"></param>
        /// <param name="itemName"></param>
        /// <returns></returns>
        object GetBlockWebMetaInfo(BlockWebMetaInfoType type, string itemName);

        /// <summary>
        /// This methods uses broker to create a new instance of a block with given handle.
        /// We need metadata when adding a Block. This can be used to find alternative Blocks
        /// at BlockWeb level on-demand. For example instead of 1.0.0.0 we may have only 1.0.0.3 of the same Block
        /// as an alternative.
        /// </summary>
        /// <returns>Unique identifier of created block. Equals to handle.Identifier or a random string if it is null</returns>
        string AddBlock(BlockHandle handle, string identifier=null);

        /// <summary>
        /// Delete a block from blocks loaded in the web.
        /// </summary>
        /// <param name="handle"></param>
        void DeleteBlock(string handle); 

        /// <summary>
        /// As we do not like to pass block object to requesters, we have a wrapper for IBlock methods.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IBlock this[string id] { get; }
        IBlock this[string id, int direction] { get; }


        //IGlobalConnector GetConnector(string key, string subKey);
        //IGlobalConnector GetConnector(string key);
        //void CreateConnector(string key);
        /// <summary>
        /// Returns URL address of this web. Other webs can connect to this blockWeb using this address
        /// </summary>
        string Address
        {
            get;
        }

        /// <summary>
        /// This method looks into Blocks in the BlockWeb that have exact same version,id,product which is specified by handle
        /// there may be many because it may be multiple-instance
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        //List<string> FindBlocks(BlockHandle handle);

        /// <summary>
        /// This methods searches in BlockWeb's Blocks and returns a list of Block identifiers that
        /// have the given tag. This is used to have a grouping mechanism.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns>A list of handles that can be used to target a specific block</returns>
        //List<string> FindBlocks(string tag);

        string Id
        {
            get;
        }

        void ReloadBlocks();

        /// <summary>
        /// Connect to an external blockWeb
        /// </summary>
        /// <param name="url"></param>
        bool Connect(string host, int port, string peerId);
        bool Connect(IBlockWeb web, string peerId);
        void Disconnect(string peerId);
        bool MigrateBlock(string id, string peerId);
        IBlockWeb GetPeer(string peerId);
    }
}
