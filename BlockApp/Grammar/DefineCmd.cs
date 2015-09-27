using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class DefineCmd : CommandHandler
    {
        private DefineSignature signature = null;
        private DefineBody body = null;

        [Rule(@"<DefineCmd> ::= ~'#' ~define <DefSignature> <DefineBody>")]
        public DefineCmd(DefineSignature sig, DefineBody body)
        {
            this.signature = sig;
            this.body = body;
        }


        public override void Execute()
        {
            ExecutionContext.Current.RegisterTemplate(signature.name.ValueText, this);
           
        }

        public void ExecuteTemplate(List<object> argValues)
        {
            ExecutionContext.EnterLevel();
            ExecutionContext.Current.RegisterVariables(signature.GetArgs(), argValues);

            body.Execute();

            ExecutionContext.ExitLevel();
        }
    }

    public class DefineSignature : Token
    {
        public Identifier name = null;
        public Optional<TokenList<Identifier>> args = null;

        [Rule(@"<DefSignature> ::= Identifier ~'(' <OptArgList> ~')'")]
        public DefineSignature(Identifier id, Optional<TokenList<Identifier>> args)
        {
            this.name = id;
            this.args = args;
        }

        public List<Identifier> GetArgs()
        {
            List<Identifier> result = new List<Identifier>();

            if ( args.HasValue )
            {
                foreach(Identifier t in args.Value ) result.Add(t);
            }

            return result;
        }
    }

    public class DefineBody : CommandHandler
    {
        public CommandBlock commandBlock = null;
        //private CommandHandler command = null;
        public Identifier token = null;

        //[Rule(@"<DefineBody> ::= <Command>")]
        //public DefineBody(CommandHandler cmd)
        //{
        //    this.command = cmd;
        //}

        [Rule(@"<DefineBody> ::= <CommandBlock>")]
        public DefineBody(CommandBlock commandBlock)
        {
            this.commandBlock = commandBlock;
        }

        public override void Execute()
        {
            //if (command != null)
            //{
            //    command.Execute();
            //}
            //else
            {
                commandBlock.Execute();
            }
        }
    }
}
