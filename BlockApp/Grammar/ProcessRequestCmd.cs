using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class ProcessRequestCmd : CommandHandler
    {
        private AddressHandler serviceAddress = null;
        private TokenList<ObjectOrCall> objOrCallList = null;

        [Rule(@"<ProcessRequestCmd> ::=  <Address> ~'(' <OptObjOrCallList> ~')' ~';' ")]
        public ProcessRequestCmd(AddressHandler serviceAddress, Optional<TokenList<ObjectOrCall>> objOrCallList)
        {
            this.serviceAddress = serviceAddress;
            this.objOrCallList = objOrCallList.Value;
        }

        public override void Execute()
        {
            List<object> args = null;

            if (objOrCallList != null)
            {
                args = new List<object>();

                foreach (ObjectOrCall ooc in objOrCallList)
                {
                    args.Add(ooc.GetValue());
                }
            }

            if (args != null)
            {
                serviceAddress.ProcessRequest(args.ToArray());
            }
            else
            {
                serviceAddress.ProcessRequest();
            }

        }

    }
}
