using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// blockWeb {frmMain} //this bw is innerweb of frmMain block
    /// {
    /// ...
    /// }
    /// 
    /// blockWeb testForm
    /// {
    /// ...
    /// }
    /// </summary>
    class BlockWebNode : ScriptNode
    {
        private string webId = null;
        private string ownerBlockId = null;

        public BlockWebNode(ScriptNode node)
            : base(node)
        {
        }

        public BlockWebNode(ScriptSection section)
            : base(section)
        {
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            BlockWebNode result = new BlockWebNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }

        public override void Process()
        {
            if (processedContents.Contains("{"))
            {
                List<string> tokens = Helper.ExtractTokens(processedContents, " ", "{", "}");
                ownerBlockId = tokens[1];
            }
            else
            {
                List<string> tokens = Helper.ExtractTokens(processedContents, " ");
                webId = tokens[1];
            }

            base.Process();
        }

        public override void Execute(ExecutionContext context)
        {
            if (context.CurrentBlockWeb != null && ownerBlockId == null)
            {
                throw new Exception("You cannot nest standalone blockWebs");
            }

            IBlockWeb newWeb = null;
            IBlockWeb oldWeb = context.CurrentBlockWeb;

            if (ownerBlockId != null)
            {
                newWeb = (IBlockWeb)context.CurrentBlockWeb[ownerBlockId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.GetInnerWeb,
                    null, null);
            }
            else
            {
                newWeb = new BlockWeb(webId, context.Broker);
            }

            context.CurrentBlockWeb = newWeb;

            if (webId != null)
            {
                context.blockWebs[webId] = newWeb;
            }

            base.Execute(context);

            context.CurrentBlockWeb = oldWeb;
        }
    }
}
