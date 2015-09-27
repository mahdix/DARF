using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Definition
{
    /// <summary>
    /// This is used to discriminate between different Blocks and BlockWeb so a BlockWeb for web platform cannot load
    /// windows Blocks. business Blocks that do not rely on a special platform are neutral.
    /// </summary>
    public enum PlatformType
    {
        /// <summary>
        /// The Block needs Windows Desktop libraries.
        /// </summary>
        Windows,
        /// <summary>
        /// Block requires ASP.NET services
        /// </summary>
        Web,
        /// <summary>
        /// Block requires services based on mobile environment
        /// </summary>
        Mobile,
        /// <summary>
        /// No special platform is required. This is a general block.
        /// </summary>
        Neutral
    }
}
