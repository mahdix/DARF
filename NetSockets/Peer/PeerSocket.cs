//TODO: rewrite this class and make it more tidy, compact, readable
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NetSockets;
using System.Net;

namespace NetSockets.Peer
{

    public class PeerSocket : IDisposable
    {
        #region private fields
        public static Random random = new Random();

        private const int defaultTimeout = 5000;

        private BinaryFormatter formatter = new BinaryFormatter();
        private NetServer serverSocket = null;
        private NetClient clientSocket = null;  //req does not need a thread, we wait for ACK after sending messages in the method
        private const string defaultHost = "127.0.0.1";

        //key can be msgCode or msgId
        //this is given to me by my container
        private ResponseHandlersCollection responseHandlers = null;
        private string parentId = "";
        private string peerId = "";

        #endregion

        #region public methods

        /// <summary>
        /// This will create a server socket on a random port, used to initiate a connection
        /// we send address of this server to the remote server to connect using a client socket
        /// The only exception is gateway socket that has no peerId (is null)
        /// </summary>
        /// <param name="_parentId"></param>
        /// <param name="_peerId"></param>
        public PeerSocket(ResponseHandlersCollection handlers, string _parentId, string _peerId)
        {
            responseHandlers = handlers;

            parentId = _parentId;
            peerId = _peerId;

            string myName = "";

            if (peerId == null)
            {
                myName = _parentId + ".Gateway";
            }
            else
            {
                myName = parentId + "." + peerId + ".Server";
            }

            serverSocket = new NetServer(myName);
            serverSocket.OnReceived += new NetClientReceivedEventHandler<byte[]>(onServerReceiveData);
            serverSocket.Start(IPAddress.Parse(defaultHost), random.Next(1025, 65500));
        }

        /// <summary>
        /// This will create a client socket and connect to a remote server socket which is prepared from the 
        /// connect initiator
        /// </summary>
        /// <param name="targetAddress"></param>
        public PeerSocket(ResponseHandlersCollection handlers, string _parentId, string _peerId, string remoteHost, int remotePort)
        {
            responseHandlers = handlers;

            parentId = _parentId;
            peerId = _peerId;

            clientSocket = new NetClient(parentId+"."+ peerId+".Client");
            clientSocket.OnReceived += new NetReceivedEventHandler<byte[]>(onClientReceiveData);
            clientSocket.TryConnect(remoteHost, remotePort);
        }        

        void onServerReceiveData(object sender, NetClientReceivedEventArgs<byte[]> e)
        {
            onReceiveData(e.Data);
        }

        void onClientReceiveData(object sender, NetReceivedEventArgs<byte[]> e)
        {
            onReceiveData(e.Data);
        }

        private void onReceiveData(byte[] data)
        {
            MemoryStream ms = new MemoryStream(data);
            List<object> msg = formatter.Deserialize(ms) as List<object>;
            ThreadPool.QueueUserWorkItem(new WaitCallback(processMessage), msg);
        }

        public List<object> SendMessage(string msgCode, params object[] items)
        {
            List<object> list = null;

            if (items != null)
            {
                list = new List<object>(items);
            }
            else
            {
                //maybe just one null value is passed
                list = new List<object>() { null };
            }

            return SendMessageList(msgCode, list);
        }

        public List<object> SendMessageList(string msgCode, List<object> list)
        {
            List<object> result = null;

            Guid msgId = Guid.NewGuid();
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            //if caller requires the response, set it up and remove it after timeout of the call
            responseHandlers.SetHandler(msgId, new MessageHandlerDelegate(
                    delegate(PeerSocket sender, string rmsgCode, Guid imsgId, string senderId, string receiverId, List<object> data)
                    {
                        result = data;
                        waitHandle.Set();
                    }));

            SendMessageAsyncList(msgCode, msgId, list);

            bool gotSignal = false;

            //in the first message, we need to check timeout because no heartbeat mechanism is setup yet
            bool checkTimeout = (msgCode == PeerMsgDef.ConnectMsgCode);
            int timeoutCounter = 0;
            //wait indefinitely but if meanwhile this socket is disposed, throw exception
            //this socket is disposed if its peer is dead (no heartbeat received)
            do
            {
                gotSignal = waitHandle.WaitOne(PeerMsgDef.SendMessageWaitResponseTimeout);

                if (checkTimeout)
                {
                    timeoutCounter += PeerMsgDef.SendMessageWaitResponseTimeout;

                    if (timeoutCounter > PeerMsgDef.ConnectTimeout)
                    {
                        break;
                    }
                }

            } while (!gotSignal && (clientSocket != null || serverSocket != null));

            if (gotSignal)
            {
                //last item is senderId. before that we have message type
                MsgType messageType = (MsgType)result[result.Count - 1];
                result.RemoveAt(result.Count - 1);

                if (messageType == MsgType.Normal)
                {
                    //if no one has sent a response, data is inact and is null
                    return result;
                }
                else if (messageType == MsgType.Exception)
                {
                    string type = (string)result[0];
                    string message = (string)result[1];
                    throw new RemoteException(type, message);
                }
            }

            throw new TimeoutException("Timeout calling " + msgCode + " on " + PeerId);
        }

        /// <summary>
        /// This method is used to send response
        /// </summary>
        /// <param name="msgCode"></param>
        /// <param name="msgId"></param>
        /// <param name="items"></param>
        public void SendMessageAsync(string msgCode, Guid msgId, params object[] items)
        {
            List<object> list = null;

            if (items != null)
            {
                list = new List<object>(items);
            }
            else
            {
                //maybe just one null value is passed
                list = new List<object>() { null };
            }

            SendMessageAsyncList(msgCode, msgId, list);
        }

        public void SendMessageAsyncList(string msgCode, Guid msgId, List<object> list)
        {
            SendMessageAsyncList(msgCode, msgId, list, MsgType.Normal);
        }

        public void SendMessageAsyncList(string msgCode, Guid msgId, List<object> list, MsgType messageType)
        {
            list.Insert(0, peerId);
            list.Insert(0, parentId);
            list.Insert(0, msgCode);
            list.Insert(0, msgId);

            //add type to the end, normal messages will be handeled in ResponseHandlersCollection
            //byt responses (including exceptions) are processed in anonymous methods in this class
            list.Add(messageType);

            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, list);

            if ( clientSocket != null && clientSocket.IsConnected)
            {
                clientSocket.Send(ms.GetBuffer());
            }
            else if ( serverSocket != null && serverSocket.Clients.Length == 1)
            {
                serverSocket.ClientStreams[serverSocket.Clients[0]].Send(ms.GetBuffer());
            }
        }

        //this method is called as a response to a request when some exception has happened
        public void SendException(string msgCode, Guid msgId, Exception exc )
        {
            List<object> list = new List<object>();
            list.Add(exc.GetType().Name);
            list.Add(exc.Message);

            SendMessageAsyncList(msgCode, msgId, list, MsgType.Exception);
        }

        public void Dispose()
        {
            //if client is sending some data, wait for finish
            if (clientSocket != null)
            {
                clientSocket.Disconnect();
                clientSocket = null;
            }

            if (serverSocket != null)
            {
                serverSocket.DisconnectAll();
                serverSocket.Stop();
                serverSocket = null;
            }
        }

        #endregion

        #region public properties

        public string Address
        {
            get
            {
                if (clientSocket != null)
                {
                    return null;
                }

                if (serverSocket != null)
                {
                    return string.Format("{0}:{1}", Host, Port);
                }

                return null;
            }
        }

        public string Host
        {
            get
            {
                if (clientSocket != null)
                {
                    return null;
                }

                if (serverSocket != null)
                {
                    return serverSocket.Address.ToString();
                }

                return null;
            }
        }

        public int Port
        {
            get
            {
                if (clientSocket != null)
                {
                    return -1;
                }

                if (serverSocket != null)
                {
                    return serverSocket.Port;
                }

                return -1;
            }
        }

        public string ParentId
        {
            get
            {
                return parentId;
            }
        }

        public string PeerId
        {
            get
            {
                return peerId;
            }
        }

        public bool IsConnected
        {
            get
            {
                if (clientSocket != null)
                {
                    return clientSocket.IsConnected;
                }

                if (serverSocket != null)
                {
                    return (serverSocket.ClientCount != 0);
                }

                return false;
            }
        }

        public int PeerPort
        {
            get
            {
                if (clientSocket != null)
                {
                    return clientSocket.RemotePort;
                }

                if (serverSocket != null && serverSocket.ClientCount == 1)
                {
                    Guid clientId = serverSocket.Clients[0];
                    return (serverSocket.ClientStreams[clientId].EndPoint as IPEndPoint).Port;
                }

                return -1;
            }
        }

        public string PeerHost
        {
            get
            {
                if (clientSocket != null)
                {
                    return clientSocket.RemoteHost;
                }

                if (serverSocket != null && serverSocket.Clients.Length == 1)
                {
                    Guid clientId = serverSocket.Clients[0];
                    EndPoint ep = serverSocket.ClientStreams[clientId].EndPoint;

                    return (serverSocket.ClientStreams[clientId].EndPoint as IPEndPoint).Address.ToString();
                }

                return null;
            }
        }

        public string PeerAddress
        {
            get
            {
                return string.Format("{0}:{1}", PeerHost, PeerPort);
            }
        }

        #endregion

        #region private methods

        private void processMessage(object data)
        {
            List<object> msg = data as List<object>;
            Guid msgId = (Guid)msg[0];
            string code = (string)msg[1];
            string senderId = (string)msg[2];
            string receiverId = (string)msg[3];

            logEvent(parentId + " received " + code);

            msg.RemoveAt(0);
            msg.RemoveAt(0);
            msg.RemoveAt(0);
            msg.RemoveAt(0);

            //am I the receiver of this message?
            if (receiverId == null || receiverId == this.parentId)
            {
                //is my peer sender of the message? (in a gateway socket, peerId is null)
                if (this.peerId == null || senderId == this.peerId)
                {
                    if (responseHandlers.ContainsKey(msgId))
                    {
                        responseHandlers.InvokeHandler(msgId, this, code, msgId, senderId, receiverId, msg);
                    }
                    else if (responseHandlers.ContainsKey(code))
                    {
                        responseHandlers.InvokeHandler(code, this, code, msgId, senderId, receiverId, msg);
                    }
                }
            }

        }

        #endregion

        private void logEvent(string log)
        {
            //StreamWriter sw = new StreamWriter("C:\\peerSocket.log", true);
            //sw.WriteLine(log);
            //sw.Close();
        }
    }
}
