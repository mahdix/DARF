using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DCRF.Primitive
{
    /// <summary>
    /// This class is used to store reference to some data in Block calls.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueTypeHolder<T>
    {
        public ValueTypeHolder(ref T x)
        {
            Value = x;
        }

        public T Value;
    }
}
