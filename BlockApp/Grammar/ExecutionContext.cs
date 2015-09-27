using System;
using System.Collections.Generic;
using System.Text;
using BlockBroker;
using DCRF.Dynamic;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Grammar
{
    public class ExecutionContext
    {
        private static ExecutionContext current = null;
        public static ExecutionContext Current
        {
            get
            {
                return current;
            }
        }

        public static void EnterLevel()
        {
            current = new ExecutionContext(current);
        }

        public static void ExitLevel()
        {
            if (current == null) throw new InvalidOperationException();

            current = current.parent;
        }
        
        private ExecutionContext(ExecutionContext parent)
        {
            this.parent = parent;

            if (parent == null)
            {
                blockBroker.FailoverBroker = dynamicBlockBroker;
            }
        }

        private ExecutionContext parent = null;
        private FileBlockBroker blockBroker = new FileBlockBroker();
        private DynamicBlockBroker dynamicBlockBroker = new DynamicBlockBroker();
        
        private string currentBlockWebId = null;
        private string currentBlockId = null;
        private Dictionary<string, IBlockWeb> blockWebs = new Dictionary<string,IBlockWeb>();
        private Dictionary<string, DefineCmd> templates = new Dictionary<string, DefineCmd>();
        private Dictionary<string, object> variables = new Dictionary<string, object>();
        private Dictionary<string, BlockWebCmd> blockWebDefinitions = new Dictionary<string, BlockWebCmd>();
        private bool continueLoopFlag = false;
        private bool breakLoopFlag = false;

        public IBlockBroker DefaultBroker
        {
            get
            {
                if (parent != null)
                {
                    //brokers are only kept at root level context
                    return parent.DefaultBroker;
                }

                return blockBroker;
            }
        }

        public void RegisterBlockWebDefinition(string id, BlockWebCmd definition)
        {
            blockWebDefinitions[id] = definition;
        }

        public BlockWebCmd LookupBlockWebDefinition(string id)
        {
            if (blockWebDefinitions.ContainsKey(id))
            {
                return blockWebDefinitions[id];
            }

            if (parent != null)
            {
                return parent.LookupBlockWebDefinition(id);
            }

            return null;
        }

        public void RegisterBrokerAssemblyFile(string filePath)
        {
            if (parent != null)
            {
                //brokers are only kept at root level context
                parent.RegisterBrokerAssemblyFile(filePath);
            }

            blockBroker.AddFile(filePath);
        }

        public void RegisterDynamicBlock(BlockHandle handle, DBDefinition def)
        {
            if (parent != null)
            {
                //brokers are only kept at root level context
                parent.RegisterDynamicBlock(handle, def);
            }

            dynamicBlockBroker.AddBlock(handle, def);
        }

        public void RegisterVariables(List<Identifier> argNames, List<object> argValues)
        {
            int i = 0;
            foreach (object value in argValues)
            {
                string argName = argNames[i++].ValueText;
                this.variables[argName] = value;
            }
        }

        public void RegisterVariable(string varName)
        {
            this.variables.Add(varName, null); 
        }

        public bool LookupVariable(string varName)
        {
            if (variables.ContainsKey(varName)) return true;
            if (parent != null) return parent.LookupVariable(varName);

            return false;
        }

        public object this[string argName]
        {
            get
            {
                if (variables.ContainsKey(argName)) return variables[argName];
                if (parent != null) return parent[argName];

                throw new Exception("Variable " + argName + " does not exist");
            }
            set
            {
                if (variables.ContainsKey(argName)) variables[argName] = value;
                if (parent != null) parent[argName] = value;
            }
        }

        public void RegisterTemplate(string name, DefineCmd handler)
        {
            templates[name] = handler;
        }

        public DefineCmd LookupTemplate(string name)
        {
            if (templates.ContainsKey(name)) return templates[name];
            if (parent != null) return parent.LookupTemplate(name);
            
            return null;
        }

        //public IBlockWeb this[string blockWebId]
        //{
        //    get
        //    {
        //        if (blockWebs.ContainsKey(blockWebId))
        //        {
        //            return blockWebs[blockWebId];
        //        }

        //        if (parent != null)
        //        {
        //            return parent[blockWebId];
        //        }

        //        return null;
        //    }
        //}

        public void RegisterBlockWeb(IBlockWeb web, bool isInnerWeb)
        {
            //add it to the parent too
            //so outer caller code will be able to see generated blockWebs
            if (!isInnerWeb && parent != null)
            {
                parent.RegisterBlockWeb(web, false);
            }

            blockWebs[web.Id] = web;
            currentBlockWebId = web.Id;
        }

        public IBlockWeb LookupBlockWeb(string id)
        {
            if (blockWebs.ContainsKey(id)) return blockWebs[id];
            if (parent != null) return parent.LookupBlockWeb(id);

            throw new Exception("BlockWeb " + id + " does not exist");
        }

        public IBlockWeb ActiveBlockWeb
        {
            get
            {
                if (currentBlockWebId != null)
                {
                    return LookupBlockWeb(currentBlockWebId);
                }

                if (parent != null)
                {
                    return parent.ActiveBlockWeb;
                }

                return null;
            }
        }

        public string ActiveBlockWebId
        {
            get
            {
                if (currentBlockWebId != null)
                {
                    return currentBlockWebId;
                }

                if (parent != null)
                {
                    return parent.ActiveBlockWebId;
                }

                return null;
            }
        }

        //public IBlock ActiveBlock
        //{
        //    get
        //    {
        //        if (currentBlockId != null) return ActiveBlockWeb[currentBlockId];

        //        if (parent != null)
        //        {
        //            return parent.ActiveBlock;
        //        }

        //        return null;
        //    }
        //    set
        //    {
        //        currentBlockId = value.Id;
        //    }
        //}

        public string ActiveBlockId
        {
            get
            {
                if (currentBlockId != null) return currentBlockId;

                if (parent != null)
                {
                    return parent.ActiveBlockId;
                }

                return null;
            }
            set
            {
                currentBlockId = value;
            }
        }

        public static void Reset()
        {
            if (current != null)
            {
                foreach (IBlockWeb bw in current.Export())
                {
                    bw.Dispose();
                }
            }

            current = null;
        }

        public List<IBlockWeb> Export()
        {
            return new List<IBlockWeb>(blockWebs.Values);
        }

        public bool ContinueLoopFlag
        {
            get
            {
                return continueLoopFlag;
            }
            set
            {
                continueLoopFlag = value;
            }
        }

        public bool BreakLoopFlag
        {
            get
            {
                return breakLoopFlag;
            }
            set
            {
                breakLoopFlag = value;
            }
        }
    }

}
