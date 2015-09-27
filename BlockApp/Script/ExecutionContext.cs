using System;
using System.Collections.Generic;
using System.Text;
using BlockBroker;
using DCRF.Interface;

namespace BlockApp.Script
{
    public class ExecutionContext
    {
        //public List<string> argValues = new List<string>();
        public FileBlockBroker Broker = new FileBlockBroker();
        
        public IBlockWeb CurrentBlockWeb = null;
        public string CurrentBlockId = null;  //to use in block children

        public Dictionary<string, IBlockWeb> blockWebs = new Dictionary<string, IBlockWeb>();

        //public ExecutionContext(ExecutionContext context)
        //{
        //    blockWeb = context.blockWeb;
        //    argValues = context.argValues;
        //    Broker = context.Broker;
        //}

        public ExecutionContext()
        {
            // TODO: Complete member initialization
        }
    }
}
