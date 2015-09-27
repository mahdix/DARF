using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class BreakCmd: CommandHandler
    {
        [Rule(@"<BreakCmd> ::= ~break ~';'")]
        public BreakCmd()
        {
        }

        public override void Execute()
        {
            ExecutionContext.Current.BreakLoopFlag = true;
        }
    }
}
