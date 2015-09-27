using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class Optional<T> : Token where T : Token
    {
        private readonly T value;

        [Rule(@"<CommandList> ::=", typeof(TokenList<CommandHandler>))]
        [Rule(@"<OptArgList> ::=", typeof(TokenList<Identifier>))]
        [Rule(@"<OptObjOrCallList> ::=", typeof(TokenList<ObjectOrCall>))]
        public Optional()
            : this(null)
        {
        }


        [Rule(@"<CommandList> ::= <Commands>", typeof(TokenList<CommandHandler>))]
        [Rule(@"<OptArgList> ::= <ArgList>", typeof(TokenList<Identifier>))]
        [Rule(@"<OptObjOrCallList> ::= <ObjOrCallList>", typeof(TokenList<ObjectOrCall>))]
        public Optional(T value)
        {
            this.value = value;
        }


        public T Value
        {
            get
            {
                return value;
            }
        }

        public bool HasValue
        {
            get
            {
                return value != null;
            }
        }
    }
}
