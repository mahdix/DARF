using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// e.g. #ttp1(x, y, z);
    /// or
    /// ttp1;
    /// </summary>
    class ReferenceNode : ScriptNode
    {
        private List<string> argValues = new List<string>();
        private string templateName = null;
        private DefineNode targetTemplate = null;

        public ReferenceNode(ScriptNode node)
            : base(node)
        {
        }

        public ReferenceNode(ScriptSection section)
            : base(section)
        {
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            ReferenceNode result = new ReferenceNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }


        public override void Process()
        {
            //TODO: debug
            List<string> tokens = Helper.ExtractTokens(processedContents, "{", "(", ")", "}" , ";");
            templateName = tokens[0];

            if (tokens.Count > 1)        //we have an argument list too
            {
                string argsString = tokens[1];
                string[] args = argsString.Split(',');

                foreach (string arg in args)
                {
                    argValues.Add(arg.Trim());
                }
            }

            targetTemplate = TemplateManager.GetInstance().LookupTemplate(templateName);

            List<ScriptNode> myLines = targetTemplate.Instantiate(argValues);

            foreach (ScriptNode child in myLines)
            {
                children.Add(child);
            }

            base.Process();
        }

        public override void Execute(ExecutionContext context)
        {
            base.Execute(context);
        }
    }
}
