using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script.Model
{
    // #define x 'a';	//inline parameterless template - inline templates have no parameter
    // #define ttp1(v1)  //v1 is an input argument - it is mentioned just for readability)
    // {
	//    //some code
	//    block pp1: v1;  //here v1 is an argument
    // }
    public class DefineNode : ScriptNode
    {
        //private string templateName = null;
        private List<string> args = new List<string>();
        //private List<string> templateBody = new List<string>();

        public DefineNode(ScriptNode node)
            : base(node)
        {
        }

        public DefineNode(ScriptSection section)
            : base(section)
        {
        }

        public override void PreProcess()
        {
            string templateName = null;
            string line = processedContents;
            line = line.Replace("#define", "").Trim();

            //now line is: tto(x, y, z) x+ y+ z;
            int idx1 = line.IndexOf(" ");
            int idx2 = line.IndexOf("(");

            if (idx2 != -1 && idx2 < idx1)  //template has some parameters
            {
                templateName = line.Substring(0, idx2).Trim();

                int idx3 = line.IndexOf(")");
                string[] tokens = line.Substring(idx2 + 1, idx3 - idx2 - 1).Trim().Split(',');

                foreach (string arg in tokens)
                {
                    args.Add(arg.Trim());
                }
            }
            else
            {
                templateName = line.Substring(0, idx1).Trim();
            }

            TemplateManager.GetInstance().RegisterTemplate(templateName, this);
        }

        public override void Process()
        {
            base.Process();
        }

        public override void Execute(ExecutionContext context)
        {
            //do not execute children - this is a special statement
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            throw new Exception("Why are you creating a clone of a #define statement?");
        }

        public string InstantiateString(List<string> argValues)
        {
            if (children.Count > 0) throw new Exception("Cannot instantiate a complex template");

            //return processedContents with arguments replaced with values
            return processedContents;
        }

        public List<ScriptNode> Instantiate(List<string> argValues)
        {
            if (argValues.Count != args.Count)
            {
                throw new Exception("Expecting " + args.Count.ToString() + " arguments but received " + argValues.Count.ToString());
            }

            List<ScriptNode> result = new List<ScriptNode>();
            Dictionary<string, string> arguments = new Dictionary<string,string>();
            
            for(int i=0;i<args.Count;i++)
            {
                arguments[args[i]] = argValues[i];
            }

            foreach(ScriptNode child in children)
            {
                result.Add(child.CloneRaw(arguments));
            }

            return result;
        }
    }
}
