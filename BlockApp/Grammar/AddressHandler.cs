using System;
using System.Collections.Generic;
using System.Text;
using bsn.GoldParser.Semantic;
using DCRF.Contract;
using DCRF.Definition;
using DCRF.Interface;

namespace BlockApp.Grammar
{

    public class AddressHandler : Token
    {
        private Identifier blockWebId = null;
        private Identifier blockId = null;
        private Identifier serviceOrConnectorName = null;
        private bool isConnector = false;

        public override string ToString()
        {
            return (isConnector ? "!" : "") + 
                (blockWebId == null ? "":blockWebId.ValueText + "." ) +
                (blockId == null ? "" : blockId.ValueText + "." ) + serviceOrConnectorName.ValueText;
        }

        [Rule(@"<GeneralAddress> ::= <Address>")]
        public AddressHandler(AddressHandler addr)
        {
            blockWebId = addr.blockWebId;
            blockId = addr.blockId;
            serviceOrConnectorName = addr.serviceOrConnectorName;
        }

        [Rule(@"<GeneralAddress> ::= '!' <Address>")]
        public AddressHandler(Token notSign, AddressHandler addr)
            : this(addr)
        {
            isConnector = true;
        }

        [Rule(@"<Address> ::= Identifier ~'.' Identifier")]
        public AddressHandler(Identifier blockIdToken, Identifier serviceNameToken)
        {
            this.blockId = blockIdToken;
            this.serviceOrConnectorName = serviceNameToken;
        }

        [Rule(@"<Address> ::= Identifier ~'.' Identifier ~'.' Identifier")]
        public AddressHandler(Identifier blockWebToken, Identifier blockIdToken, Identifier serviceNameToken)
        {
            this.blockId = blockIdToken;
            this.serviceOrConnectorName = serviceNameToken;
            this.blockWebId = blockWebToken;
        }

        [Rule(@"<Address> ::= Identifier")]
        public AddressHandler(Identifier id)
        {
            this.serviceOrConnectorName = id;
        }

        public IConnector GetConnector(string timing)
        {
            IBlock block = ExecutionContext.Current.LookupBlockWeb(GetBlockWebId())[GetBlockId()];

            return block[SysEventCode.Join(timing, serviceOrConnectorName.ValueText)];
        }

        //TODO: change to property
        public string GetBlockWebId()
        {
            if (blockWebId == null) return ExecutionContext.Current.ActiveBlockWebId ;
            return blockWebId.ValueText;
        }

        public string GetBlockId()
        {
            if (blockId == null) return ExecutionContext.Current.ActiveBlockId;

            return blockId.ValueText;
        }

        public string GetServiceName()
        {
            return this.serviceOrConnectorName.ValueText;
        }

        public string GetConnectorKey()
        {
            return GetServiceName();
        }

        public bool IsConnector
        {
            get
            {
                return isConnector;
            }
        }

        internal List<string> GetList()
        {
            List<String> result = new List<string>();

            result.Add(GetBlockWebId());
            result.Add(GetBlockId());
            result.Add(GetServiceName());

            return result;
        }

        public object ProcessRequest(params object[] args)
        {
            if (!isConnector)
            {
                return ExecutionContext.Current.LookupBlockWeb(GetBlockWebId())[GetBlockId()].ProcessRequest(
                                GetServiceName(), args);
            }
            else
            {
                return ExecutionContext.Current.LookupBlockWeb(GetBlockWebId())[GetBlockId()][GetConnectorKey()].ProcessRequest(args);
            }
        }
    }
}
