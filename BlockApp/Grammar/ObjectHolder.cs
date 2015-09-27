using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public abstract class ObjectHolder : Token
    {
        internal readonly string _idName;

        public ObjectHolder(string symbol)
        {
            _idName = symbol;
        }

        public string ValueText
        {
            get
            {
                return _idName;
            }
        }

        public virtual object Value
        {
            get
            {
                if (ExecutionContext.Current.LookupVariable(_idName) == false)
                {
                    return _idName;
                }

                return ExecutionContext.Current[_idName];
            }
        }
    }

    [Terminal("Integer")]
    public class IntegerTerminal : ObjectHolder
    {
        public IntegerTerminal(string symbol) : base(symbol) { }

        public override object Value
        {
            get
            {
                return int.Parse(base.Value.ToString());
            }
        }
    }

    [Terminal("GeneralObject")]
    public class GeneralObjectTerminal : ObjectHolder
    {
        public GeneralObjectTerminal(string symbol) : base(symbol) { }

        public override object Value
        {
            get
            {
                string txt = _idName;

                //e.g. System.String[]:40%:80%
                string[] items = txt.Substring(1, txt.Length - 2).Split(':');

                Type arrType = Type.GetType(items[0]);
                Type elementType = arrType.GetElementType();

                Array typedArray = Array.CreateInstance(elementType, items.Length - 1);

                for (int i = 1; i < items.Length; i++)
                {
                    string item = items[i];
                    object argResult = item;

                    if (ExecutionContext.Current.LookupVariable(item))
                    {
                        argResult = ExecutionContext.Current[item];
                    }

                    if (argResult != null)
                    {
                        typedArray.SetValue(argResult, i - 1);
                    }
                    else
                    {
                        typedArray.SetValue(Convert.ChangeType(item, elementType), i - 1);
                    }
                }

                return typedArray;

            }
        }
    }

    [Terminal("StringLiteral")]
    public class StringTerminal : ObjectHolder
    {
        private string innerValue = null;

        public StringTerminal(string symbol) : base(symbol) 
        {
        }

        public override object Value
        {
            get
            {
                if (innerValue == null)
                {
                    string refined = base.Value.ToString();

                    //remove start/and single quote
                    innerValue = refined.Substring(1, refined.Length - 2);
                }

                return innerValue;
            }
        }
    }


    [Terminal("null")]
    public class NullTerminal : ObjectHolder
    {
        public NullTerminal(string symbol)
            : base(symbol)
        {
        }

        public override object Value
        {
            get
            {
                return null;
            }
        }
    }

    [Terminal("true")]
    [Terminal("false")]
    public class BoolTerminal : ObjectHolder
    {
        public BoolTerminal(string symbol)
            : base(symbol)
        {
        }

        public override object Value
        {
            get
            {
                return this._idName == "true" ? true: false;
            }
        }
    }

}
