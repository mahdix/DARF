using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;

namespace BlockApp.Script.Model
{
    public class IfNode: ScriptNode
    {
        private string conditionArgument = null;
        private string conditionOperator = null;
        private string conditionTarget = null;

        public IfNode(ScriptNode node)
            : base(node)
        {
        }

        public IfNode(ScriptSection section): base(section)
        {
        }

        public override void Process()
        {
            string line = processedContents.Replace("#if", "").Trim().TrimStart('(').TrimEnd(')').Trim();

            if (line.Contains("=="))
            {
                conditionOperator = "==";
            }
            else
            {
                conditionOperator = "!=";
            }

            string[] parts = line.Split(new string[]{conditionOperator}, StringSplitOptions.None);
            conditionArgument = parts[0].Trim();
            conditionTarget = parts[1].Trim();

            base.Process();
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            IfNode result = new IfNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }

        public override void Execute(ExecutionContext context)
        {
            bool conditionResult = false;

            switch (conditionOperator)
            {
                case "==":
                {
                    conditionResult = (conditionArgument == conditionTarget);
                    break;
                }
                case "!=":
                {
                    conditionResult = (conditionArgument != conditionTarget);
                    break;
                }
            }

            if ( conditionResult )
            {
                base.Execute(context);
            }
        }

    }
}
