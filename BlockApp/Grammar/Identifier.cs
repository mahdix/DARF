using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    [Terminal("Identifier")]
    [Terminal("FilePathIdentifier")]
    [Terminal("create")]
    [Terminal("abstract")]
    [Terminal("remotable")]
    [Terminal("A")]
    [Terminal("B")]
    [Terminal("I")]
    public class Identifier : Token
    {
        internal readonly string _idName;

        public Identifier()
        {
        }

        public Identifier(string idName)
        {
            _idName = idName;
        }

        public string Id
        {
            get
            {
                return _idName;
            }
        }

        public string ValueText
        {
            get
            {
                return (string)Value;
            }
        }

        public object Value
        {
            get
            {
                if (_idName == null) return null;

                if (ExecutionContext.Current.LookupVariable(_idName) == false)
                {
                    return _idName;
                }

                return ExecutionContext.Current[_idName];
            }
        }
    }
}
