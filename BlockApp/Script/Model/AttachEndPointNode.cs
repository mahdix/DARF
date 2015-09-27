using System;
using System.Collections.Generic;
using System.Text;
using BlockApp.Script.Tree;
using DCRF.Contract;
using DCRF.Interface;
using DCRF.Primitive;

namespace BlockApp.Script.Model
{
    /// <summary>
    /// 
    /// cmdOK.Click += lblResult.SetValue(rf.FetchRow(p));  //attach service endpoint
	///	cmdOK.Click += txtResult2.SetValue(cmdAdder.Add(txt1.GetValue(), txt2.GetValue()));
	///	cmdOK.Click += !txtResult3.FireEvent(5, cmd1.GetValue()); //attachConnector - when click is invoked, invoke fireEvent too
    ///	frmMain.Text += 'RowFetcher' {System.String} {create};
    ///	_coordinator_.FormId += frmMain {create};
    /// </summary>
    class AttachEndPointNode : ScriptNode
    {
        private EndPointDescriptor endPoint = null;
        private string blockId = null;
        private string connectorKey = null;
        private bool createConnector = false;


        public AttachEndPointNode(ScriptNode node): base(node)
        {
        }

        public AttachEndPointNode(ScriptSection section)
            : base(section)
        {
        }

        public override void Process()
        {
            string separator = "=";
            if (processedContents.Contains("+=")) separator = "+=";
            string[] ownerBody = processedContents.Split(new string[] { separator }, StringSplitOptions.None);

            string owner = ownerBody[0].Trim();
            string body = ownerBody[1].Trim();

            endPoint = new EndPointDescriptor();
            int idx = owner.IndexOf(".");

            if (idx == -1)
            {
                blockId = null;
            }
            else
            {
                blockId = owner.Substring(0, idx);
            }
            connectorKey = owner.Substring(idx + 1);

            if (body.Contains("{create}"))
            {
                body = body.Replace("{create}", "");
                createConnector = true;
            }

            List<FunctionCall> fc = Helper.ExtractNestedFunctionCalls(body);
            endPoint = extractEndPoint(fc[0]);
        }

        private EndPointDescriptor extractEndPoint(FunctionCall fc)
        {
            //Helper.RefineToken(ref fc.Identifier);

            if (fc.Identifier.EndsWith(";")) fc.Identifier = fc.Identifier.Substring(0, fc.Identifier.Length - 1);
            EndPointDescriptor endPoint = new EndPointDescriptor();

            //! connector mode: !txtResult3.FireEvent(...
            //{ value mode: {11}
            //else: service mode txtResult2.SetValue(...
            if (fc.Identifier.StartsWith("!"))
            {
                List<string> tokens = Helper.ExtractTokens(fc.Identifier, "!", ".");
                endPoint.blockId = tokens[0];
                endPoint.connectorKey = tokens[1];
            }
            else if (Helper.IsObject(fc.Identifier))
            {
                endPoint.value = Helper.ReadObject(fc.Identifier.Replace(";",""));
            }
            else  //service mode
            {
                List<string> tokens = Helper.ExtractTokens(fc.Identifier, ".");
                endPoint.blockId = tokens[0];
                endPoint.serviceName = tokens[1];
            }

            foreach (FunctionCall child in fc.Arguments)
            {
                endPoint.FixedArgs.Add(extractEndPoint(child));
            }

            return endPoint;
        }

        public override ScriptNode CloneRaw(Dictionary<string, string> arguments)
        {
            AttachEndPointNode result = new AttachEndPointNode(this);

            foreach (ScriptNode child in children)
            {
                result.children.Add(child.CloneRaw(arguments));
            }

            return result;
        }

        public override void Execute(ExecutionContext context)
        {
            string bId = blockId;

            if (bId == null)
            {
                //we do not use blockId directly as it is possible to run this node 
                //multiple times (via #define) in different conditions
                bId = context.CurrentBlockId;
            }

            if (createConnector)
            {
                context.CurrentBlockWeb[bId].ProcessRequest("ProcessMetaService", BlockMetaServiceType.CreateConnector, connectorKey, null);
            }

            endPoint.Attach(context.CurrentBlockWeb[bId][connectorKey], context.CurrentBlockWeb);
        }
    }

    public class EndPointDescriptor
    {
        public object value = null;
        public string blockId = null;
        public string serviceName = null;
        public string connectorKey = null;

        public List<EndPointDescriptor> FixedArgs = new List<EndPointDescriptor>();

        public EndPointDescriptor createClone()
        {
            EndPointDescriptor result = new EndPointDescriptor();

            result.value = value;
            result.blockId = blockId;
            result.serviceName = serviceName;
            result.connectorKey = connectorKey;

            foreach (EndPointDescriptor epd in FixedArgs)
            {
                result.FixedArgs.Add(epd.createClone());
            }

            return result;
        }

        public void Attach(IConnector connector, IBlockWeb context)
        {
            string epKey = null;

            if (value != null)
            {
                epKey = connector.AttachEndPoint(value);
            }
            else if (serviceName != null)
            {
                epKey = connector.AttachEndPoint(blockId, serviceName);
            }
            else
            {
                epKey = connector.AttachConnectorEndPoint(blockId, connectorKey);
            }

            foreach (EndPointDescriptor ep in FixedArgs)
            {
                connector.AddFixedArg(epKey, ep.GetEndPoint(context));
            }
        }

        private DCRF.Contract.Connector.EndPoint GetEndPoint(IBlockWeb context)
        {
            DCRF.Contract.Connector.EndPoint result = new Connector.EndPoint(context);

            if (value != null)
            {
                result.Value = value;
            }
            else if (serviceName != null)
            {
                result.BlockId = blockId;
                result.ServiceName = serviceName;
            }
            else
            {
                result.BlockId = blockId;
                result.ConnectorKey = connectorKey;
            }

            foreach (EndPointDescriptor ep in this.FixedArgs)
            {
                result.AddFixedArg(ep.GetEndPoint(context));
            }

            return result;
        }
    }
}
