using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class WhileCmd: CommandHandler
    {
        private Condition condition = null;
        private CommandBlock commandBlock = null;
        private bool isDoWhile = false;

        [Rule(@"<WhileCmd> ::= ~while <Condition> <CommandBlock>")]
        public WhileCmd(Condition condition, CommandBlock commandBlock)
        {
            this.condition = condition;
            this.commandBlock = commandBlock;
        }

        [Rule(@"<WhileCmd> ::= ~do <CommandBlock> ~while <Condition> ~';'")]
        public WhileCmd(CommandBlock commandBlock, Condition condition)
        {
            this.condition = condition;
            this.commandBlock = commandBlock;
            isDoWhile = true;
        }

        public override void Execute()
        {

            if (isDoWhile)
            {
                do
                {
                    ExecutionContext.EnterLevel();
                    commandBlock.Execute();
                    if (ExecutionContext.Current.BreakLoopFlag) break; ;
                    ExecutionContext.ExitLevel();
                } while (condition.Evaluate() == true);
            }
            else
            {
                while (condition.Evaluate() == true)
                {
                    ExecutionContext.EnterLevel();
                    commandBlock.Execute();
                    if (ExecutionContext.Current.BreakLoopFlag) break; ;
                    ExecutionContext.ExitLevel();
                }
            }
        }
    }
}
