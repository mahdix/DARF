using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;
using DCRF.Primitive;
using DCRF.DBC;

namespace DCRF.ComplexData
{
    /// <summary>
    /// Stores a collection of Tuples
    /// </summary>
    public class TupleCollection: CollectionBase
    {
        //this is only used to initialize manager of created tuples
        //this will be manager of created tuples
        private ITupleManager _manager = null;

        //each tuple that is created by this collection will have this idenfitier prefixed in its id
        private string _identifier = null;
        public string Identifier
        {
            get
            {
                return _identifier;
            }
        }

        public TupleCollection()
        {
        }

        public TupleCollection(string id)
        {
            _identifier = id;
        }

        public TupleCollection(string id,ITupleManager man):this(id)
        {
            _manager = man;
        }

        public Tuple this[int index]
        {
            get
            {
                return this.List[index] as Tuple;
            }
            set
            {
                this.List[index] = value;
            }
        }

        public Tuple CreateTuple()
        {
            return CreateTuple(0);
        }

        public Tuple CreateTuple(int size)
        {
            return CreateTuple(size, null);
        }

        public Tuple CreateTuple(string id)
        {
            return CreateTuple(0, id);
        }

        /// <summary>
        /// When id of this object ia A.B and id argument is C.D then
        /// id of the tuple will be A.B.C.D
        /// </summary>
        /// <param name="size"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Tuple CreateTuple(int size,string id)
        {
            string tupleId = id;

            if (Identifier != null)
            {
                tupleId = Identifier;

                if (id != null)
                {
                    tupleId = Identifier + "." + id;
                }
            }

            Tuple t = new Tuple(size,_manager,tupleId);
            return t;
        }

        public void Add(Tuple t)
        {
            if (Identifier != null)
            {
                if (t.Identifier == null)
                {
                    throw new Exception("Cannot add a tuple which does not belong to this collection.");
                }

                if ( t.Identifier.StartsWith(Identifier) == false )
                {
                    throw new Exception("Cannot add a tuple which does not belong to this collection.");
                }
            }

            List.Add(t);
        }

        public void Add(params object[] items)
        {
            Tuple t = CreateTuple(items.Length, null);

            t.InitData(items);

            Add(t);
        }

        /// <summary>
        /// Finds the first tuple whose index value is value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public Tuple Find(object value, int index)
        {
            foreach (Tuple t in List)
            {
                if (t[index] != null)
                {
                    if (t[index].Equals(value))
                    {
                        return t;
                    }
                }
                else if (value == null)
                {
                    return t;
                }
            }

            return null;
        }

        public Tuple Find(object value, string key)
        {
            Check.Require(_manager != null);

            int idx = _manager.GetItemIndex(Identifier, key);

            return Find(value, idx);
        }

        public void Remove(Tuple t)
        {
            List.Remove(t);
        }

        public void Remove(int idx)
        {
            List.RemoveAt(idx);
        }
    }
}
