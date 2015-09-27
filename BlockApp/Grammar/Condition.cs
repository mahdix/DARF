using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class Condition: Token
    {
        private AddressHandler serviceAddress = null;
        private Optional<TokenList<ObjectOrCall>> objOrCallList = null;
        private bool isNegate = false;

        [Rule(@"<Condition> ::= ~'(' <Address> ~'(' <OptObjOrCallList> ~')' ~')'")]
        public Condition(AddressHandler serviceAddress, Optional<TokenList<ObjectOrCall>> objOrCallList)
        {
            this.serviceAddress = serviceAddress;
            this.objOrCallList = objOrCallList;
        }

        [Rule(@"<Condition> ::= ~'(' '~' <Address> ~'(' <OptObjOrCallList> ~')' ~')'")]
        public Condition(Token negate, AddressHandler serviceAddress, Optional<TokenList<ObjectOrCall>> objOrCallList)
        {
            this.serviceAddress = serviceAddress;
            this.objOrCallList = objOrCallList;
            isNegate = true;
        }

        public bool Evaluate()
        {
            bool isSatisfied = false;

            List<object> args = null;

            if (objOrCallList != null && objOrCallList.HasValue)
            {
                args = new List<object>();

                foreach (ObjectOrCall ooc in objOrCallList.Value)
                {
                    args.Add(ooc.GetValue());
                }
            }

            if (args != null)
            {
                isSatisfied = (bool)serviceAddress.ProcessRequest(args.ToArray());
            }
            else
            {
                isSatisfied = (bool)serviceAddress.ProcessRequest();
            }

            if (isNegate) return !isSatisfied;

            return isSatisfied;
        }
    }
}
