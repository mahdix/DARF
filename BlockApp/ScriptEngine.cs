using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using BlockApp.Grammar;
using BlockBroker;
using bsn.GoldParser.Grammar;
using bsn.GoldParser.Semantic;
using DCRF.Interface;

namespace BlockApp
{
    public class ScriptEngine
    {
        private static SemanticTypeActions<Token> grammarActions = null;
        public static string DefaultBlocksPath = null;

        private static void init()
        {
            Stream resourceStream = typeof(ScriptEngine).Assembly.GetManifestResourceStream("BlockApp.Grammar.src.BlockAppGrammar.egt");
            CompiledGrammar grammar = CompiledGrammar.Load(resourceStream);
            grammarActions = new SemanticTypeActions<Token>(grammar);

            try
            {
                grammarActions.Initialize(true);
            }
            catch (InvalidOperationException ex)
            {
                throw ex;
            }
        }

        public static void Execute(string filePath)
        {
            if (grammarActions == null)
            {
                init();
            }

            ExecutionContext.Reset();

            //create default base context
            ExecutionContext.EnterLevel();
            (ExecutionContext.Current.DefaultBroker as FileBlockBroker).SetLibFolder(DefaultBlocksPath);
            
            ScriptReader reader = new ScriptReader(filePath);
            SemanticProcessor<Token> processor = new SemanticProcessor<Token>(reader, grammarActions);
            ParseMessage parseMessage = processor.ParseAll();

            reader.Close();

            if (parseMessage != ParseMessage.Accept)
            {
                string newLine = "\n";
                string errorMessage = parseMessage.ToString()+".";
                string location = string.Format("Expecting one of following tokens at Line {0}, Column {1}:", processor.CurrentToken.Line, processor.CurrentToken.Column);
                string symbols = "";
                foreach (Symbol s in processor.GetExpectedTokens())
                {
                    string txt = "\t'" + s.Name + "' of type " + s.Kind.ToString()+newLine;
                    symbols += txt;
                }

                throw new Exception(errorMessage+newLine+location+newLine+symbols);
            }
            
            Optional<TokenList<CommandHandler>> optCommands = processor.CurrentToken as Optional<TokenList<CommandHandler>>;
            
            if (optCommands.HasValue)
            {
                TokenList<CommandHandler> commands = optCommands.Value;

                foreach (CommandHandler command in commands)
                {
                    command.Execute();
                }
            }
        }
    }    
}
