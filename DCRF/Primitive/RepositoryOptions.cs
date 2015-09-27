using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using DCRF.Interface;

namespace DCRF.Primitive
{
    public class RepositoryOptions : IBlockBrokerOptions
    {
        public string Folder = "";
        public Hashtable GeneralLibrariesPath = new Hashtable();
    }
}
