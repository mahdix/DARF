using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// block lblResult: Label;
    /// {
    /// ...
    /// }
    /// at the end of the } after block, its OnAfterLoad will be called
    /// </summary>
    class BlockNode : ScriptNode
    {
        private BlockHandle handle = null;
        private string blockId = null;


        public BlockNode(ScriptNode node)
            : base(node)
        {
        }

        public BlockNode(ScriptSection section)
            : base(section)
        {            
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            BlockNode result = new BlockNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }

        public override void Process()
        {
            List<string> tokens = Helper.ExtractTokens(processedContents, " ", ":");

            blockId = tokens[1];
            handle = new BlockHandle(tokens[2].Replace(";", ""));

            base.Process();
        }

        public override void Execute(ExecutionContext context)
        {
            context.CurrentBlockWeb.AddBlock(handle, blockId);
            
            //maybe we are already inside a block 
            string oldBlockId = context.CurrentBlockId;
            context.CurrentBlockId = blockId;

            //execute child statements
            base.Execute(context);

            context.CurrentBlockId = oldBlockId;

            (context.CurrentBlockWeb[blockId] as IContainedBlock).OnAfterLoad();
        }
    }
}
