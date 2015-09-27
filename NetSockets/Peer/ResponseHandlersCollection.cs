using System;
using System.Collections.Generic;
using System.Text;

namespace NetSockets.Peer
{
    public delegate void MessageHandlerDelegate(PeerSocket source, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data);

    public class ResponseHandlersCollection
    {
        //key can be string (msgCode) or Guid (msgId)
        //for handling a message sent from other initiator, we set handler for that msgCode
        //for handling response sent from responder to us (initiator) we use msgId to receive response to 
        //that exact message. For example if two exactly same messages are sent to a single peer, their msgId will be different
        //so each sender object instance will receive its own response
        private Dictionary<object, MessageHandlerDelegate> handlers = new Dictionary<object, MessageHandlerDelegate>();

        public void SetHandler(Guid msgId, MessageHandlerDelegate handler)
        {
            //set a timeout to remove item
            handlers.Add(msgId, handler);
        }

        public void SetHandler(string msgCode, MessageHandlerDelegate handler)
        {
            handlers.Add(msgCode, handler);
        }

        public bool ContainsKey(object msgCode)
        {
            return handlers.ContainsKey(msgCode);
        }

        public bool ContainsKey(Guid msgId)
        {
            return handlers.ContainsKey(msgId);
        }

        public void InvokeHandler(string targetMsgCode, PeerSocket source, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            innerInvokeHandler(targetMsgCode, source, msgCode, msgId, senderId, receiverId, data);
        }

        public void InvokeHandler(Guid targetMsgId, PeerSocket source, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            innerInvokeHandler(targetMsgId, source, msgCode, msgId, senderId, receiverId, data);
        }

        private void innerInvokeHandler(object key, PeerSocket source, string msgCode, Guid msgId, string senderId, string receiverId, List<object> data)
        {
            if (handlers.ContainsKey(key))
            {
                try
                {
                    handlers[key](source, msgCode, msgId, senderId, receiverId, data);
                }
                catch (Exception exc)
                {
                    source.SendException(msgCode, msgId, exc);
                }
            }
        }

        //TODO: implement clearing of msgIds that are guid
    }
}
