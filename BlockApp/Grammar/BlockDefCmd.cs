using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Dynamic;
using DCRF.Primitive;

namespace BlockApp.Grammar
{
    public class BlockDefCmd: CommandHandler
    {
        private Identifier id = null;
        private DefBlockBase baseType = null;
        private TokenList<DefBlockBodyItem> body = null;

        [Rule(@"<DefBlockCmd> ::= ~'#' ~block Identifier <DefBlockBase> ~'{' <DefBlockBody> ~'}'")]
        public BlockDefCmd(Identifier id, DefBlockBase baseType, TokenList<DefBlockBodyItem> body)
        {
            this.id = id;
            this.baseType = baseType;
            this.body = body;
        }

        public override void Execute()
        {
            BlockHandle handle = new BlockHandle(id.ValueText);
            DBDefinition definition = new DBDefinition();

            foreach (DefBlockBodyItem bodyItem in body)
            {
                bodyItem.Fill(definition);
            }

            if (baseType.Handle != null)
            {
                definition.BaseType = baseType.Handle;
            }

            //register the dynamic block so in future commands
            //user can instantiate it using: block command
            ExecutionContext.Current.RegisterDynamicBlock(handle, definition);
        }
    }

    public class DefBlockBase : Identifier
    {
        private BlockHandleToken handle = null;

        [Rule(@"<DefBlockBase> ::= ~':' <BlockHandle>")]
        public DefBlockBase(BlockHandleToken handle)
        {
            this.handle = handle;
        }

        [Rule(@"<DefBlockBase> ::=")]
        public DefBlockBase()
        {
        }

        public BlockHandle Handle
        {
            get
            {
                if (handle == null) return null;
                return handle.Handle;
            }
        }
    }
    
    public class DefBlockBodyItem: Identifier
    {
        //context is used to lookup values
        public virtual void Fill(DBDefinition definition)
        {
        }
    }

    public class DefBlockConnector: DefBlockBodyItem
    {
        private Identifier id = null;

        [Rule(@"<DefBlockConnector> ::= ~connector Identifier ~';'")]
        public DefBlockConnector(Identifier id)
        {
            this.id = id;
        }

        public override void Fill(DBDefinition definition)
        {
            definition.Connectors.Add(id.ValueText);
        }
    }

    public class DefBlockService: DefBlockBodyItem
    {
        private Identifier id = null;
        private Optional<TokenList<Identifier>> optArgList = null;
        private TokenList<DefBlockServiceLine> body = null;

        [Rule(@"<DefBlockService> ::= ~service Identifier ~'(' <OptArgList> ~')' ~'{' <DefBlockServiceBody> ~'}'")]
        public DefBlockService(Identifier id, Optional<TokenList<Identifier>> optArgList, TokenList<DefBlockServiceLine> body)
        {
            this.id = id;
            this.optArgList = optArgList;
            this.body = body;
        }

        public override void Fill(DBDefinition definition)
        {
            DBServiceDefinition serviceDef = new DBServiceDefinition();

            if (optArgList.HasValue)
            {
                foreach (Identifier argTitle in optArgList.Value)
                {
                    serviceDef.Args.Add(argTitle.ValueText);
                }
            }

            bool hasReturn = false;

            foreach (DefBlockServiceLine item in body)
            {
                DBSLineDefinition dbslid = item.getDefinition();
                serviceDef.Body.Add(dbslid);

                hasReturn = dbslid.IsReturn;
            }

            definition.Services[this.id.ValueText] = serviceDef;
        }
    }

    public class DefBlockServiceLine : Identifier
    {
        private DefBlockServiceBodyModifier modifier = null;
        private AddressHandler address = null;
        private Optional<TokenList<ObjectOrCall>> optObjOrCallList = null;
        private Identifier rightValue = null;
        private ObjectHolder rightValueObj = null;
        private AttachSuffix suffix = null;
        private EndPoint endPoint = null;

        [Rule(@"<DefBlockServiceLine> ::= <DefBlockServiceBodyModifier> <GeneralAddress> ~'(' <OptObjOrCallList> ~')' ~';'")]
        public DefBlockServiceLine(DefBlockServiceBodyModifier modifier, AddressHandler address, 
            Optional<TokenList<ObjectOrCall>> optObjOrCallList)
        {
            this.modifier = modifier;
            this.address = address;
            this.optObjOrCallList = optObjOrCallList;
        }

        [Rule(@"<DefBlockServiceLine> ::= <DefBlockServiceBodyModifier> Identifier ~';'")]
        public DefBlockServiceLine(DefBlockServiceBodyModifier modifier, Identifier rightValue)
        {
            if (modifier.ValueText != "return")
            {
                throw new Exception("Identifier " + rightValue.ValueText + " can only follow a return statement");
            } 
            
            this.modifier = modifier;
            this.rightValue = rightValue;
        }

        [Rule(@"<DefBlockServiceLine> ::= <DefBlockServiceBodyModifier> <Object> ~';'")]
        public DefBlockServiceLine(DefBlockServiceBodyModifier modifier, ObjectHolder rightValueObj)
        {
            if (modifier.ValueText != "return")
            {
                throw new Exception("Identifier " + rightValueObj.ValueText + " can only follow a return statement");
            }

            this.modifier = modifier;
            this.rightValueObj = rightValueObj;
        }


        [Rule(@"<DefBlockServiceLine> ::= <Address> ~'=' <Endpoint> <AttachSuffix> ~';'")]
        public DefBlockServiceLine(AddressHandler address, EndPoint endPoint, AttachSuffix suffix)
        {
            this.address = address;
            this.suffix = suffix;
            this.endPoint = endPoint;
        }

        //private bool suffixContainsIdentifier(string txt)
        //{
        //    if (suffixList == null) return false;

        //    if (suffixList.HasValue)
        //    {
        //        foreach (Identifier id in suffixList.Value)
        //        {
        //            if (id.ValueText == txt) return true;
        //        }
        //    }

        //    return false;
        //}

        internal DBSLineDefinition getDefinition()
        {
            DBSLineDefinition result = new DBSLineDefinition();

            result.LineType = (endPoint == null ? DBSLineType.ProcessRequest : DBSLineType.AttachEndPoint);
            result.CreateConnector = (suffix != null && suffix.Create);
            result.IsReturn = (modifier != null && modifier.IsReturn);

            result.Start = new DBSLineObjOrCall();

            if (address != null)
            {
                result.Start.isConnectorCall = address.IsConnector;
                result.Start.Address = address.GetList();
            }

            if (optObjOrCallList != null && optObjOrCallList.HasValue)
            {
                foreach (ObjectOrCall ooc in optObjOrCallList.Value)
                {
                    result.Start.Args.Add(ooc.GetDefinition());
                }
            }
            else if (rightValue != null)
            {
                DBSLineObjOrCall ooc = new DBSLineObjOrCall();
                ooc.Obj = rightValue.Value;
                result.Start.Args.Add(ooc);
            }
            else if (rightValueObj != null)
            {
                DBSLineObjOrCall ooc = new DBSLineObjOrCall();
                ooc.Obj = rightValueObj.Value;
                result.Start.Args.Add(ooc);
            }
            else if (endPoint != null)
            {
                DBSLineObjOrCall ooc = new DBSLineObjOrCall();
                endPoint.FillDefinition(ooc);

                result.Start.Args.Add(ooc);
            }

            return result;
        }
    }


    public class DefBlockServiceBodyModifier : Identifier
    {
        [Rule(@"<DefBlockServiceBodyModifier> ::= ~return")]
        public DefBlockServiceBodyModifier(Token txt)
            : base("return")
        {
        }

        [Rule(@"<DefBlockServiceBodyModifier> ::= ")]
        public DefBlockServiceBodyModifier()
        {
        }

        public bool IsReturn
        {
            get
            {
                return this.ValueText == "return";
            }
        }
    }
}
