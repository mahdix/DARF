using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Interface
{
    /// <summary>
    /// Represents container block of a web
    /// </summary>
    public interface IContainerBlock : IBlock
    {
        //TODO: merge this with this[guid] of IBlock which takes a direction

        /// <summary>
        /// Finds a block in the parent web of a block by using handle.
        /// Used by innerWeb of the block, when a handle is not found in the local blocks. 
        /// For example when a connector is 
        /// called that refers to a block in an external web.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        IBlock GetParentWebBlock(string handle);
    }
}
