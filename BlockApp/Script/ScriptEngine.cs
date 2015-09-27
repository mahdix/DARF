using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BlockApp.Script.Model;
using BlockApp.Script.Tree;
using DCRF.Contract;
using DCRF.Core;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Script
{
    public class ScriptEngine
    {
        public static object ExecuteScriptFile(string scriptFile)
        {
            TemplateManager.GetInstance().Clear();

            ScriptTree tree = BuildScriptTreeFromFile(scriptFile);

            return executeScriptTree(tree);
        }

        public static object ExecuteScript(string scriptContents)
        {
            TemplateManager.GetInstance().Clear();

            ScriptTree tree = BuildScriptTreeFromContents(scriptContents);

            return executeScriptTree(tree);
        }

        private static object executeScriptTree(ScriptTree tree)
        {
            List<ScriptNode> graph = tree.CreateScriptModel();

            foreach (ScriptNode node in graph)
            {
                node.PreProcess();
            } 
            
            foreach (ScriptNode node in graph)
            {
                node.Process();
            }

            ExecutionContext context = new ExecutionContext();

            foreach (ScriptNode node in graph)
            {
                node.Execute(context);
            }

            return context.CurrentBlockWeb;
        }

        public static ScriptTree BuildScriptTreeFromFile(string scriptFile)
        {
            StreamReader sr = new StreamReader(scriptFile);

            return innerBuildScriptTree(sr);
        }

        public static ScriptTree BuildScriptTreeFromContents(string scriptContents)
        {
            StringReader tr = new StringReader(scriptContents);

            return innerBuildScriptTree(tr);
        }

        private static ScriptTree innerBuildScriptTree(TextReader tr)
        {
            try
            {
                ScriptTree tree = new ScriptTree();
                ScriptSection section = readScriptSection(tr);

                while (section != null)
                {
                    tree.Add(section);
                    section = readScriptSection(tr);
                }

                return tree;
            }
            finally
            {
                if (tr != null) tr.Close();
            }
        }

        private static ScriptSection readScriptSection(TextReader tr)
        {
            ScriptSection result = new ScriptSection();

            string scriptLine = Helper.ReadNextLine(tr);
            if (scriptLine == null) //EOF?
            {
                return null;
            }
            result.Contents = scriptLine;

            if (result.IsComplex)
            {
                string startSectionMarker = Helper.ReadNextLine(tr);
                Debug.Assert(startSectionMarker == "{");

                result.Children = new ScriptTree();
                ScriptSection section = null;
                do
                {
                    section = readScriptSection(tr);
                    if (section != null) result.Children.Add(section);
                } while (section != null);
            }
            else if (result.IsSectionEndMarker)
            {
                return null;
            }

            return result;
        }
    }
}
