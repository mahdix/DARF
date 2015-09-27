using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Primitive;

namespace DCRF.Dynamic
{
    public class DBDefinition
    {
        public BlockHandle BaseType = null;
        public List<string> Connectors = new List<string>();
        public Dictionary<string, DBServiceDefinition> Services = new Dictionary<string, DBServiceDefinition>();
    }
}
