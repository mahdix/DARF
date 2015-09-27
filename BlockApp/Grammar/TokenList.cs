using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class TokenList<T> : Token, IEnumerable<T> where T : Token
    {
        private readonly T item;
        private readonly TokenList<T> next;

        [Rule(@"<Commands> ::= <Command> <Commands>", typeof(CommandHandler))]
        [Rule(@"<Commands> ::= <Preprocessor> <Commands>", typeof(CommandHandler))]
        [Rule(@"<ArgList> ::= Identifier ~',' <ArgList>", typeof(Identifier))]
        [Rule(@"<ObjOrCallList> ::= <ObjOrCall> ~',' <ObjOrCallList>", typeof(ObjectOrCall))]
        [Rule(@"<DefBlockServiceBody> ::= <DefBlockServiceLine> <DefBlockServiceBody>", typeof(DefBlockServiceLine))]
        [Rule(@"<DefBlockBody> ::=  <DefBlockBodyItem> <DefBlockBody>", typeof(DefBlockBodyItem))]
        //[Rule(@"<BlockImportList> ::= <Address> ~',' <BlockImportList>", typeof(AddressHandler))]
        //[Rule(@"<AttachSuffixList> ::= ~'[' create ~']' <AttachSuffixList>", typeof(Identifier))]
        //[Rule(@"<AttachSuffixList> ::= ~'[' single ~']' <AttachSuffixList>", typeof(Identifier))]
        public TokenList(T item, TokenList<T> next)
        {
            this.item = item;
            this.next = next;
        }

        [Rule(@"<Commands> ::= <Command>", typeof(CommandHandler))]
        [Rule(@"<Commands> ::= <Preprocessor>", typeof(CommandHandler))]
        [Rule(@"<ArgList> ::= Identifier", typeof(Identifier))]
        [Rule(@"<ObjOrCallList> ::= <ObjOrCall>", typeof(ObjectOrCall))]
        [Rule(@"<DefBlockServiceBody> ::= <DefBlockServiceLine>", typeof(DefBlockServiceLine))]
        [Rule(@"<DefBlockBody> ::=  <DefBlockBodyItem>", typeof(DefBlockBodyItem))]
        //[Rule(@"<BlockImportList> ::= <Address>", typeof(AddressHandler))]
        //[Rule(@"<AttachSuffixList> ::= ~'[' create ~']'", typeof(Identifier))]
        //[Rule(@"<AttachSuffixList> ::= ~'[' single ~']'", typeof(Identifier))]
        public TokenList(T item)
            : this(item, null)
        {
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            for (TokenList<T> sequence = this; sequence != null; sequence = sequence.next)
            {
                if (sequence.item != null)
                {
                    yield return sequence.item;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

    }
}
