using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.Interface
{
    /// <summary>
    /// Represents a block that is/can be hosted in a web.
    /// </summary>
    public interface IContainedBlock: IBlock
    {
        /// <summary>
        /// Initializes Block
        /// </summary>
        void InitBlock();

        IBlock GetInnerWebBlock(string handle);

        /// <summary>
        /// Create and initialize connectors
        /// </summary>
        void InitConnectors();

        object OnBeforeMigration(ref bool cancelOperation);
        void OnAfterMigration(object state);

        object OnBeforeReload(ref bool cancelOperation);
        void OnAfterReload(object state);

        /// <summary>
        /// called after block is loaded from xml file
        /// </summary>
        //void OnAfterLoad();
    }
}
