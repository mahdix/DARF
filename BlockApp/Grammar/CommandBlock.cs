using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class CommandBlock : CommandHandler
    {
        private Optional<TokenList<CommandHandler>> commands = null;
        private CommandHandler cmdHandler = null;

        [Rule(@"<CommandBlock> ::= ~';'")]
        public CommandBlock()
        {
        }

        [Rule(@"<CommandBlock> ::= ~'{' <CommandList> ~'}'")]
        public CommandBlock(Optional<TokenList<CommandHandler>> commands)
        {
            this.commands = commands;
        }

        [Rule(@"<CommandBlock> ::= <Command>")]
        public CommandBlock(CommandHandler cmdHandler)
        {
            this.cmdHandler = cmdHandler;
        }

        public TokenList<CommandHandler> GetCommands()
        {
            if (commands != null) return commands.Value;
            if (cmdHandler != null) return new TokenList<CommandHandler>(cmdHandler);

            return null;
        }

        public override void Execute()
        {
            if (commands != null && commands.Value != null)
            {
                foreach (CommandHandler cmd in commands.Value)
                {
                    cmd.Execute();

                    if (ExecutionContext.Current.ContinueLoopFlag || ExecutionContext.Current.BreakLoopFlag)
                    {
                        return;
                    }
                }
            }
            else if (cmdHandler != null)
            {
                cmdHandler.Execute();
            }
        }
    }
}
