using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DCRF.Primitive
{
    /// <summary>
    /// Defines a Block identification structure
    /// </summary>
    [Serializable]
    public class BlockHandle
    {
        /// <summary>
        /// Represents a unique string used to differentiate between different instances of a block
        /// </summary>
        //public string Identifier = null;

        /// <summary>
        /// Identifier of this Block. Different versions of a Block with same ClassName are
        /// counted as different versions of a single Block. This helps DCRF keep track of different versions of
        /// a block
        /// </summary>
        public string ClassName;

        /// <summary>
        /// We can use assembly version but special Block version is more fine grained - maybe
        /// assembly has other Block or objects that have their our versioning system.
        /// </summary>
        public BlockVersion BlockVersion = new BlockVersion(0,0,0,0);

        /// <summary>
        /// Shows the system/products of which this is a Block.
        /// There can be same Blocks (same ID and version) if different products which are
        /// treated as totally different Blocks
        /// </summary>
        public string Product = "default";

        #region Methods

        /// <summary>
        /// This will be used to locate a Block between BlockWeb Blocks.
        /// For singlecall Blocks, we should also compare runtimehandle (e.g. multiple portlets in BlockWeb)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is BlockHandle))
            {
                return false;
            }

            BlockHandle c = obj as BlockHandle;

            return (ClassName == c.ClassName &&
                        BlockVersion == c.BlockVersion&&
                        Product == c.Product
                        );
        }


        public static bool operator ==(BlockHandle v1, BlockHandle v2)
        {
            if (v1 is BlockHandle)
            {
                return v1.Equals(v2);
            }

            if (!(v2 is BlockHandle))
            {
                return true;
            }

            return false;
            
        }

        public static bool operator !=(BlockHandle v1, BlockHandle v2)
        {
            return !(v1 == v2);
        }

        public void InitFrom(BlockHandle v)
        {
            ClassName = v.ClassName;
            BlockVersion = v.BlockVersion;
            Product = v.Product;
        }

        public override int GetHashCode()
        {
            string str = ToString();
            return str.GetHashCode();
        }

        /// <summary>
        /// Returns true if given Block identifier 'c' has the same product and Id as this Block
        /// And their version is compatible
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsCompatible(BlockHandle c)
        {
            return (ClassName == c.ClassName && 
                Product == c.Product &&
                BlockVersion.IsCompatible(c.BlockVersion));
        }

        public override string ToString()
        {
            return this.Product + "." + this.ClassName + "(v" + this.BlockVersion.ToString(4) + ")";
        }

        public BlockHandle()
        {
        }

        public BlockHandle(string clsName)
        {
            ClassName = clsName;
        }

        public BlockHandle(string clsName, BlockVersion version, string product)
        {
            ClassName = clsName;
            Product = product;
            BlockVersion = version;
        }

        public BlockHandle(string clsName, string product)
        {
            ClassName = clsName;
            Product = product;
        }

        public BlockHandle(string clsName, BlockVersion version)
        {
            ClassName = clsName;
            BlockVersion = version;
        }

        public static BlockHandle New(string clsName, int major, int minor, int build, int revision, string product)
        {
            BlockHandle result = new BlockHandle(clsName, new BlockVersion(major, minor, build, revision), product);

            return result;
        }

        public static BlockHandle New(string clsName, int major, int minor, int build, int revision)
        {
            BlockHandle result = new BlockHandle(clsName, new BlockVersion(major, minor, build, revision));

            return result;
        }

        public static BlockHandle New(string clsName)
        {
            BlockHandle result = new BlockHandle();
            result.ClassName = clsName;

            return result;
        }

        public static BlockHandle New(string clsName, string product)
        {
            BlockHandle result = new BlockHandle(clsName, product);

            return result;
        }

        #endregion

        public static BlockHandle Parse(string txt)
        {
            BlockHandle result = new BlockHandle();

            //txt is generated using CID.ToString
            //this.Product + "." + this.Identifier + "(v" + this.BlockVersion.ToString(4) + ")";
            int idx1 = txt.IndexOf(".");
            int idx2 = txt.IndexOf("(v", idx1 + 1);

            bool hasProduct = (idx1 != -1 || (idx2 != -1 && idx1 < idx2));
            bool hasVersion = (idx2 != -1);

            if (idx2 == -1) idx2 = txt.Length;

            if (hasProduct)
            {
                result.Product = txt.Substring(0, idx1);
            }

            result.ClassName = txt.Substring(idx1 + 1, idx2 - idx1 - 1);

            if (hasVersion)
            {
                string version = txt.Substring(idx2 + 2);
                version = version.Replace(")", "");
                result.BlockVersion = new BlockVersion(version);
            }

            return result;
        }

    }
}
