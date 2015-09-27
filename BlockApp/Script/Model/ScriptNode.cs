using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script.Model
{
    public abstract class ScriptNode
    {
        protected string rawContents = "";          //e.g. cmdOK.Text = #hvalue;
        protected string processedContents = null;  //e.g. cmdOK.Text = 'Hello';

        protected List<ScriptNode> children = new List<ScriptNode>();

        public ScriptNode(ScriptNode clone)
        {
            processedContents = clone.processedContents;
        }

        public ScriptNode(ScriptSection section)
        {
            rawContents = section.Contents;

            foreach (ScriptSection child in section.Children)
            {
                children.Add(child.CreateScriptModel());
            }
        }

        public virtual void PreProcess()
        {
            //process rawContents and build processedContents: this method is for inline templates
            //e.g. cmdOK.Text = {def1};  inline template
            //e.g. cmdOK.Text = {def1('a');}  inline template
            //not #action1; as a whole line because this is a referenceNode 
            //although a referenceNode may also contains inline templates. e.g. 
            //#action1(#hValue);

            //here we find inline templates by checking every token of the rawContents and replace their value with one provided
            //from templateManager's lookup method

            int startIndex = 1;
            processedContents = rawContents;

            //what about cmdok.text = {def1({def2})};?
            //currently nested templates are not supported
            string token = Helper.ExtractTokenMatch(processedContents, '{', '}', ref startIndex);

            while (token != null)
            {
                List<FunctionCall> fCalls = Helper.ExtractNestedFunctionCalls(token);  //extract arguments
                FunctionCall templateCall = fCalls[0];

                DefineNode template = TemplateManager.GetInstance().LookupTemplate(templateCall.Identifier);

                if (template != null)       //maybe it is an object notation. in this case no template shall be found
                {
                    if (template.children.Count > 0)
                    {
                        throw new Exception("You cannot reference a complex template inside a statement. Complex templates needs their own reference statement.");
                    }

                    List<string> argValues = new List<string>();

                    foreach(FunctionCall fc in templateCall.Arguments)
                    {
                        argValues.Add(fc.Identifier);
                    }

                    //now we know that template has no children
                    //get the result string according to template text and given argument values
                    string concreteTemplate = template.InstantiateString(argValues);

                    //replace
                    int originalStartIndex = startIndex - token.Length;
                    processedContents = processedContents.Substring(0, originalStartIndex) + concreteTemplate + processedContents.Substring(startIndex);
                }

                token = Helper.ExtractTokenMatch(processedContents, '{', '}', ref startIndex);
            }

            foreach (ScriptNode child in children) child.PreProcess();
        }

        /// <summary>
        /// for rootNode input is null and output is result
        /// for others input=output=current web
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual void Process()
        {
            foreach (ScriptNode child in children)
            {
                child.Process();
            }
        }

        //create a clone of the current node with given values for arguments
        //e.g. if blockName is #bb1 and arguments["#bb1"] is XXX then replace accordingly
        //Hint: use Helper.ProcessTemplateLine(templateLine, args, argValues)
        //TODO: if no argument in arguments is used during cloning process, you can easily
        //return a reference to 'this'
        //CloneRaw means clone without any processing. Caller will do the rest.
        public abstract ScriptNode CloneRaw(Dictionary<string, string> arguments);

        public virtual void Execute(ExecutionContext context)
        {
            foreach (ScriptNode child in children)
            {
                child.Execute(context);
            }
        }

        public override string ToString()
        {
            return processedContents;
        }
    }
}
