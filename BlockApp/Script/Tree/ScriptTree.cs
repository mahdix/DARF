using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using BlockApp.Script.Model;
using BlockApp.Script.Tree;

namespace BlockApp.Script.Tree
{
    /// <summary>
    /// This class is only a storage with little knowledge about its contents
    /// </summary>
    public class ScriptTree: List<ScriptSection>
    {
        public ScriptTree()
        {
        }

        internal List<ScriptNode> CreateScriptModel()
        {
            List<ScriptNode> result = new List<ScriptNode>();

            foreach (ScriptSection section in this)
            {
                result.Add(section.CreateScriptModel());
            }

            return result;
        }
    }
}
