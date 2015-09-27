﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace NetSockets
{
    public static class NetUtility
    {
        public static int ParsePort(string s)
        {
            return int.Parse(s);
        }
        public static bool TryParsePort(string s, out int port)
        {
            try
            {
                port = ParsePort(s);
                return true;
            }
            catch
            {
                port = 0;
                return false;
            }
        }

        public static bool Ping(string host, int port)
        {
            return Ping(host, port, TimeSpan.MaxValue);
        }
        public static bool Ping(string host, int port, out TimeSpan elapsed)
        {
            return Ping(host, port, TimeSpan.MaxValue, out elapsed);
        }
        public static bool Ping(string host, int port, TimeSpan timeout)
        {
            TimeSpan elapsed;
            return Ping(host, port, timeout, out elapsed);
        }
        public static bool Ping(string host, int port, TimeSpan timeout, out TimeSpan elapsed)
        {
            using (TcpClient tcp = new TcpClient())
            {
                DateTime start = DateTime.Now;
                IAsyncResult result = tcp.BeginConnect(host, port, null, null);
                WaitHandle wait = result.AsyncWaitHandle;
                bool ok = true;

                try
                {
                    if (!result.AsyncWaitHandle.WaitOne(timeout, false))
                    {
                        tcp.Close();
                        ok = false;
                    }

                    tcp.EndConnect(result);
                }
                catch
                {
                    ok = false;
                }
                finally
                {
                    wait.Close();
                }

                DateTime stop = DateTime.Now;
                elapsed = stop.Subtract(start);
                return ok;
            } 
        }
    }
}
