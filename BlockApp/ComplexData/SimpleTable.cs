using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DCRF.ComplexData
{
    /// <summary>
    /// A simple type table used to transfer complex data types (e.g. hashtable)
    /// This is a lightweight, serializable and marshalable data sutrcture
    /// </summary>
    public class SimpleTable
    {
        private List<string> keys = new List<string>();
        private List<Type> types = new List<Type>();
        private List<SimpleRow> values = new List<SimpleRow>();

        public SimpleTable()
        {
        }

        public List<SimpleRow> Rows
        {
            get
            {
                return values;
            }
        }

        public SimpleTable(params object[] ks)
        {
            AddKeys(ks);
        }

        public List<string> Keys
        {
            get
            {
                return keys;
            }
        }

        public List<Type> Types
        {
            get
            {
                return types;
            }
        }

        /// <summary>
        /// A series of this format: name,type,name,type
        /// </summary>
        /// <param name="ks"></param>
        public void AddKeys(params object[] ks)
        {
            for(int i=0;i<ks.Length;i+=2)
            {
                string key = ks[i].ToString();
                Type type = ks[i+1] as Type;

                Keys.Add(key);
                Types.Add(type);
            }
        }

        public SimpleRow AddRow()
        {
            SimpleRow result = new SimpleRow(this);

            values.Add(result);

            return result;
        }

        public SimpleRow AddRow(params object[] values)
        {
            SimpleRow result = AddRow();

            for (int i = 0; i < values.Length; i++)
            {
                result[i] = values[i];
            }

            return result;
        }

        public void DeleteRow(SimpleRow row)
        {
            values.Remove(row);
        }

        public SimpleRow this[int index]
        {
            get
            {
                DBC.Check.Require(values.Count > index);


                return values[index];
            }
        }

        public int Count
        {
            get
            {
                return values.Count;
            }
        }
    }
}

