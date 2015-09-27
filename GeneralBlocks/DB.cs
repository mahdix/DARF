//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using DBML.Interface.Provider;
//using DCRF.Attributes;
//using DCRF.Core;
//using DCRF.Interface;
//using DCRF.Primitive;

//namespace GeneralBlocks
//{
//    [BlockHandle("DB")]
//    public class DB: BlockBase
//    {
//        public DB(string id, IContainerBlockWeb blockWeb)
//            : base(id, blockWeb)
//        {
//        }

//        public override void InitConnectors()
//        {
//            base.InitConnectors();

//            createConnectors("DBType", "DBFilePath");
//        }

//        [BlockService]
//        public void Init(string filePath)
//        {
//            DBML.DBCore.file = filePath;
//            DBML.DBCore.initializeProvider(DBML.ProviderType.SQLite);
//        }

//        [BlockService]
//        public double GetDouble()
//        {
//            return 3.45;
//        }

//        [BlockService]
//        public IEnumerator ReadTableSchme(string tableName)
//        {
//            DBField[] result = DBML.DBCore.getInstance().GetFieldsInfo(tableName);
//            return result.GetEnumerator();
//        }

//        [BlockService]
//        public object MoveNext(IEnumerator en)
//        {
//            return en.MoveNext();
//        }

//        [BlockService]
//        public string GetFieldName(IEnumerator en)
//        {
//            return (en.Current as DBField).name;
//        }

//        [BlockService]
//        public enumDataType GetFieldType(IEnumerator en)
//        {
//            return (en.Current as DBField).dataType;
//        }

//        [BlockService]
//        public BlockHandle GetFieldTypeBlockHandle(enumDataType type)
//        {
//            return new BlockHandle("TextBox");
//        }
//    }
//}
