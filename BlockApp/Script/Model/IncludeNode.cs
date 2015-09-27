using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script.Model
{
    class IncludeNode : ScriptNode
    {
        public IncludeNode(ScriptNode node)
            : base(node)
        {
        }

        public IncludeNode(ScriptSection section)
            : base(section)
        {
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            throw new Exception("Why are you cloning an #include statement?");
        }

        public override void Process()
        {
        }

        public override void Execute(ExecutionContext context)
        {
            base.Execute(context);
        }
    }
}
