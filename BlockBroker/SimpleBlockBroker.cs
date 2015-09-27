using System;
using System.Threading;
using System.Collections;
using DCRF;
using DCRF.Helper;
using DCRF.Core;
using DCRF.Primitive;
using System.Reflection;
using System.IO;
using DCRF.Interface;
using DCRF.DBC;
using System.Collections.Generic;
using DCRF.Attributes;

namespace BlockBroker
{
    /// <summary>
    /// This class loads Block without using a repository
    /// Just looks up DLLs in the given folder name passed upon broker setup 
    /// </summary>
    public class SimpleBlockBroker: IBlockBroker
    {
        private string folder = "";
        private IBlockBroker failover = null;
        private AssemblyLoader loader = new AssemblyLoader();
        private List<BlockHandle> blocks = null;

        //key is handle and value is A\B where A is assembly name and B is full class name
        private Hashtable cidInfo = new Hashtable();
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

        /// <summary>
        /// Returns a list of handles that belong to all blocks in all files in the broker folder.
        /// </summary>
        public List<BlockHandle> Blocks
        {
            get
            {
                if (blocks == null)
                {
                    blocks = new List<BlockHandle>();
                    cidInfo.Clear();

                    foreach (string fName in Directory.GetFiles(folder,"*.dll"))
                    {
                        string assemblyName = Path.GetFileName(fName);

                        List<string> blockClassNames = loader.GetBlocks(assemblyName, folder);

                        foreach (string clsName in blockClassNames)
                        {
                            //we create an instance of the block just to retrieve its handle
                            IBlock tmpBlock = loader.LoadBlock(assemblyName, clsName, folder, new object[] {null, null});

                            string assemblyPath = fName.Substring(folder.Length);
                            assemblyPath = assemblyPath.Trim('\\');

                            BlockHandle id = getBlockId(tmpBlock);

                            cidInfo[id] = assemblyPath + "\\" + clsName;

                            blocks.Add(id);
                        }
                    }
                }

                return blocks;
            }
        }

        public void ClearCache()
        {
            loader.ClearCache();
            blocks = null;

            if (failover != null)
            {
                failover.ClearCache();
            }
        }

        private BlockHandle getBlockId(IBlock block)
        {
            object[] result = block.GetType().GetCustomAttributes(typeof(BlockHandleAttribute), true);

            if (result.Length == 1)
            {
                return (result[0] as BlockHandleAttribute).BlockId;
            }

            throw new Exception("BlockId has problem");
        }

        /// <summary>
        /// Loads a block with given identity information.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public IContainedBlock LoadBlock(BlockHandle handle, params object[] args)
        {
            List<BlockHandle> blocks = Blocks;
            IContainedBlock result = null;       

            if (cidInfo.ContainsKey(handle))
            {
                string data = (string)cidInfo[handle];
                string assemblyName = data.Substring(0, data.IndexOf("\\"));
                string className = data.Substring(data.IndexOf("\\") + 1);

                //by default we suppose that className of the Block is the same as asembly name
                result = loader.LoadBlock(assemblyName, className, folder, args);
            }

            //TODO: lookup newer versions of this block in cidInfo

            if (result == null && failover != null)
            {
                return failover.LoadBlock(handle);
            }

            return result;
        }


        public void DisposeBlock(IBlock comp)
        {
            Check.Ensure(comp != null);

            comp.Dispose();

            if (failover != null)
            {
                failover.DisposeBlock(comp);
            }
        }

        public void SetupBroker(RepositoryOptions options)
        {
            if (options == null)
            {
                options = new RepositoryOptions();
                (options as RepositoryOptions).Folder = ".";
            }

            folder = (options as RepositoryOptions).Folder;
            loader.SetGeneralLibrariesPath((options as RepositoryOptions).GeneralLibrariesPath, null);
        }

        #endregion
    }
}
