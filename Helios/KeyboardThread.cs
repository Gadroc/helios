//  Copyright 2014 Craig Courtney
//    
//  Helios is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  Helios is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace GadrocsWorkshop.Helios
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    class KeyboardThread
    {
        private readonly Thread _thread;
        private Socket _Clientsocket;

        public Queue<NativeMethods.INPUT> _events = new Queue<NativeMethods.INPUT>();
        public int _keyDelay = 30;

        public KeyboardThread(int keyDelay)
        {
            _keyDelay = keyDelay;
            _thread = new Thread(Run);
            _thread.IsBackground = true;
            _thread.Start();
        }

        #region Properties

        public int KeyDelay
        {
            get
            {
                lock (typeof(KeyboardThread))
                {
                    return _keyDelay;
                }
            }
            set
            {
                lock (typeof(KeyboardThread))
                {
                    if (!_keyDelay.Equals(value) && value > 0)
                    {
                        _keyDelay = value;
                    }
                }
            }
        }

        #endregion

        internal void AddEvents(List<NativeMethods.INPUT> events)
        {
            lock (typeof(KeyboardThread))
            {
                bool interupt = (_events.Count == 0);

                foreach (NativeMethods.INPUT keyEvent in events)
                {
                    _events.Enqueue(keyEvent);
                }
                if (interupt)
                {
                    _thread.Interrupt();
                }
            }
        }
        private void ConnectSocketAsync(IAsyncResult result)
        {
            lock (typeof(KeyboardThread))
            {
                Socket server = (Socket)result.AsyncState;
                _Clientsocket = server.EndAccept(result);
                server.BeginAccept(ConnectSocketAsync, server); // wait for another connection
            }
        }
        private Boolean TCPSend(byte[] buf, int buflen)
        {
            try
            {
                if ((_Clientsocket != null) && _Clientsocket.Connected)
                {
                    int BytesSent = _Clientsocket.Send(buf, buflen, SocketFlags.None);
                    return (_Clientsocket.Connected && (BytesSent == buflen));
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public Boolean TCPSend(string data)
        {
            try
            {
                byte[] buffer = new byte[data.Length];
                buffer = Encoding.ASCII.GetBytes(data);
                if ((_Clientsocket != null) && _Clientsocket.Connected)
                {
                    int BytesSent = _Clientsocket.Send(buffer, data.Length, SocketFlags.None);
                    return (_Clientsocket.Connected && (BytesSent == data.Length));
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }


        public void Run()
        {
            // ToDo
            /* Kiwi.Lost.In.Melb@Gmail.com
             * Added functionality for a TCP server open on a static port of 5009 - yes needs to be in a config with edit screen but my WPF skills are NIL.
             * If there is a TCP client connected then it will send key presses to a PC running the receiver
             * Otherwise it will send the keypresses to the local PC
             * 
             * TCP code may need a clean up as I used a routine I have used many times and know it works
             * The TCP Server code was handy and a known quality
             * 
             * The way I have done this is to just serialise the INPUT object and send over TCP. Seemed the easiest way to accomodate this feature quickly.
            */
            Socket Svrsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 5009);
            Svrsocket.Bind(localEndPoint);
            Svrsocket.Listen(10);
            Svrsocket.BeginAccept(ConnectSocketAsync, Svrsocket);

            while (true)
            {
                int sleepTime = 20; // Changed from Timeout.Infinite to just delay and recheck;
                lock (typeof(KeyboardThread))
                {
                    if (_events.Count > 0)
                    {
                        sleepTime = _keyDelay;
                        NativeMethods.INPUT keyEvent = _events.Dequeue();

                        int size = Marshal.SizeOf(keyEvent);
                        byte[] arr = new byte[size];
                        IntPtr ptr = Marshal.AllocHGlobal(size);
                        Marshal.StructureToPtr(keyEvent, ptr, true);
                        Marshal.Copy(ptr, arr, 0, size);
                        // Send via TCP - if no connection then send to the local PC
                        if (!TCPSend(arr, size))
                            NativeMethods.SendInput(1, new NativeMethods.INPUT[] { keyEvent }, Marshal.SizeOf(keyEvent));
                        Marshal.FreeHGlobal(ptr);
                    }
                    else if (_Clientsocket != null)
                    {
                        // check TCP read thread
                        try
                        {
                            byte[] buffer = new byte[1024];
                            _Clientsocket.ReceiveTimeout = 1; // so we dont block - will cause an exception
                            int readBytes = _Clientsocket.Receive(buffer, buffer.Length, SocketFlags.None);
                            if (readBytes != 0)
                            {
                                String DataIn = System.Text.Encoding.ASCII.GetString(buffer, 0, readBytes);
                                if (DataIn.Contains("HEARTBEAT"))
                                    // Send a response - a heatbeat is used because we dont always know when TCP disconnects
                                    // This allows the client to know whether it is still in a connected state
                                    TCPSend("HEARTBEAT");
                            }
                        }
                        catch (SocketException)
                        {
                            // Socket read timeout - ignore
                        }
                    }
                }
                try
                {
                    Thread.Sleep(sleepTime);
                }
                catch (ThreadInterruptedException)
                {
                    // NOOP
                }
            }
        }
    }
}
