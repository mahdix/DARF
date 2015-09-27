using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// block1.service1();
    /// block1.serv1({1},{'a'});
    /// block1.serv1(block2.serv2());
    /// </summary>
    class ProcessRequestNode: ScriptNode
    {
        private string blockWebId = null;
        private string blockId = null;
        private string serviceName = null;
        private List<FunctionCall> arguments = null;

        public ProcessRequestNode(ScriptNode node)
            : base(node)
        {
        }

        public ProcessRequestNode(ScriptSection section)
            : base(section)
        {
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            ProcessRequestNode result = new ProcessRequestNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }
        public override void Process()
        {
            //format can be A.B.C() A: BlockWebId, B: BlockId, C: serviceName
            //or B.C(...) 
            int idx1 = processedContents.IndexOf("(");
            int idx2 = processedContents.LastIndexOf(")");

            string args = processedContents.Substring(idx1 + 1, idx2 - idx1 - 1).Trim();
            if (args.Length > 0)
            {
                arguments = Helper.ExtractNestedFunctionCalls(args);
            }

            string firstPart = processedContents.Substring(0, idx1);
            int dotIndex = firstPart.IndexOf(".");
            string firstItem = firstPart.Substring(0, dotIndex);
            firstPart = firstPart.Substring(dotIndex+1).Trim();

            dotIndex = firstPart.IndexOf(".");

            string secondItem = firstPart;

            if (dotIndex != -1)
            {
                secondItem = firstPart.Substring(0, dotIndex);
            }

            dotIndex = firstPart.IndexOf(".");

            if (dotIndex == -1)  //we have only blockId.ServiceName
            {
                blockId = firstItem;
                serviceName = secondItem;
                blockWebId = null;
            }
            else
            {
                string thirdItem = firstPart.Substring(dotIndex+1);

                blockWebId = firstItem;
                blockId = secondItem;
                serviceName = thirdItem;
            }
        }

        public override void Execute(ExecutionContext context)
        {
            object[] args = new object[0];

            if ( arguments != null )
            {
                args = new object[arguments.Count];

                int i=0;
                foreach (FunctionCall call in arguments)
                {
                    args[i++] = processCallArgument(context, call);
                }
            }

            if (blockWebId == null)
            {
                context.CurrentBlockWeb[blockId].ProcessRequest(serviceName, args);
            }
            else
            {
                context.blockWebs[blockWebId][blockId].ProcessRequest(serviceName, args);
            }
        }

        private object processCallArgument(ExecutionContext context, FunctionCall call)
        {
            if (Helper.IsObject(call.Identifier))
            {
                return Helper.ReadObject(call.Identifier);
            }
            else
            {
                string[] blockService = call.Identifier.Split('.');
                string blockId = blockService[0];
                string serviceName = blockService[1];

                object[] args = new object[call.Arguments.Count];

                for(int i=0;i<args.Length;i++)
                {
                    args[i] = processCallArgument(context, call.Arguments[i]);
                }

                return context.CurrentBlockWeb[blockId].ProcessRequest(serviceName, args);
            }
        }
    }
}
