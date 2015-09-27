//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using DBML.Interface.Provider;
//using DCRF.Attributes;
//using DCRF.Core;
//using DCRF.Interface;

//namespace GeneralBlocks
//{
//    [BlockHandle("RowSaver")]
//    public class RowSaver: BlockBase
//    {
//        public RowSaver(string id, IContainerBlockWeb parent)
//            : base(id, parent)
//        {
//        }

//        [BlockService]
//        public void Save(string tblName, string dataOwnerId, IEnumerator fields)
//        {
//            DBML.Common.Dynamic.DynamicRow row = DBML.Common.Dynamic.DynamicRow.CreateRow(tblName);
//            fields.Reset();
//            while ((bool)fields.MoveNext())
//            {
//                string fieldName = (fields.Current as DBField).name;

//                if (blockWeb[dataOwnerId][fieldName] != null)
//                {
//                    object value = blockWeb[dataOwnerId][fieldName].GetValue<object>();

//                    row.SetFieldValue(fieldName, value);
//                }
//            }

//            row.SaveData();
//        }
//    }
//}
