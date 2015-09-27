using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{

    public class ReferenceCmd : CommandHandler
    {
        private Identifier templateName = null;
        private TokenList<ObjectOrCall> argValues = null;


        [Rule(@"<ReferenceCmd> ::= ~'[' Identifier ~'(' <OptObjOrCallList> ~')' ~']' ~';'")]
        public ReferenceCmd(Identifier id, Optional<TokenList<ObjectOrCall>> objList)
        {
            templateName = id;
            if (objList.HasValue) argValues = objList.Value;
        }

        public override void Execute()
        {
            DefineCmd cmd = ExecutionContext.Current.LookupTemplate(this.templateName.ValueText);

            List<object> lstValues = new List<object>();

            if (argValues != null)
            {
                foreach (ObjectOrCall ooc in argValues)
                {
                    lstValues.Add(ooc.GetValue());
                }
            }

            cmd.ExecuteTemplate(lstValues);
        }
    }

}
