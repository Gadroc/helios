﻿//  Copyright 2019 Piet Van Nes
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

using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace GadrocsWorkshop.Helios.KeyPressReceiver
{
    public sealed class TCPClient
    {
        Socket Clientsocket;

        Thread _readThread;
        volatile bool _keepReading;

        //begin Singleton pattern
        static readonly TCPClient instance = new TCPClient();

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static TCPClient()
        {
        }

        TCPClient()
        {
            Clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _readThread = null;
            _keepReading = false;
        }

        public bool ClientConnected
        {
            get
            {
                if (Clientsocket != null)
                    return Clientsocket.Connected;
                else
                    return false;
            }
        }

        public static TCPClient Instance
        {
            get
            {
                return instance;
            }
        }
        //end Singleton pattern

        //begin Observer pattern
        public delegate void EventHandler(string param);
        public EventHandler StatusChanged;
        public EventHandler DataReceived;
        //end Observer pattern

        private void StartReading()
        {
            if (!_keepReading)
            {
                _keepReading = true;
                _readThread = new Thread(ReadPort);
                _readThread.Start();
            }
        }

        private void StopReading()
        {
            if (_keepReading)
            {
                _keepReading = false;
                if ((Clientsocket != null) && (Clientsocket.Connected)) { Clientsocket.Close(); }
                _readThread.Join(); //block until exits
                _readThread = null;
            }
        }

        /// <summary> Get the data and pass it on. </summary>
        private void ReadPort()
        {
            while (_keepReading)
            {
                if ((Clientsocket != null) && Clientsocket.Connected)
                {
                    //Read the command's Type.
                    byte[] buffer = new byte[1024];
                    try
                    {
                        int readBytes = this.Clientsocket.Receive(buffer, buffer.Length, SocketFlags.None);
                        if (readBytes == 0)
                        {
                            // Socket might have closed on the remote side - just keep reading and it will close in time
                            TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                            Thread.Sleep(waitTime);
                            continue;
                        }
                        String DataIn = System.Text.Encoding.ASCII.GetString(buffer, 0, readBytes);
                        if (DataReceived != null)
                            DataReceived(DataIn);
                    }
                    catch (SocketException)
                    {
                        // Socket closed - exit loop
                        if ((Clientsocket != null) && !Clientsocket.Connected)
                        {
                            IPEndPoint remoteIpEndPoint = null;
                            try
                            {
                                remoteIpEndPoint = Clientsocket.RemoteEndPoint as IPEndPoint;
                            }
                            catch
                            {
                                remoteIpEndPoint = null;
                            }

                            if (remoteIpEndPoint != null)
                            {
                                if (StatusChanged != null)
                                    StatusChanged(String.Format("Client disconnected from:{0}", remoteIpEndPoint.Address));
                            }
                            Clientsocket.Dispose();
                            Clientsocket = null;
                        }
                        continue;
                    }
                }
                else
                {
                    // Socket closed - exit loop
                    if ((Clientsocket != null) && !Clientsocket.Connected)
                    {
                        IPEndPoint remoteIpEndPoint = Clientsocket.RemoteEndPoint as IPEndPoint;
                        if (remoteIpEndPoint != null)
                        {
                            if (StatusChanged != null)
                                StatusChanged(String.Format("Client disconnected from:{0}", remoteIpEndPoint.Address));
                        }
                        Clientsocket.Dispose();
                        Clientsocket = null;
                    }
                    TimeSpan waitTime = new TimeSpan(0, 0, 0, 0, 50);
                    Thread.Sleep(waitTime);
                }
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state object.  
            Socket client = (Socket)ar.AsyncState;
            if (client.Connected)
                StartReading();
            try
            {
                // Complete the connection. 
                client.EndConnect(ar);
            }
            catch { }
        }

        /// <summary> Open the serial port with current settings. </summary>
        public void Open()
        {
            Close();

            try
            {
                if (Clientsocket == null)
                    Clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                SocketAsyncEventArgs e = new SocketAsyncEventArgs();
                string ValidIpAddressRegex = "^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";

                IPAddress ipAddress = IPAddress.None;
                if (Regex.IsMatch(Properties.Settings.Default.ServerAddress, ValidIpAddressRegex))
                {
                    // the string is an IP
                    ipAddress = IPAddress.Parse(Properties.Settings.Default.ServerAddress);
                }
                else
                {
                    // find IP
                    IPHostEntry ipHostInfo = Dns.GetHostEntry(Properties.Settings.Default.ServerAddress);
                    ipAddress = ipHostInfo.AddressList[0];
                    foreach (IPAddress thisAddress in ipHostInfo.AddressList)
                    {
                        if ((thisAddress.AddressFamily == AddressFamily.InterNetwork) && (thisAddress.IsIPv6LinkLocal == false))
                            ipAddress = thisAddress;
                    }
                }
                if (ipAddress == IPAddress.None)
                    return;

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, Properties.Settings.Default.ServerPort);
                e.RemoteEndPoint = remoteEP;

                //e.RemoteEndPoint = Clientsocket.RemoteEndPoint;
                Clientsocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), Clientsocket);
                // Clientsocket.Connect(Properties.Settings.Default.ServerAddress, Properties.Settings.Default.ServerPort);
                //StartReading();
            }
            catch (IOException)
            {
                if (StatusChanged != null)
                    StatusChanged(String.Format("{0} does not exist", Properties.Settings.Default.ServerAddress));
            }
            catch (UnauthorizedAccessException)
            {
                if (StatusChanged != null)
                    StatusChanged(String.Format("{0} already in use", Properties.Settings.Default.ServerPort));
            }
            catch (Exception ex)
            {
                if (StatusChanged != null)
                    StatusChanged(String.Format("{0}", ex.ToString()));
                Clientsocket.Dispose();
                Clientsocket = null;
            }
        }

        /// <summary> Close the serial port. </summary>
        public void Close()
        {
            StopReading();
            if ((Clientsocket != null) && (Clientsocket.Connected)) { Clientsocket.Close(); }
            // Svrsocket.Close();
            if (StatusChanged != null)
                StatusChanged("connection closed");
        }

        /// <summary> Get the status of the serial port. </summary>
        public bool IsOpen
        {
            get
            {
                return Clientsocket.Connected;
            }
        }

        /// <summary>Send data to the serial port after appending line ending. </summary>
        /// <param name="data">An string containing the data to send. </param>
        //This Semaphor is to protect the critical section from concurrent access of sender threads.
        System.Threading.Semaphore semaphor = new System.Threading.Semaphore(1, 1);
        public Boolean Send(string data)
        {
            try
            {
                semaphor.WaitOne();
                //Type
                byte[] buffer = new byte[data.Length];
                buffer = Encoding.ASCII.GetBytes(data);
                if ((Clientsocket != null) && Clientsocket.Connected)
                {
                    int BytesSent = Clientsocket.Send(buffer, data.Length, SocketFlags.None);
                    semaphor.Release();

                    return (Clientsocket.Connected && (BytesSent == data.Length));
                }
                else
                {
                    semaphor.Release();
                    return false;
                }
            }
            catch
            {
                semaphor.Release();
                return false;
            }
        }
    }
}
