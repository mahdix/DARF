using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using DCRF.Helper;
using DCRF.Interface;
using DCRF.DBC;

namespace DCRF.ComplexData
{
    public class TupleManager: ITupleManager
    {
        internal class ItemInfo
        {
            public ItemInfo(string k, Type t)
            {
                key = k;
                type = t;
            }

            public string key;
            public Type type;
        }

        //key:Identifier(string[]), Value: List<ItemInfo>
        private Hashtable idTable = new Hashtable();

        public int GetItemIndex(string id, string key)
        {
            string[] realId = DecodeId(id);

            for (int i = 0; i < realId.Length; i++)
            {
                int idx = -1;

                ItemInfo item = FindItemInfo(realId, i, key, ref idx);

                if (item != null)
                {
                    //we should calculate the index using the items in id array
                    //size is the size of parent id information
                    int size = GetTotalLevelsSize(realId, i);

                    return (size + idx);
                }
            }

            return -1;
        }

        public Type GetItemType(string id, int index)
        {
            string[] realId = DecodeId(id);
            int activeIndex = 0;
            int activeLevel = 1;

            while (activeLevel <= realId.Length)
            {
                int currentLevelSize = GetLevelSize(realId, activeLevel-1);

                if (currentLevelSize > 0)
                {
                    for (int i = 0; i < currentLevelSize; i++)
                    {
                        if (index == activeIndex)
                        {
                            //return indexth item in this level
                            ItemInfo item = FindItemInfo(realId, activeLevel - 1, i);

                            if (item == null)
                            {
                                return null;
                            }

                            return item.type;
                        }

                        activeIndex++;
                    }
                }

                activeLevel++;
            };

            return null;
        }

        private int GetTotalLevelsSize(string[] ids, int idsLen)
        {
            int result = 0;

            for (int i = 0; i < idsLen; i++)
            {
                result += GetLevelSize(ids, i);
            }

            return result;
        }

        private int GetLevelSize(string[] id, int idItemIndex)
        {
            List<ItemInfo> idData = GetLevel(id, idItemIndex);

            if (idData == null)
            {
                return 0;
            }

            return idData.Count;
        }

        private ItemInfo FindItemInfo(string[] id,int idLevelIndex, int index)
        {
            List<ItemInfo> idData = GetLevel(id, idLevelIndex);

            if (idData == null)
            {
                return null;
            }

            if (idData.Count <= index)
            {
                return null;
            }

            return idData[index];

        }

        private ItemInfo FindItemInfo(string[] id,int idLevelIndex, string key,ref int index)
        {
            List<ItemInfo> idLevel = GetLevel(id, idLevelIndex);

            if (idLevel == null)
            {
                return null;
            }

            for(int i=0;i<idLevel.Count;i++)
            {
                ItemInfo item = idLevel[i];

                if (item.key == key)
                {
                    index = i;
                    return item;
                }
            }

            return null;
        }

        /// <summary>
        /// ID can be a single string or a combination of IDs
        /// for example when we have "Control" and "Settings" id
        /// if we want to add a type and key information for "Update" information which
        /// belongs to "Control", id should be "Control.Update" this will not be confused
        /// with "Settings.Update"
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keysAndTypes"></param>
        public void RegisterItemTypes(string id, params object[] keysAndTypes)
        {
            Check.Require(keysAndTypes.Length % 2 == 0);

            List<ItemInfo> idLevel = GetLevel(id);

            if (idLevel == null)
            {
                idLevel = new List<ItemInfo>();
            }
            else
            {
                idLevel.Clear();
            }

            for (int i = 0; i < keysAndTypes.Length; i+=2)
            {
                object key = keysAndTypes[i];
                object type = keysAndTypes[i+1];


                idLevel.Add(new ItemInfo(key.ToString(), type as Type));
            }

            idTable[id] = idLevel;
        }

        /// <summary>
        /// id is a single id or a.b.c.d combination
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private List<ItemInfo> GetLevel(string id)
        {
            return idTable[id] as List<ItemInfo>;
        }
        
        private List<ItemInfo> GetLevel(string[] id, int idx)
        {
            //for x = 0 to idx
            //combine items of id array from index x to idx
            //this key exists? returns it

            //for example when id is {"A","B","C","D"} and idx is 3
            //look for "A.B.C.D", "B.C.D", "C.D" and "D" in the idTable

            for (int i = 0; i <= idx; i++)
            {
                string resultId = ConvertId(id, i, idx);

                List<ItemInfo> result = idTable[resultId] as List<ItemInfo>;

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private string ConvertId(string[] id, int startIndex,int endIndex)
        {
            string result = "";

            for (int i = startIndex; i <= endIndex; i++)
            {
                result += id[i];

                if (i < endIndex)
                {
                    result += ".";
                }
            }

            return result;
        }

        private string[] DecodeId(string id)
        {
            return id.Split('.');
        }
    }
}
