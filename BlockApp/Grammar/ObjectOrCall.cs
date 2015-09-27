using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Contract;
using DCRF.Dynamic;

namespace BlockApp.Grammar
{
    public class ObjectOrCall : Token
    {
        private AddressHandler address = null;
        private TokenList<ObjectOrCall> objOrCallList = null;
        private ObjectHolder obj = null;
        private Identifier id = null;

        [Rule(@"<ObjOrCall> ::= <GeneralAddress> ~'(' <OptObjOrCallList> ~')'")]
        public ObjectOrCall(AddressHandler address, Optional<TokenList<ObjectOrCall>> objOrCallList)
        {
            this.address = address;
            this.objOrCallList = objOrCallList.Value;
        }

        [Rule(@"<ObjOrCall> ::= <Object>")]
        public ObjectOrCall(ObjectHolder obj)
        {
            this.obj = obj;
        }

        [Rule(@"<ObjOrCall> ::= Identifier")]
        public ObjectOrCall(Identifier id)
        {
            this.id = id;
        }

        public DBSLineObjOrCall GetDefinition()
        {
            DBSLineObjOrCall result = new DBSLineObjOrCall();

            if (obj != null)
            {
                result.Obj = obj.Value;
            }
            else if (id != null)
            {
                result.Obj = id.Value;
            }
            else
            {
                result.isConnectorCall = address.IsConnector;

                result.Address = address.GetList();

                foreach (ObjectOrCall ooc in objOrCallList)
                {
                    result.Args.Add(ooc.GetDefinition());
                }
            }

            return result;
        }

        /// <summary>
        /// This calls the obj/service and returns the result-> using in a processRequest command
        /// which we want to execute something
        /// </summary>
        public object GetValue()
        {
            if (obj != null) return obj.Value;
            if (id != null) return id.Value;

            List<object> args = null;

            if (objOrCallList != null)
            {
                args = new List<object>();

                foreach (ObjectOrCall ooc in objOrCallList)
                {
                    args.Add(ooc.GetValue());
                }
            }

            object result = null;
            if (args == null)
            {
                result = address.ProcessRequest();
            }
            else
            {
                result = address.ProcessRequest(args.ToArray());
            }

            return result;
        }
    }

}
