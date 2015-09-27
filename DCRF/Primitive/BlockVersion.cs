using System;
using System.Xml;
using System.Xml.Serialization;
using System.Globalization;

namespace DCRF.Primitive
{
    /// <summary>
    /// this source is almost a copy of the reflected source code of Version class
    /// </summary>
	[Serializable]
	public sealed class BlockVersion : ICloneable, IComparable
	{
        //Major and minor together enable us to detect compatible versions
		// Fields
		private int _Major;
		private int _Minor;
        private int _Build;
		private int _Revision;

		// Methods
		public BlockVersion()
		{
			this._Build = -1;
			this._Revision = -1;
			this._Major = 0;
			this._Minor = 0;
		}

		public BlockVersion(string BlockVersion)
		{
			this._Build = -1;
			this._Revision = -1;
			if (BlockVersion == null)
			{
				throw new ArgumentNullException("BlockVersion");
			}
			string[] textArray = BlockVersion.Split(new char[] { '.' });
			int length = textArray.Length;
			if ((length < 2) || (length > 4))
			{
				throw new ArgumentException(("Arg_VersionString"));
			}
			this._Major = int.Parse(textArray[0], CultureInfo.InvariantCulture);
			if (this._Major < 0)
			{
				throw new ArgumentOutOfRangeException("BlockVersion", ("ArgumentOutOfRange_Version"));
			}
			this._Minor = int.Parse(textArray[1], CultureInfo.InvariantCulture);
			if (this._Minor < 0)
			{
				throw new ArgumentOutOfRangeException("BlockVersion", ("ArgumentOutOfRange_Version"));
			}
			length -= 2;
			if (length > 0)
			{
				this._Build = int.Parse(textArray[2], CultureInfo.InvariantCulture);
				if (this._Build < 0)
				{
					throw new ArgumentOutOfRangeException("build", ("ArgumentOutOfRange_Version"));
				}
				length--;
				if (length > 0)
				{
					this._Revision = int.Parse(textArray[3], CultureInfo.InvariantCulture);
					if (this._Revision < 0)
					{
						throw new ArgumentOutOfRangeException("revision", ("ArgumentOutOfRange_Version"));
					}
				}
			}
		}

		public BlockVersion(int major, int minor)
		{
			this._Build = -1;
			this._Revision = -1;
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", ("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", ("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
		}

		public BlockVersion(int major, int minor, int build)
		{
			this._Build = -1;
			this._Revision = -1;
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", ("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", ("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", ("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
		}

		public BlockVersion(int major, int minor, int build, int revision)
		{
			this._Build = -1;
			this._Revision = -1;
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", ("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", ("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", ("ArgumentOutOfRange_Version"));
			}
			if (revision < 0)
			{
				throw new ArgumentOutOfRangeException("revision", ("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
			this._Revision = revision;
		}

		public object Clone()
		{
			BlockVersion BlockVersion = new BlockVersion();
			BlockVersion._Major = this._Major;
			BlockVersion._Minor = this._Minor;
			BlockVersion._Build = this._Build;
			BlockVersion._Revision = this._Revision;
			return BlockVersion;
		}

		public int CompareTo(object BlockVersion)
		{
			if (BlockVersion == null)
			{
				return 1;
			}
			if (!(BlockVersion is BlockVersion))
			{
				throw new ArgumentException(("Arg_MustBeVersion"));
			}

			BlockVersion version2 = (BlockVersion) BlockVersion;
			if (this._Major != version2._Major)
			{
				if (this._Major > version2._Major)
				{
					return 1;
				}
				return -1;
			}
			if (this._Minor != version2._Minor)
			{
				if (this._Minor > version2._Minor)
				{
					return 1;
				}
				return -1;
			}
			if (this._Build != version2._Build)
			{
				if (this._Build > version2._Build)
				{
					return 1;
				}
				return -1;
			}
			if (this._Revision == version2._Revision)
			{
				return 0;
			}
			if (this._Revision > version2._Revision)
			{
				return 1;
			}
			return -1;
		}

		public override bool Equals(object obj)
		{
			if ((obj == null) || !(obj is BlockVersion))
			{
				return false;
			}
			BlockVersion BlockVersion = (BlockVersion) obj;
			return (((this._Major == BlockVersion._Major) && (this._Minor == BlockVersion._Minor)) && ((this._Build == BlockVersion._Build) && (this._Revision == BlockVersion._Revision)));
		}

		public override int GetHashCode()
		{
			int num = 0;
			num |= (this._Major & 15) << 0x1c;
			num |= (this._Minor & 0xff) << 20;
			num |= (this._Build & 0xff) << 12;
			return (num | (this._Revision & 0xfff));
		}

		public static bool operator ==(BlockVersion v1, BlockVersion v2)
		{ 
			return v1.Equals(v2);
		}

		public static bool operator >(BlockVersion v1, BlockVersion v2)
		{
			return (v2 < v1);
		}

		public static bool operator >=(BlockVersion v1, BlockVersion v2)
		{
			return (v2 <= v1);
		}

		public static bool operator !=(BlockVersion v1, BlockVersion v2)
		{
			return !(v1 == v2);
		}

		public static bool operator <(BlockVersion v1, BlockVersion v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return (v1.CompareTo(v2) < 0);
		}

		public static bool operator <=(BlockVersion v1, BlockVersion v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return (v1.CompareTo(v2) <= 0);
		}

		public override string ToString()
		{
			if (this._Build == -1)
			{
				return this.ToString(2);
			}
			if (this._Revision == -1)
			{
				return this.ToString(3);
			}
			return this.ToString(4);
		}

		public string ToString(int fieldCount)
		{
			switch (fieldCount)
			{
				case 0:
					return string.Empty;

				case 1:
					return (this._Major).ToString();

				case 2:
					return (this._Major + "." + this._Minor);
			}
			if (this._Build == -1)
			{
				throw new ArgumentException(string.Format(("ArgumentOutOfRange_Bounds_Lower_Upper"), "0", "2"), "fieldCount");
			}
			if (fieldCount == 3)
			{
				return string.Concat(new object[] { this._Major, ".", this._Minor, ".", this._Build });
			}
			if (this._Revision == -1)
			{
				throw new ArgumentException(string.Format(("ArgumentOutOfRange_Bounds_Lower_Upper"), "0", "3"), "fieldCount");
			}
			if (fieldCount != 4)
			{
				throw new ArgumentException(string.Format(("ArgumentOutOfRange_Bounds_Lower_Upper"), "0", "4"), "fieldCount");
			}
			return string.Concat(new object[] { this.Major, ".", this._Minor, ".", this._Build, ".", this._Revision });
		}

		public void SetValue(int major,int minor,int build,int revision)
		{
			Major = major;
			Minor = minor;
			Build = build;
			Revision = revision;
		}

		// Properties
		public int Build
		{
			get
			{
				return this._Build;
			}
			set
			{
				this._Build = value;
			}
		}

		public int Major
		{
			get
			{
				return this._Major;
			}
			set
			{
				this._Major = value;
			}
		}

		public int Minor
		{
			get
			{
				return this._Minor;
			}
			set
			{
				this._Minor = value;
			}
		}

		public int Revision
		{
			get
			{
				return this._Revision;
			}
			set
			{
				this._Revision = value;
			}
		}


        /// <summary>
        /// Returns true if minor and major parts of both versions are the same. When a specific version of a block is not 
        /// found compatible blocks can be used instead
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsCompatible(BlockVersion v)
        {
            if (this.Major == v.Major && this.Minor == v.Minor)
            {
                return true;
            }

            return false;
        }
	}
}


