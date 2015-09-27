using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    class ContinueCmd: CommandHandler
    {
        [Rule(@"<ContinueCmd> ::= ~continue ~';'")]
        public ContinueCmd()
        {
        }

        public override void Execute()
        {
            ExecutionContext.Current.ContinueLoopFlag = true;
        }
    }
}
