using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Grammar
{
    public class BlockWebCmd : CommandHandler
    {
        //private Identifier ownerBlockId = null;
        private CommandBlock commandBlock = null;
        private BlockWebImport import = null;
        private Identifier name = null;
        private BlockWebSuffix suffix = null;

        [Rule(@"<BlockWebCmd> ::= ~blockWeb Identifier <BlockWebImport> <BlockWebSuffix> <CommandBlock>")]
        public BlockWebCmd(Identifier name, BlockWebImport import, BlockWebSuffix suffix, CommandBlock commandBlock)
        {
            this.name = name;
            this.commandBlock = commandBlock;
            this.import = import;
            this.suffix = suffix;

            ExecutionContext.Current.RegisterBlockWebDefinition(name.ValueText, this);
        }

        public BlockCmd LookupBlockCmd(string identifier)
        {
            if (commandBlock.GetCommands() != null)
            {
                foreach (CommandHandler cmd in commandBlock.GetCommands())
                {
                    if (cmd is BlockCmd && (cmd as BlockCmd).Identifier == identifier)
                    {
                        return (cmd as BlockCmd);
                    }
                }
            }

            return import.LookupBlockCmd(identifier);
        }

        public override void Execute()
        {
            //template blockWebs are not executed, they are just imported
            if (suffix != null && suffix.IsAbstract) return;

            IBlockWeb result = null;
            ExecutionContext.EnterLevel();

            result = new BlockWeb(name.ValueText, ExecutionContext.Current.DefaultBroker, null, DCRF.Definition.PlatformType.Neutral,
                null, suffix.IsRemotable);
            ExecutionContext.Current.RegisterBlockWeb(result, false);

            innerExecute(result);

            ExecutionContext.ExitLevel();
        }

        public void innerExecute(IBlockWeb result)
        {
            import.DoImport(result);

            commandBlock.Execute();
        }
    }

    public class BlockWebImport : Token
    {
        private TokenList<Identifier> importList = null;

        [Rule(@"<BlockWebImport> ::= ~'[' <ArgList> ~']'")]
        public BlockWebImport(TokenList<Identifier> importList)
        {
            this.importList = importList;
        }

        [Rule(@"<BlockWebImport> ::= ")]
        public BlockWebImport()
        {
        }

        public void DoImport(IBlockWeb baseBlockWeb)
        {
            if (importList == null) return;

            foreach (Identifier import in importList)
            {
                BlockWebCmd bwCmd = ExecutionContext.Current.LookupBlockWebDefinition(import.ValueText);

                if (bwCmd == null)
                {
                    throw new Exception("Cannot find reference block-web: " + import.ValueText);
                }

                bwCmd.innerExecute(baseBlockWeb);
            }
        }

        public BlockCmd LookupBlockCmd(string identifier)
        {
            if (importList == null) return null;

            foreach (Identifier import in importList)
            {
                BlockWebCmd bwCmd = ExecutionContext.Current.LookupBlockWebDefinition(import.ValueText);

                BlockCmd result = bwCmd.LookupBlockCmd(identifier);

                if (result != null) return result;
            }

            return null;
        }
    }

    public class BlockWebSuffix : Token
    {
        private bool isAbstract = false;
        private bool isRemotable = false;

        [Rule(@"<BlockWebSuffix> ::= ':' abstract")]
        [Rule(@"<BlockWebSuffix> ::= ':' remotable")]
        public BlockWebSuffix(Token prefix, Identifier suffix)
        {
            if (suffix.ValueText == "abstract")
            {
                isAbstract = true;
            }
            if (suffix.ValueText == "remotable")
            {
                isRemotable = true;
            }
        }

        [Rule(@"<BlockWebSuffix> ::= ':' abstract ~',' remotable")]
        public BlockWebSuffix(Token prefix, Token suffix1, Token suffix2)
        {
            isAbstract = true;
            isRemotable = true;
        }

        [Rule(@"<BlockWebSuffix> ::=")]
        public BlockWebSuffix()
        {
        }

        public bool IsAbstract
        {
            get
            {
                return isAbstract;
            }
        }

        public bool IsRemotable
        {
            get
            {
                return isRemotable;
            }
        }
    }
}
