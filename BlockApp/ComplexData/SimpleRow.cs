using System;
using System.Collections.Generic;
using System.Text;

namespace DCRF.ComplexData
{
    public class SimpleRow
    {
        private SimpleTable parent = null;
        private List<object> values = new List<object>();

        public SimpleRow(SimpleTable p)
        {
            parent = p;
        }

        public SimpleRow Clone()
        {
            SimpleRow row = new SimpleRow(this.parent);
            row.values.InsertRange(0, this.values);

            return row;
        }

        public SimpleTable Parent
        {
            get
            {
                return parent;
            }
        }

        public object this[int index]
        {
            get
            {
                if (parent != null) DBC.Check.Require(index < parent.Keys.Count);
                DBC.Check.Require(index != -1);

                if (index < values.Count)
                {
                    return values[index];
                }

                return null;
            }
            set
            {
                if (parent != null) DBC.Check.Require(index < parent.Keys.Count);
                DBC.Check.Require(index != -1);

                Type t = null;

                if (parent != null)
                {
                    t = parent.Types[index];
                }

                //if (value != null && !value.GetType().Equals(t))
                //{
                //    throw new Exception("Invalid type for " + parent.Keys[index]);
                //}

                if (index >= values.Count)
                {
                    for (int i = values.Count; i <= index; i++)
                    {
                        values.Add(null);
                    }
                }

                if (value == null)
                {
                    values[index] = null;
                }
                else
                {
                    if (parent != null)
                    {
                        values[index] = Convert.ChangeType(value, t); ;
                    }
                    else
                    {
                        values[index] = value;
                    }
                }
            }
        }

        public object this[string key]
        {
            get
            {
                DBC.Check.Require(parent != null);
                int index = parent.Keys.IndexOf(key);

                return this[index];
            }
            set
            {
                DBC.Check.Require(parent != null);
                int index = parent.Keys.IndexOf(key);

                this[index] = value;
            }
        }
    }
}
