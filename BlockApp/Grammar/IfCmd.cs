using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class IfCmd : CommandHandler
    {
        private Condition condition = null;
        private CommandBlock ifBlock = null;
        private CommandBlock elseBlock = null;

        [Rule(@"<IfCmd> ::= ~if <Condition> <CommandBlock> ")]
        public IfCmd(Condition condition, CommandBlock ifBlock)
        {
            this.condition = condition;
            this.ifBlock = ifBlock;
        }

        [Rule(@"<IfCmd> ::= ~if <Condition> <CommandBlock> ~else <CommandBlock>")]
        public IfCmd(Condition condition, CommandBlock ifBlock, CommandBlock elseBlock)
        {
            this.condition = condition;
            this.ifBlock = ifBlock;
            this.elseBlock = elseBlock;
        }

        public override void Execute()
        {
            bool isSatisfied = condition.Evaluate();

            if (isSatisfied)
            {
                if (ifBlock != null)
                {
                    ifBlock.Execute();
                }
            }

            if (!isSatisfied && elseBlock != null)
            {
                elseBlock.Execute();
            }
        }
    }
}
