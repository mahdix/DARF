using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DCRF.Primitive;

namespace BlockApp.Script
{
    public class Helper
    {
        public static bool HasABlock(string line)
        {
            return !line.Trim().EndsWith(";");
        }

        /// <summary>
        /// input is something like this: f(g(h(a,b,c),d,g),n,o(x,y),p)
        /// and result will be: f-> g->
        ///                         n   h-> a
        ///                         o   d   b
        ///                         p   g   c
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static List<FunctionCall> ExtractNestedFunctionCalls(string txt)
        {
            List<FunctionCall> result = new List<FunctionCall>();
            FunctionCall currentCall = new FunctionCall();
            string currentIdentifier = "";

            for (int i = 0; i < txt.Length; i++)
            {
                string chr = txt[i].ToString();

                if (chr == "(")
                {
                    currentCall.Identifier = currentIdentifier;

                    string exp = extractBracketExpression(txt, ref i);

                    foreach (FunctionCall fc in ExtractNestedFunctionCalls(exp))
                    {
                        currentCall.AddArgument(fc);
                    }

                    result.Add(currentCall);
                    currentIdentifier = "";
                    currentCall = new FunctionCall();

                    //bypass closing bracket
                    i++;
                }
                else if (chr == ",")
                {
                    if (currentIdentifier != "")
                    {
                        currentCall.Identifier = currentIdentifier;
                        result.Add(currentCall);
                        currentIdentifier = "";
                        currentCall = new FunctionCall();
                    }
                }
                else
                {
                    currentIdentifier += chr;
                }
            }

            if (currentIdentifier != "")
            {
                currentCall.Identifier = currentIdentifier;
                result.Add(currentCall);
            }

            return result;
        }

        private static string extractBracketExpression(string txt, ref int i)
        {
            //fnid matching closing bracker
            int level = 1;
            string exp = "";
            i++;

            while (level != 0 || txt[i] != ')' )
            {
                if (txt[i] == ')')
                {
                    level--;
                    if (level > 0)
                    {
                        exp += txt[i++];
                    }
                }
                else if (txt[i] == '(')
                {
                    level++;
                    exp += txt[i++];
                }
                else
                {
                    exp += txt[i++];
                }
            }

            //bypass closing bracket
            i++;

            return exp;
        }

        public static ScriptElementType DetermineElementType(string txt)
        {
            if (txt.StartsWith("//"))
            {
                return ScriptElementType.Comment;
            }
            else if (txt.StartsWith("#include"))
            {
                return ScriptElementType.Include;
            }
            else if (txt.StartsWith("#define"))
            {
                return ScriptElementType.Define;
            }
            else if (txt.StartsWith("#if"))
            {
                return ScriptElementType.If;
            }
            else if (txt.StartsWith("blockWeb"))
            {
                return ScriptElementType.BlockWeb;
            }
            else if (txt.StartsWith("block"))
            {
                return ScriptElementType.Block;
            }
            else if (txt.Contains("+=") || txt.Contains("="))
            {
                return ScriptElementType.AttachEndPoint;
            }
            else if (txt.StartsWith("#register"))
            {
                return ScriptElementType.Register;
            }
            else if (txt.StartsWith("{"))
            {
                return ScriptElementType.Reference;
            }
            else
            {
                return ScriptElementType.ProcessRequest;
            }
        }

        public static string ReadNextLine(TextReader tr)
        {
            string result = "";

            while (result.Length == 0 || result.StartsWith("//"))
            {
                result = tr.ReadLine();

                if (result == null) return null;

                result = result.Trim(' ', '\t');
            }

            return result;
        }

        public static string ProcessTemplateLine(string template, List<string> args, List<string> argValues)
        {
            Tokenizer.TokenList tokens = Tokenizer.GetInstance().Tokenize(template);
            string result = "";

            foreach (Tokenizer.Token t in tokens)
            {
                int idx = args.IndexOf(t.Text);

                if (idx != -1)
                {
                    t.Text = argValues[idx];
                }

                result += t.Text;
            }

            return result;
        }

        public static List<string> ExtractTokens(string text, params string[] markers)
        {
            List<string> result = new List<string>();
            int startIndex = 0;

            for (int i = 0; i < markers.Length-1; i++)
            {
                string token = ExtractToken(text, markers[i], markers[i + 1], ref startIndex);
                result.Add(token);
            }

            return result;
        }

        /// <summary>
        /// in text there may be multiple markers which are matched. 
        /// e.g. aa{bb{cc}dd}
        /// here {bb{cc}dd} is a token whose startMarker is { and endMarker is }
        /// but using old extractToken method {bb{cc} will be returned as a token
        /// </summary>
        /// <param name="text"></param>
        /// <param name="startMarker"></param>
        /// <param name="endMarker"></param>
        /// <returns></returns>
        public static string ExtractTokenMatch(string text, char startMarker, char endMarker, ref int startIndex)
        {
            string result = "";

            startIndex = text.IndexOf(startMarker, startIndex);
            if (startIndex == -1) return null;

            startIndex++;
            int level = 0;
            for (; startIndex < text.Length; startIndex++)
            {
                char chr = text[startIndex];

                if (chr == startMarker) 
                {
                    level++;
                }
                else if (chr == endMarker)
                {
                    level--;

                    if (level == -1) return result;
                }
                else
                {
                    result += chr;
                }
            }

            //a matching endMarker is not seen 
            return null;
            
        }

        public static string ExtractToken(string text, string startMarker, string endMarker, ref int startIndex)
        {
            string result = "";

            if (startMarker == null)   //start from the very beginning and look and endMarker
            {
                int idx = text.IndexOf(endMarker, startIndex);

                if (idx == -1)
                {
                    throw new Exception("Error extracting token from " + text + " endMarker="+endMarker);
                    //result = null;
                }
                else
                {
                    result = text.Substring(startIndex, idx - startIndex).Trim();
                    startIndex = idx + endMarker.Length;
                }
            }
            else if (endMarker == "")  //start from place of startMarker and go through the end of the string
            {
                int idx = text.IndexOf(startMarker, startIndex);

                if (idx == -1)
                {
                    throw new Exception("Error extracting token from " + text + " startMarker=" + startMarker);
                }
                else
                {
                    result = text.Substring(idx + startMarker.Length).Trim();
                    startIndex = text.Length;
                }
            }
            else
            {
                int sIndex = text.IndexOf(startMarker, startIndex);
                int eIndex = text.IndexOf(endMarker, sIndex+1);

                if (sIndex == -1 || eIndex == -1)
                {
                    throw new Exception("Error extracting token from " + text + " startMarker=" + startMarker + ", endMarker=" + endMarker);
                }

                result = text.Substring(sIndex+startMarker.Length, eIndex - sIndex - startMarker.Length).Trim();
                startIndex = eIndex + endMarker.Length;
            }

            return result;
        }

        /// <summary>
        /// TODO: use this method instead of hard coding
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        public static bool IsObject(string txt)
        {
            if (txt.StartsWith("{") && txt.EndsWith("}")) return true; //general object
            if (txt.StartsWith("'") && txt.EndsWith("'")) return true;  //string

            int x = 0;
            if ( int.TryParse(txt, out x) ) return true;  //int

            return false;
        }
        /// <summary>
        /// note than object notation should not contain colon ','
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        internal static object ReadObject(string txt)
        {
            if (txt.StartsWith("{") && txt.EndsWith("}"))
            {
                //for a string array with separator: System.String[]:a:b:c
                //for an int array with separator: System.Int32[]:1:2:3
                //for nullValue: '^'
                string[] items = txt.Substring(1, txt.Length - 2).Split(':');

                Type arrType = Type.GetType(items[0]);
                Type elementType = arrType.GetElementType();

                Array typedArray = Array.CreateInstance(elementType, items.Length - 1);

                for (int i = 1; i < items.Length; i++)
                {
                    typedArray.SetValue(Convert.ChangeType(items[i], elementType), i-1);
                }

                return typedArray;
            }

            if (txt.StartsWith("'") && txt.EndsWith("'")) return txt.Substring(1, txt.Length - 2); ;

            int x = 0;
            if (int.TryParse(txt, out x)) return x;


            return null;
        }
    }

    public class FunctionCall
    {
        public string Identifier = null;
        public List<FunctionCall> Arguments = new List<FunctionCall>();

        public FunctionCall Parent { get; set; }

        public void AddArgument(FunctionCall call)
        {
            Arguments.Add(call);
            call.Parent = this;
        }

        public FunctionCall createClone(FunctionCall parent = null)
        {
            FunctionCall result = new FunctionCall();

            result.Identifier = Identifier;
            result.Parent = parent;
            result.Arguments = new List<FunctionCall>();

            foreach (FunctionCall fc in Arguments)
            {
                result.Arguments.Add(fc.createClone(result));
            }

            return result;
        }
    }

    public enum ScriptElementType
    {
        Root,
        Comment,
        Include,
        Define,
        Block,
        BlockWeb,
        AttachEndPoint,
        Register,
        ProcessRequest,
        Reference,       //e.g. ttp1(x) to include ttp1 define
        If
    }
}
