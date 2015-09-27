using System;
using System.Collections.Generic;
using System.Text;
using DCRF.Interface;

namespace DCRF.Primitive
{
    public class BlockWebSysEventArgs
    {
        public BlockWebSysEventArgs(IBlockWeb web, BlockHandle handle)
        {
            BlockWeb = web;
            BlockHandle = handle;
        }

        public BlockWebSysEventArgs(IBlockWeb web, string id)
        {
            BlockWeb = web;
            BlockHandle = new BlockHandle();
            BlockId = id;
        }

        public IBlockWeb BlockWeb = null;
        public BlockHandle BlockHandle = null;
        public string BlockId = null;
    }

    public class BlockWebPeerSysEventArgs
    {
        public BlockWebPeerSysEventArgs(IBlockWeb web, string id, string host, int port, bool isConnect)
        {
            BlockWeb = web;
            PeerHost = host;
            PeerPort = port;
            IsConnectEvent = isConnect;
            PeerId = id;
        }

        public IBlockWeb BlockWeb = null;
        public bool IsConnectEvent = false;
        public string PeerHost = null;
        public int PeerPort = -1;
        public string PeerId = null;
    }
}
