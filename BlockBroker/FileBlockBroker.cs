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
    public class FileBlockBroker : IBlockBroker
    {
        private List<string> filePaths = new List<string>();
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

        public void SetLibFolder(string folder)
        {
            loader.SetGeneralLibrariesPath(null, folder);
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

                    foreach (string fName in filePaths)
                    {
                        string assemblyName = Path.GetFileName(fName);
                        string folder = Path.GetDirectoryName(fName);

                        List<string> blockClassNames = loader.GetBlocks(assemblyName, folder);

                        foreach (string clsName in blockClassNames)
                        {
                            //we create an instance of the block just to retrieve its handle
                            IBlock tmpBlock = loader.LoadBlock(assemblyName, clsName, folder, new object[] { null, null });

                            //string assemblyPath = fName.Substring(folder.Length);
                            //assemblyPath = assemblyPath.Trim('\\');

                            BlockHandle id = getBlockId(tmpBlock);

                            cidInfo[id] = fName + "\\" + clsName;

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
                string assemblyPath = data.Substring(0, data.LastIndexOf("\\"));
                string className = data.Substring(data.LastIndexOf("\\") + 1);

                //by default we suppose that className of the Block is the same as asembly name
                result = loader.LoadBlock(Path.GetFileName(assemblyPath), className,
                    Path.GetDirectoryName(assemblyPath), args);
            }

            //TODO: lookup newer versions of this block in cidInfo
            if (result == null && failover != null)
            {
                return failover.LoadBlock(handle, args);
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

        public void AddFile(string filePath)
        {
            filePaths.Add(filePath);
        }

        #endregion
    }
}
