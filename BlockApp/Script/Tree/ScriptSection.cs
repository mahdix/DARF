using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Model;

namespace BlockApp.Script.Tree
{
    public class ScriptSection
    {
        public ScriptSection()
        {
        }

        private string contents = null;
        public string Contents
        {
            get
            {
                return contents;
            }
            set
            {
                contents = value;
                Type = Helper.DetermineElementType(contents);

                if (this.Type == ScriptElementType.Define)
                {
                    processDefineSection();
                }
                else if (Type == ScriptElementType.Include)
                {
                    processIncludeSection();
                }
            }
        }

        private void processIncludeSection()
        {
            int temp = 0;
            string includeFile = Helper.ExtractToken(Contents, "<", ">", ref temp);
            Children = ScriptEngine.BuildScriptTreeFromFile(includeFile);
        }

        private void processDefineSection()
        {
            //if this command contains a child section, do not process for inline sections
            if (IsComplex) return;

            string line = contents.Replace("#define", "").Trim();
            int idx1 = line.IndexOf(" ");
            int idx2 = line.IndexOf("(");

            if (idx2 != -1 && idx2 < idx1)  //template has some parameters
            {
                int idx3 = line.IndexOf(")");

                if (Children.Count == 0)  //if this is an inline template
                {
                    addInlineChild(line.Substring(idx3 + 1).Trim());
                }
            }
            else
            {
                //no arguments: e.g. #define a b or #define a \r\n{...}
                if (Children.Count == 0)  //if this is an inline template
                {
                    addInlineChild(line.Substring(idx1 + 1).Trim());
                }
            }
        }

        private void addInlineChild(string line)
        {
            ScriptSection inline = new ScriptSection();
            inline.Contents = line;
            Children.Add(inline);
        }

        public ScriptTree Children = new ScriptTree();
        public ScriptElementType Type = ScriptElementType.Comment;

        public bool IsComplex
        {
            get
            {
                if (IsSectionEndMarker) return false;
                if (IsSectionStartMarker) return false;
                if (Contents.EndsWith(";")) return false;
                if (Contents.StartsWith("#include")) return false;
                if (Contents.StartsWith("#register")) return false;

                return true;
            }
        }

        public bool IsSectionStartMarker
        {
            get
            {
                return Contents == "{";
            }
        }

        public bool IsSectionEndMarker
        {
            get
            {
                return Contents == "}";
            }
        }

        internal ScriptNode CreateScriptModel()
        {
            switch (Type)
            {
                case ScriptElementType.Define:
                {
                    return new DefineNode(this);
                }
                case ScriptElementType.AttachEndPoint:
                {
                    return new AttachEndPointNode(this);
                }
                case ScriptElementType.Block:
                {
                    return new BlockNode(this);
                }
                case ScriptElementType.BlockWeb:
                {
                    return new BlockWebNode(this);
                }
                case ScriptElementType.Reference:
                {
                    return new ReferenceNode(this);
                }
                case ScriptElementType.Include:
                {
                    return new IncludeNode(this);
                }
                case ScriptElementType.Register:
                {
                    return new RegisterNode(this);
                }
                case ScriptElementType.ProcessRequest:
                {
                    return new ProcessRequestNode(this);
                }
                case ScriptElementType.If:
                {
                    return new IfNode(this);
                }
            }

            return null;
        }
    }
}
