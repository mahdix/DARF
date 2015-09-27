using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Grammar
{
    public class BlockCmd : CommandHandler
    {
        private Identifier id = null;
        private BlockHandleToken handle = null;  //either we have a handle or a block import
        private CommandBlock commandBlock = null;
        private BlockImport import = null;
        private BlockWebImport webImport = null;
        private BlockRemotableType blockSuffix = null;

        [Rule(@"<BlockCmd> ::= ~block Identifier ~':' <BlockHandle> <BlockWebImport> <BlockRemotableType> <CommandBlock>")]
        public BlockCmd(Identifier id, BlockHandleToken handle, BlockWebImport webImport, BlockRemotableType blockSuffix, CommandBlock commandBlock)
        {
            this.id = id;
            this.handle = handle;
            this.commandBlock = commandBlock;
            this.webImport = webImport;
            this.blockSuffix = blockSuffix;
        }

        [Rule(@"<BlockCmd> ::= ~block Identifier <BlockImport> <BlockWebImport> <CommandBlock>")]
        public BlockCmd(Identifier id, BlockImport import, BlockWebImport webImport, CommandBlock commandBlock)
        {
            this.id = id;
            this.import = import;
            this.commandBlock = commandBlock;
            this.webImport = webImport;
        }

        public string Identifier
        {
            get
            {
                return id.ValueText;
            }
        }

        public override void Execute()
        {
            ExecutionContext.EnterLevel();

            innerExecute(id.ValueText);

            ExecutionContext.ExitLevel();
        }

        public void innerExecute(string identifier)
        {
            if (import != null)
            {
                import.DoImport(identifier);
            }
            else if (handle != null)
            {
                BlockBase.MakeInnerWebsRemotable = (blockSuffix != null && blockSuffix.IsRemotable);

                ExecutionContext.Current.ActiveBlockWeb.AddBlock(handle.Handle, identifier);
                ExecutionContext.Current.ActiveBlockId = identifier;
            }
            else
            {
                throw new Exception("You need to either specify a base block or a handle for block definition");
            }

            IBlockWeb innerWeb = (IBlockWeb)ExecutionContext.Current.ActiveBlockWeb[identifier].ProcessRequest(
                                    "ProcessMetaService", BlockMetaServiceType.GetInnerWeb, null, null);
            ExecutionContext.Current.RegisterBlockWeb(innerWeb, true);

            if (webImport != null)
            {
                webImport.DoImport(innerWeb);
            }

            commandBlock.Execute();
        }
    }

    public class BlockImport : Token
    {
        private Identifier blockWebId = null;
        private Identifier blockId = null;

        [Rule(@"<BlockImport> ::= ~':' Identifier ~'.' Identifier")]
        public BlockImport(Identifier blockWebId, Identifier blockId)
        {
            this.blockWebId = blockWebId;
            this.blockId = blockId;
        }

        public void DoImport(string identifier)
        {
            BlockWebCmd blockWebCmd = ExecutionContext.Current.LookupBlockWebDefinition(blockWebId.ValueText);
            BlockCmd blockCmd = blockWebCmd.LookupBlockCmd(blockId.ValueText);

            if (blockCmd == null)
            {
                throw new Exception("Cannot find block to import: " + blockWebId.ValueText + "." + blockId.ValueText);
            }

            blockCmd.innerExecute(identifier);
        }
    }

    public class BlockHandleToken : Token
    {
        private Identifier classNameOrHandle = null;
        private Identifier productName = null;
        private Identifier versionMajor = null;
        private Identifier versionMinor = null;
        private Identifier versionBuild = null;
        private Identifier versionRevision = null;


        //Here identifier can be a string containing class name
        //or be an identifier object which contains a complete block handle
        [Rule(@"<BlockHandle> ::= Identifier")]
        public BlockHandleToken(Identifier classNameOrHandle)
        {
            this.classNameOrHandle = classNameOrHandle;
        }

        [Rule(@"<BlockHandle> ::= Identifier ~'(' Identifier ~')'")]
        public BlockHandleToken(Identifier className, Identifier productName)
        {
            this.classNameOrHandle = className;
            this.productName = productName;
        }

        [Rule(@"<BlockHandle> ::= Identifier ~'(' Identifier ~',' Identifier ~'.' Identifier ~'.' Identifier ~'.' Identifier ~')'")]
        public BlockHandleToken(Identifier className, Identifier productName, 
            Identifier versionMajor, Identifier versionMinor, 
            Identifier versionBuild, Identifier versionRevision)
        {
            this.classNameOrHandle = className;
            this.productName = productName;
            this.versionMajor = versionMajor;
            this.versionMinor = versionMinor;
            this.versionBuild = versionBuild;
            this.versionRevision = versionRevision;
        }


        public BlockHandle Handle
        {
            get
            {
                if (productName == null)
                {
                    //use className identifier. It may be a class name string
                    //or a blockHandle object
                    if (classNameOrHandle.Value is BlockHandle)
                    {
                        return classNameOrHandle.Value as BlockHandle;
                    }

                    return new BlockHandle(classNameOrHandle.ValueText);
                }
                else if (versionMajor == null)
                {
                    return new BlockHandle(classNameOrHandle.ValueText, productName.ValueText);
                }

                int vm = int.Parse(versionMajor.ValueText);
                int vmn = int.Parse(versionMinor.ValueText);
                int vb = int.Parse(versionBuild.ValueText);
                int vr = int.Parse(versionRevision.ValueText);

                BlockVersion version = new BlockVersion(vm, vmn, vb, vr);

                return new BlockHandle(classNameOrHandle.ValueText, version, productName.ValueText);
            }

        }
    }

    public class BlockRemotableType : Token
    {
        private bool isRemotable = false;

        [Rule(@"<BlockRemotableType> ::= remotable")]
        public BlockRemotableType(Identifier remotable)
        {
            this.isRemotable = true;
        }

        [Rule(@"<BlockRemotableType> ::=")]
        public BlockRemotableType()
        {
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
