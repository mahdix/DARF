using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Definition
{
    /// <summary>
    /// Type of block
    /// </summary>
    public enum BlockType
    {
        /// <summary>
        /// BlockWeb can contain multiple instances of this Block. They are differentiated by their Guid
        /// </summary>
        MultipleInstance,
        /// <summary>
        /// There can only be one instance of Blocks of this type in a BlockWeb
        /// </summary>
        SingleInstance
    }
}
