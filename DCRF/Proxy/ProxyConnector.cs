using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Contract;


///TODO: according to recent changes in IBlockWeb (e.g. no global connector) update this class and related classes
namespace DCRF.Proxy
{
    public class ProxyConnector : IConnector
    {
        private ProxyBlockWeb parentWeb = null;
        private ProxyBlock parentBlock = null;
        private string key = null;

        public ProxyConnector(string k, ProxyBlockWeb parentW, ProxyBlock parentB)
        {
            key = k;
            parentWeb = parentW;
            parentBlock = parentB;
        }

        public string AttachEndPoint(string blockId, string serviceName)
        {
            //if (parentBlock != null)
            //{
            //    return (string)parentWeb.CallConnectorMethod(parentBlock.Id, key, "AttachEndPointG", blockId, serviceName);
            //}
            //else
            //{
            //    return (string)parentWeb.CallConnectorMethod(null, key, "AttachEndPointG", blockId, serviceName);
            //}
            return null;
        }

        public string AttachEndPoint(object value)
        {
            //if (parentBlock != null)
            //{
            //    return (string)parentWeb.CallConnectorMethod(parentBlock.Id, key, "AttachEndPointV", value);
            //}
            //else
            //{
            //    return (string)parentWeb.CallConnectorMethod(null, key, "AttachEndPointV", value);
            //}

            return null;
        }

        public string AttachConnectorEndPoint(string blockId, string chainConnectorKey)
        {
            //if (parentBlock != null)
            //{
            //    return (string)parentWeb.CallConnectorMethod(parentBlock.Id, key, "AttachConnectorEndPoint", blockId, chainConnectorKey);
            //}
            //else
            //{
            //    return (string)parentWeb.CallConnectorMethod(null, key, "AttachConnectorEndPoint", blockId, chainConnectorKey);
            //}

            return null;
        }

        public void DetachEndPoint(string epkey)
        {
            //if (parentBlock != null)
            //{
            //    parentWeb.CallConnectorMethod(parentBlock.Id, key, "DetachEndPoint", epkey);
            //}
            //else
            //{
            //    parentWeb.CallConnectorMethod(null, key, "DetachEndPoint", epkey);
            //}
        }

        public List<object> ProcessRequest(params object[] args)
        {
            //if (parentBlock != null)
            {
                throw new Exception("You cannot call an internal connector of a block");
            }
            //else
            //{
            //    return (List<object>)parentWeb.CallConnectorMethod(null, key, "ProcessRequestAll", args);
            //}
        }


        public bool HasEndPoint(string blockId, string serviceName)
        {
            //if (parentBlock != null)
            //{
            //    return (bool)parentWeb.CallConnectorMethod(parentBlock.Id, key, "HasEndPoint", blockId, serviceName);
            //}
            //else
            //{
            //    return (bool)parentWeb.CallConnectorMethod(null, key, "HasEndPoint", blockId, serviceName);
            //}

            return false;
        }


        //public void AddFixedArg(string endpointKey, Connector.EndPoint arg)
        //{
        //    //if (parentBlock != null)
        //    //{
        //    //    parentWeb.CallConnectorMethod(parentBlock.Id, key, "AddFixedArg", endpointKey, arg);
        //    //}
        //    //else
        //    //{
        //    //    parentWeb.CallConnectorMethod(null, key, "AddFixedArg", endpointKey, arg);
        //    //}
        //}

        public T GetValue<T>(params object[] args)
        {
            List<object> result = ProcessRequest(args);

            if (result.Count == 0) return default(T);

            return (T)result[0];
        }

        public T GetValue<T>(T defaultValue, params object[] args)
        {
            List<object> result = ProcessRequest(args);

            if (result.Count == 0) return defaultValue;

            return (T)result[0];
        }

        bool IConnector.AttachEndPoint(string blockId, string serviceName)
        {
            throw new NotImplementedException();
        }

        bool IConnector.AttachEndPoint(object value)
        {
            throw new NotImplementedException();
        }

        bool IConnector.AttachConnectorEndPoint(string blockId, string chainConnectorKey)
        {
            throw new NotImplementedException();
        }

        object IConnector.ProcessRequest(params object[] args)
        {
            throw new NotImplementedException();
        }
    }
}
