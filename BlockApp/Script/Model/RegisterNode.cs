using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// #register <aa.dll>
    /// </summary>
    public class RegisterNode: ScriptNode
    {
        private string assemblyFile = null;

        public RegisterNode(ScriptNode node)
            : base(node)
        {
        }

        public RegisterNode(ScriptSection section)
            : base(section)
        {            
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            RegisterNode result = new RegisterNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }

        public override void Process()
        {
            int temp = 0;
            assemblyFile = Helper.ExtractToken(processedContents, "<", ">", ref temp);
        }

        public override void Execute(ExecutionContext context)
        {
            context.Broker.AddFile(assemblyFile);
        }
    }
}
