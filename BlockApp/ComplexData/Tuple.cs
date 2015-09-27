using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Helper;
using DCRF.Interface;
using DCRF.DBC;

namespace DCRF.ComplexData
{
    /// <summary>
    /// Stores a tuple (a,b,c,d,e,f,...) which items can be retrieved using
    /// index and name(key). For example we can define "Text" as a key for first item (a). then tuple["Text"] will
    /// return value of'a'
    /// TupleManager converts names to indices and also keeps track of valid type for
    /// each key's value
    /// This can be used to store some kind of more complex data types
    /// </summary>
    public class Tuple
    {
        private ITupleManager _manager = null;
        private List<object> _items;

        private string _identifier = null;

        public void InitData(params object[] data)
        {
            //maybe all of items are not initialized here
            for (int i = 0; i < data.Length; i++)
            {
                _items[i] = data[i];
            }
        }

        public Tuple(int size)
        {
            _items = new List<object>(size);
            for (int i = 0; i < size; i++)
            {
                _items.Add(null);
            }
        }

        public Tuple(int size,ITupleManager manager,string id):this(size)
        {            
            _manager = manager;
            _identifier = id;
        }

        public Tuple(int size,ITupleManager manager,string id,params object[] data):this(size,manager,id)
        {
            InitData(data);
        }

        public Tuple(int size, params object[] data)
            : this(size)
        {
            InitData(data);
        }

        /// <summary>
        /// has the form of A.B.C
        /// </summary>
        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        // Properties
        public object this[int index]
        {
            get
            {
                if (index >= _items.Count || index < 0)
                {
                    return null;
                }

                return this._items[index];
            }
            set
            {
                if (IsTypeValid(index, value) == false)
                {
                    throw new Exception("Tuple entry has invalid type. ");
                }

                EnsureSize(index);
                this._items[index] = value;
            }
        }

        private bool IsTypeValid(int index, object value)
        {
            if (_manager == null || value == null )
            {
                //if no manager, anything is accepted
                return true;
            }

            Type correctType = _manager.GetItemType(Identifier, index);

            if (correctType == null)
            {
                //if nothing is specified then anything is ok
                return true;
            }

            if (correctType.IsInstanceOfType(value) == false)
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// make sure that the nternal list has a size to accept the given index
        /// if not, add null items
        /// </summary>
        /// <param name="index"></param>
        private void EnsureSize(int index)
        {
            if (index < _items.Count)
            {
                return;
            }

            int delta = (index - _items.Count)+1;

            for (int i = 0; i < delta; i++)
            {
                _items.Add(null);
            }
        }

        public object this[string key]
        {
            get
            {
                Check.Require(_manager != null);

                int idx = _manager.GetItemIndex(Identifier, key);

                return this[idx];
            }
            set
            {
                Check.Require(_manager != null);
                int idx = _manager.GetItemIndex(Identifier, key);

                if (idx == -1)
                {
                    throw new Exception("Invalid key: " + key);
                }

                this[idx] = value;
            }
        }

        public object this[string key,object nullValue]
        {
            get
            {
                object result = this[key];

                if (result == null)
                {
                    return nullValue;
                }

                return result;
            }
        }
    }
}
