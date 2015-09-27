using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

namespace NetSockets.Peer
{
    public delegate void PeerConnectDelegate(string peerId, bool isConnected, string peerHost, int peerPort);
    public delegate void PeerLogDelegate(int type, string log);

    public class PeerManager : IDisposable
    {
        #region private fields
        private PeerSocket gatewaySocket = null;
        private PeerLogDelegate logMethod = null;

        //key is peerId and value is the related peerSocket,
        //If i was the initiator of the peer connection, this will be a server or else client
        private Dictionary<string, PeerSocket> peers = new Dictionary<string, PeerSocket>();
        private Dictionary<string, DateTime> peersExpireDates = new Dictionary<string, DateTime>();

        private ResponseHandlersCollection responseHandlers = new ResponseHandlersCollection();
        private Thread heartbeatThread = null;
        #endregion

        public event PeerConnectDelegate PeerConnected;
        public event PeerConnectDelegate PeerDisconnected;

        public PeerManager(string parentId): this(parentId,null)
        {
        }

        public PeerManager(string parentId,PeerLogDelegate logMethod )
        {
            //we may receive connect request by sockets other than gateway, for example
            //when a adminConsole blockweb tries to order remote BW to connect to a third BW
            responseHandlers.SetHandler(PeerMsgDef.ConnectMsgCode, new MessageHandlerDelegate(onConnectRequestReceived));
            responseHandlers.SetHandler(PeerMsgDef.DisconnectMsgCode, new MessageHandlerDelegate(onDisconnectReceived));
            responseHandlers.SetHandler(PeerMsgDef.HeartbeatMsgCode, new MessageHandlerDelegate(onHeartbeatReceived));

            gatewaySocket = new PeerSocket(responseHandlers, parentId, null);

            heartbeatThread = new Thread(new ThreadStart(heartbeatSender));
            heartbeatThread.Start();

            this.logMethod = logMethod;

            logEvent("Create PM(" + parentId + ") - Gateway Address = " + this.Address);
        }

        public bool HasPeer(string id)
        {
            return peers.ContainsKey(id);
        }

        #region Connect/Disconnect methods

        public bool Connect(string host, int port, string peerId)
        {
            if (gatewaySocket == null) return false;

            //step 1 - create a server peerSocket 
            PeerSocket myServerSocket = new PeerSocket(responseHandlers, gatewaySocket.ParentId, peerId);

            //step 2 - create a temp client peer socket to send my address to the peer's gateway
            PeerSocket tempClientSocket = new PeerSocket(responseHandlers, gatewaySocket.ParentId, peerId, host, port);
            
            //step 3 - use temp client socket to send my server socket address to peer
            List<object> response = null;

            try
            {
                response = tempClientSocket.SendMessage(PeerMsgDef.ConnectMsgCode, myServerSocket.Host, myServerSocket.Port, gatewaySocket.Host, gatewaySocket.Port);
            }
            catch (Exception exc)
            {
                myServerSocket.Dispose();
                myServerSocket = null;

                throw;
            }
            finally
            {
                //step 4 - dispose temp socket
                tempClientSocket.Dispose();
                tempClientSocket = null;
            }

            //step 5 - was connect successfull?
            if (response == null)
            {
                myServerSocket.Dispose();
                myServerSocket = null;
                return false;
            }

            //step 6 - extract address that peer has prepared for me (peer will connect to my server socket automatically)
            bool done = (bool)response[0];

            //step 7 - check correctness of the address
            if (!myServerSocket.IsConnected || !done)
            {
                myServerSocket.Dispose();
                myServerSocket = null;
                return false;
            }

            //step 8 - store peer socket
            peers.Add(peerId, myServerSocket);
            peersExpireDates[peerId] = DateTime.Now.AddMilliseconds(PeerMsgDef.HeartbeatValidness);

            logEvent("PM (" + gatewaySocket.ParentId + ") Connecting- PeerAddress = " + myServerSocket.PeerAddress + " PeerId = " + peerId);

            return true;
        }

        private void onConnectRequestReceived(PeerSocket sender, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            //step 1 - read the other-side address
            string otherHost = (string)data[0];
            int otherPort = (int)data[1];

            //we use these to store info
            string otherGatewayHost = (string) data[2];
            int otherGatewayPort = (int)data[3];

            logEvent("PM (" + gatewaySocket.ParentId + ") - Receive Connect Request - PeerAddress = " + otherHost+":"+otherPort.ToString() + ", PeerId = " + senderId);

            //step 2 - create a client peer socket and connect to that address
            PeerSocket myClientSocket = new PeerSocket(responseHandlers, gatewaySocket.ParentId, senderId, otherHost, otherPort);

            //step 3 - check if client is connected successfully
            if (!myClientSocket.IsConnected || (myClientSocket.PeerHost != otherHost) ||
                (myClientSocket.PeerPort != otherPort))
            {
                myClientSocket.Dispose();
                myClientSocket = null;

                return;
            }

            //step 4 - add this client socket to peers
            peers.Add(senderId, myClientSocket);
            peersExpireDates[senderId] = DateTime.Now.AddMilliseconds(PeerMsgDef.HeartbeatValidness);

            //step 5 - let sender know that I am connected
            sender.SendMessageAsync(msgCode, msgId, true);

            if (PeerConnected != null)
            {
                PeerConnected(senderId, true, otherGatewayHost, otherGatewayPort );
            }
        }

        private void onDisconnectReceived(PeerSocket sender, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            //if the sender is my gateway ignore this, as temp sockets will always disconnect from it ASAP
            if (sender == gatewaySocket) return;

            //dispose and remove my socket
            sender.Dispose();

            peers.Remove(senderId);
            peersExpireDates.Remove(senderId);

            if (PeerDisconnected != null)
            {
                //upon disconnect, we know host and port of peer
                PeerDisconnected(senderId, false, null, -1);
            }
        }

        public void Disconnect()
        {
            string[] peerKeys = new string[peers.Count];

            peers.Keys.CopyTo(peerKeys, 0);

            foreach (string peer in peerKeys)
            {
                Disconnect(peer);
            }
        }

        public void Disconnect(string peerId)
        {
            if (gatewaySocket == null || !peers.ContainsKey(peerId)) return;

            logEvent("PM (" + Address + ") Disconnecting from " + peerId);

            //inform peer that I want to disconnect
            SendMessageAsync(peerId, PeerMsgDef.DisconnectMsgCode);

            //wait a little so message is delivered
            Thread.Sleep(200);

            if (peers.ContainsKey(peerId))
            {
                PeerSocket peerSocket = peers[peerId];
                peers.Remove(peerId);
                peersExpireDates.Remove(peerId);
                peerSocket.Dispose();
                peerSocket = null;
            }
        }

        public void Dispose()
        {
            Disconnect();

            gatewaySocket.Dispose();
            gatewaySocket = null;

            heartbeatThread.Abort();
        }

        #endregion

        #region Send Message
        /// <summary>
        /// in this method, no msgId is required. Caller does not need to concern about id of its message.
        /// PeerManager will handle to create and maintain id of the message and parse the result and 
        /// finally return the result of the call.
        /// </summary>
        /// <param name="msgCode"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public List<object> BroadcastMessage(string msgCode, params object[] items)
        {
            return BroadcastMessageList(msgCode, new List<object>(items));
        }

        public List<object> BroadcastMessageList(string msgCode, List<object> items)
        {
            if (peers.Count == 0 || gatewaySocket == null) return null;
            
            List<object> result = null;

            Guid msgId = Guid.NewGuid();
            ManualResetEvent waitHandle = new ManualResetEvent(false);

            //if caller requires the response, set it up and remove it after timeout of the call
            responseHandlers.SetHandler(msgId, new MessageHandlerDelegate(
                    delegate(PeerSocket sender, string rmsgCode, Guid imsgId, string senderId, string receiverId, List<object> data)
                    {
                        result = data;

                        //add the id of the sender of the response at the end of result list
                        result.Add(senderId);

                        waitHandle.Set();
                    }));

            foreach (string key in peers.Keys)
            {
                peers[key].SendMessageAsyncList(msgCode, msgId, items);
            }

            bool gotSignal = waitHandle.WaitOne(PeerMsgDef.BroadcastMessageWaitResponseTimeout);

            if (gotSignal)
            {
                //if no one has sent a response, data is inact and is null
                return result;
            }

            throw new Exception("Timeout occured waiting for response to " + msgCode + " broadcast");
        }



        public void BroadcastMessageAsync(string msgCode, params object[] items)
        {
            BroadcastMessageAsyncList(msgCode, new List<object>(items));
        }

        public void BroadcastMessageAsyncList(string msgCode, List<object> items)
        {
            if (peers.Count == 0 || gatewaySocket == null) return;

            Guid msgId = Guid.NewGuid();
        
            foreach (string key in peers.Keys)
            {
                peers[key].SendMessageAsyncList(msgCode, msgId, items);
            }
        }

        public List<object> SendMessage(string peerId, string msgCode, params object[] items)
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

            return SendMessageList(peerId, msgCode, list);
        }

        public List<object> SendMessageList(string peerId, string msgCode, List<object> items)
        {
            if (peers.Count == 0 || gatewaySocket == null || !peers.ContainsKey(peerId)) return null;

            logEvent(gatewaySocket.ParentId + " is sending " + msgCode + " to " + peerId);

            return peers[peerId].SendMessageList(msgCode, items);
        }

        public void SendMessageAsync(string peerId, string msgCode, params object[] items)
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

            SendMessageAsyncList(peerId, msgCode, list);
        }

        public void SendMessageAsyncList(string peerId, string msgCode, List<object> items)
        {
            if (peers.Count == 0 || gatewaySocket == null || !peers.ContainsKey(peerId)) return;

            logEvent(gatewaySocket.ParentId + " is async-sending " + msgCode + " to " + peerId);

            peers[peerId].SendMessageAsyncList(msgCode, Guid.Empty, items);
        }

        #endregion

        #region Handlers Management

        public void SetHandler(string msgCode, MessageHandlerDelegate handler)
        {
            responseHandlers.SetHandler(msgCode, handler);
        }

        #endregion

        #region Public properties

        public int PeerCount
        {
            get
            {
                return this.peers.Count;
            }
        }

        public string Address
        {
            get
            {
                return gatewaySocket.Address;
            }
        }

        public string Host
        {
            get
            {
                return gatewaySocket.Host;
            }
        }


        public int Port
        {
            get
            {
                return gatewaySocket.Port;
            }
        }

        #endregion

        private void logEvent(string log)
        {
            if (logMethod != null)
            {
                //-1 is peermanager
                logMethod(-1, log);
            }
        }

        private void logEvent(int type, string log)
        {
            if (logMethod != null)
            {
                //-1 is peermanager
                logMethod(type, log);
            }
        }


        private void heartbeatSender()
        {
            while (true)
            {
                string[] peerKeys = new string[peers.Count];

                peers.Keys.CopyTo(peerKeys, 0);

                foreach (string peer in peerKeys)
                {
                    try
                    {
                        if (peersExpireDates[peer] < DateTime.Now)
                        {
                            logEvent(-2, gatewaySocket.ParentId + " is disconnecting from dead peer: " + peer);

                            //seems that that peer is dead
                            Disconnect(peer);
                        }
                        else
                        {
                            peers[peer].SendMessageAsync(PeerMsgDef.HeartbeatMsgCode, Guid.Empty);
                            logEvent(-2, gatewaySocket.ParentId + " sending heartbeat to " + peer);
                        }
                    }
                    catch (KeyNotFoundException exc)
                    {
                        //just ignore keys that are deleted from peers list during loop
                    }
                }

                Thread.Sleep(PeerMsgDef.HeartbeatInterval);
            }
        }

        private void onHeartbeatReceived(PeerSocket sender, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            //renew expire date of peer
            peersExpireDates[senderId] = DateTime.Now.AddMilliseconds(PeerMsgDef.HeartbeatValidness);

            logEvent(-2, gatewaySocket.ParentId + " got heartbeat from " + senderId);
        }

    }
}
