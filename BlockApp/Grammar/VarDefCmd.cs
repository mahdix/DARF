using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;

namespace BlockApp.Grammar
{
    public class VarDefCmd : CommandHandler
    {
        private Identifier variableName = null;
        private VarDefCmdValue value = null;
        private bool isDefine = false;
        
        [Rule(@"<VarDefCmd> ::= var Identifier ~'=' <VarDefCmdValue> ~';'")]
        public VarDefCmd(Token varToken, Identifier variableName, VarDefCmdValue value)
        {
            this.variableName = variableName;
            this.value = value;
            isDefine = true;
        }

        [Rule(@"<VarDefCmd> ::= Identifier ~'=' <VarDefCmdValue> ~';'")]
        public VarDefCmd(Identifier variableName, VarDefCmdValue value)
        {
            this.variableName = variableName;
            this.value = value;
        }

        public override void Execute()
        {
            if (isDefine)
            {
                ExecutionContext.Current.RegisterVariable(this.variableName.Id);
            }
            else
            {
                if (!ExecutionContext.Current.LookupVariable(this.variableName.Id))
                {
                    throw new Exception("Variable " + this.variableName.ValueText + " used without declaration");
                }
            }

            object finalValue = value.GetValue();
            ExecutionContext.Current[variableName.Id] = finalValue;
        }
    }

    public class VarDefCmdValue : Token
    {
        private static Random r = new Random();

        private AddressHandler address = null;
        private Optional<TokenList<ObjectOrCall>> objOrCallList = null;
        private ObjectHolder objectHolder = null;
        private Identifier id = null;

        [Rule(@"<VarDefCmdValue> ::= <GeneralAddress> ~'(' <OptObjOrCallList> ~')'")]
        public VarDefCmdValue(AddressHandler address, Optional<TokenList<ObjectOrCall>> objOrCallList)
        {
            this.address = address;
            this.objOrCallList = objOrCallList;
        }

        [Rule(@"<VarDefCmdValue> ::= <Object>")]
        public VarDefCmdValue(ObjectHolder objectHolder)
        {
            this.objectHolder = objectHolder;
        }

        [Rule(@"<VarDefCmdValue> ::= Identifier")]
        public VarDefCmdValue(Identifier id)
        {
            this.id = id;
        }
        
        [Rule(@"<VarDefCmdValue> ::= ~'[' ~uid ~']'")]
        public VarDefCmdValue()
        {
            this.id = null;
        }

        public object GetValue()
        {
            if (id != null)
            {
                return id.Value;
            }
            else if (objectHolder != null)
            {
                return objectHolder.Value;
            }
            else if (address != null)
            {
                object result = null;

                if (objOrCallList.HasValue)
                {
                    List<object> args = new List<object>();

                    foreach (ObjectOrCall ooc in objOrCallList.Value)
                    {
                        args.Add(ooc.GetValue());
                    }

                    result = address.ProcessRequest(args.ToArray());
                }
                else
                {
                    result = address.ProcessRequest();
                }

                return result;
            }
            else
            {
                string uniqueId = "uid_";

                for (int i = 0; i < 8; i++)
                {
                    uniqueId += ((char)(97 + r.Next(26))).ToString();
                }

                return uniqueId;
            }
        }
    }
}
