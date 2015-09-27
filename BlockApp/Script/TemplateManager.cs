using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Model;
using BlockApp.Script.Tree;
using DCRF.Interface;

namespace BlockApp.Script
{
    public class TemplateManager
    {
        private static TemplateManager instance = new TemplateManager();
        public static TemplateManager GetInstance()
        {
            return instance;
        }

        protected TemplateManager()
        {
        }

        private Dictionary<string, DefineNode> templates = new Dictionary<string, DefineNode>();


        public void Clear()
        {
            templates.Clear();
        }

        public void RegisterTemplate(string key, DefineNode node)
        {
            templates[key] = node;
        }

        public DefineNode LookupTemplate(string key)
        {
            if (templates.ContainsKey(key)) return templates[key];
            return null;
        }
       
    }
}
