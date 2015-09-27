using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Core;
using DCRF.Primitive;
using System.Collections;
using DCRF.Helper;
using DCRF.Interface;
using DCRF.Attributes;

namespace BlockBroker
{
    /// <summary>
    /// This broker has a pre-defined list of available block instances and returns one of them upon a request (if available)
    /// </summary>
    public class RuntimeBlockBroker: IBlockBroker
    {
        private IBlockBroker failover = null;

        #region IBlockBroker Members

        public IBlockBroker FailoverBroker
        {
            get
            {
                return failover;
            }
            set
            {
                failover = value;
            }
        }

        private Dictionary<BlockHandle, IContainedBlock> runtimeBlocks = new Dictionary<BlockHandle, IContainedBlock>();

        public IContainedBlock LoadBlock(BlockHandle handle, params object[] args)
        {
            IContainedBlock result = runtimeBlocks[handle];

            if (result == null && failover != null)
            {
                return failover.LoadBlock(handle);
            }

            if (result != null)
            {
                //maybe we need to load multiple instances of this Block, so we need to create a new instance
                //per each request
                result = Activator.CreateInstance(result.GetType(), args) as IContainedBlock;
            }

            return result;
        }

        public List<BlockHandle> Blocks
        {
            get
            {
                return new List<BlockHandle>(runtimeBlocks.Keys);
            }
        }

        public void DisposeBlock(IBlock comp)
        {
            comp.Dispose();

            if (failover != null)
            {
                failover.DisposeBlock(comp);
            }
        }

        public void AddBlock<T>() where T:IBlock
        {
            Type t = typeof(T);

            IContainedBlock comp = Activator.CreateInstance(t, string.Empty, null) as IContainedBlock;

            runtimeBlocks.Add(getBlockId(comp), comp);
        }

        //TODO: there is a copy of this in simpleBlockBroker, merge them in a single place
        private BlockHandle getBlockId(IContainedBlock block)
        {
            object[] result = block.GetType().GetCustomAttributes(typeof(BlockHandleAttribute), true);

            if (result.Length == 1)
            {
                return (result[0] as BlockHandleAttribute).BlockId;
            }

            throw new Exception("BlockId has problem");
        }

        #endregion

        public void ClearCache()
        {
            runtimeBlocks.Clear();
        }
    }
}
