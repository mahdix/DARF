using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Contract;
using DCRF.Definition;
using DCRF.Dynamic;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Grammar
{
    public class AttachEndPointCmd : CommandHandler
    {
        private AddressHandler address = null;
        private AttachSuffix suffix = null;
        private EndPoint endPoint = null;
        private AttachMode mode = null;

        [Rule(@"<AttachEndpointCmd> ::= ~'!' <Address> <AttachMode> ~'=' <Endpoint> <AttachSuffix> ~';'")]
        public AttachEndPointCmd(AddressHandler address, AttachMode mode, EndPoint endPoint, AttachSuffix suffix)
        {
            this.address = address;
            this.endPoint = endPoint;
            this.suffix = suffix;
            this.mode = mode;
        }

        public override void Execute()
        {
            string connectorKey = mode.GetConnectorFullName(address.GetConnectorKey());
            IBlock block = ExecutionContext.Current.LookupBlockWeb(address.GetBlockWebId())[address.GetBlockId()];

            //address: to which block we want to attach an ednpoint
            //objOrCall: what do we want to attach?
            //suffix: create or just attach?
            if (suffix.Create)
            {
                block.ProcessRequest(
                    "ProcessMetaService", 
                    BlockMetaServiceType.CreateConnector,
                    connectorKey, 
                    null);
            }

            
            IConnector connector = block[connectorKey];
            if (connector == null)
            {
                throw new Exception("Connector " + connectorKey + " does not exist!");
            }

            endPoint.AttachToConnector(connector);
        }
    }

    public class EndPoint : Token
    {
        private ObjectHolder epObject = null;
        private Identifier epIdentifier = null;
        private AddressHandler address = null;
        private bool addressIsConnector = false;
        private string epConnectorKey = null;

        [Rule(@"<Endpoint> ::= '&' <Address>")]
        public EndPoint(Token andSign, AddressHandler address)
        {
            this.address = address;
            this.addressIsConnector = false;
        }

        [Rule(@"<Endpoint> ::= ~'!' <Address>")]
        public EndPoint(AddressHandler address)
        {
            this.address = address;
            this.addressIsConnector = true;
        }

        [Rule(@"<Endpoint> ::= <Object>")]
        public EndPoint(ObjectHolder epObject)
        {
            this.epObject = epObject;
        }

        [Rule(@"<Endpoint> ::= Identifier")]
        public EndPoint(Identifier epIdentifier)
        {
            this.epIdentifier = epIdentifier;
        }

        public void AttachToConnector(IConnector connector)
        {
            if (address != null)
            {
                if (addressIsConnector == false)
                {
                    connector.AttachEndPoint(address.GetBlockId(), address.GetServiceName());
                }
                else
                {
                    connector.AttachConnectorEndPoint(address.GetBlockId(), address.GetConnectorKey());
                }
            }
            else if (epObject != null)
            {
                connector.AttachEndPoint(epObject.Value);
            }
            else
            {
                connector.AttachEndPoint(epIdentifier.Value);
            }
        }

        public void FillDefinition(DBSLineObjOrCall ooc)
        {
            if (address != null)
            {
                if (addressIsConnector == false)
                {
                    ooc.Address = new List<string>() { null, address.GetBlockId(), address.GetServiceName()};
                }
                else
                {
                    ooc.Address = new List<string>() { null, address.GetBlockId(), address.GetConnectorKey()};
                    ooc.isConnectorCall = true;
                }
            }
            else if (epObject != null)
            {
                ooc.Obj = epObject.Value;
            }
            else
            {
                ooc.Obj = epIdentifier.Value;
            }
        }
    }

    public class AttachSuffix : Identifier
    {

        [Rule(@"<AttachSuffix> ::= ~'[' create ~']'")]
        public AttachSuffix(Identifier cmd): base(cmd.ValueText)
        {
        }

        [Rule(@"<AttachSuffix> ::=")]
        public AttachSuffix()
        {
        }

        public bool Create
        {
            get
            {
                return ValueText == "create";
            }
        }
    }

    public class OptIdentifier : Token
    {
        private Identifier id = null;

        [Rule(@"<OptIdentifier> ::= ~',' Identifier")]
        public OptIdentifier(Identifier id)
        {
            this.id = id;
        }

        [Rule(@"<OptIdentifier> ::=")]
        public OptIdentifier()
        {
        }

        public string Identifier
        {
            get
            {
                if (id == null) return null;

                return id.ValueText;
            }
        }

    }

    public class AttachMode : Token
    {
        private string prefix = "";
        private OptIdentifier optIdentifier = null;

        [Rule(@"<AttachMode> ::= '[' B <OptIdentifier> ']'")]
        [Rule(@"<AttachMode> ::= '[' I <OptIdentifier> ']'")]
        [Rule(@"<AttachMode> ::= '[' A <OptIdentifier> ']'")]
        public AttachMode(Token a, Identifier b, OptIdentifier optIdentifier, Token c)
        {
            prefix = b.ValueText;
            this.optIdentifier = optIdentifier;
        }

        [Rule(@"<AttachMode> ::=")]
        public AttachMode()
        {
        }

        public string GetConnectorFullName(string connectorKey)
        {
            string id = null;

            if (optIdentifier != null && optIdentifier.Identifier != null)
            {
                return SysEventCode.Join(prefix, connectorKey, optIdentifier.Identifier);
            }
            else
            {
                return SysEventCode.Join(prefix, connectorKey);
            }
        }
    }
}
