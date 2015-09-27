using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Dynamic;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockBroker
{
    public class DynamicBlockBroker: IBlockBroker
    {
        private IBlockBroker failover = null;

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

        private Dictionary<BlockHandle, DBDefinition> blocks = new Dictionary<BlockHandle, DBDefinition>();

        public List<BlockHandle> Blocks
        {
            get 
            {
                return new List<BlockHandle>(blocks.Keys);
            }
        }

        public IContainedBlock LoadBlock(BlockHandle handle, params object[] args)
        {
            if (!blocks.ContainsKey(handle) )
            {
                if (failover != null)
                {
                    return failover.LoadBlock(handle);
                }

                return null;
            } 
            
            DBDefinition block = blocks[handle];

            return new DynamicBlock(block, args);
        }

        public void DisposeBlock(IBlock comp)
        {
            comp.Dispose();

            if (failover != null)
            {
                failover.DisposeBlock(comp);
            }
        }

        public void ClearCache()
        {
            throw new NotImplementedException();
        }

        public void AddBlock(BlockHandle handle, DBDefinition blockDef)
        {
            blocks[handle] = blockDef;
        }
    }
}
