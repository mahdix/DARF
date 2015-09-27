//using System;
//using System.Collections.Generic;
//using System.Text;
//using DCRF.Core;
//using DCRF.Interface;

//namespace DCRF.Contract.Impl
//{
//    public class BlockReference: BlockConnectorBase
//    {
//        public BlockReference(BlockBase parent, string key)
//            : base(parent, key)
//        {
//        }

//        public IBlock GetBlock()
//        {
//            string id = (string)Connector.ProcessRequest()[0];

//            return parent.GetParentWebBlock(id);
//        }

//        public List<IBlock> GetBlocks()
//        {
//            List<IBlock> result = new List<IBlock>();

//            foreach (string item in Connector.ProcessRequest())
//            {
//                result.Add(parent.GetParentWebBlock(item));
//            }

//            return result;
//        }
//    }
//}
