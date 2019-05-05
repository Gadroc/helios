using System;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace GadrocsWorkshop.Helios
{
  public sealed class KeyboardTCPServer
  {
    Socket Svrsocket;
    Socket Clientsocket;

    Thread _readThread;
    volatile bool _keepReading;

    //begin Singleton pattern
    static readonly KeyboardTCPServer instance = new KeyboardTCPServer();


    // I have set a static port of 5009.  This is just arbitrary and could easily be changed or added to a config file
    private int TCPPort = 5009; 

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static KeyboardTCPServer()
    {
    }

    KeyboardTCPServer()
    {
      Svrsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
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

    public static KeyboardTCPServer Instance
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
        Svrsocket.BeginAccept(ConnectSocketAsync, Svrsocket);
      }
    }

    private void StopReading()
    {
      if (_keepReading)
      {
        _keepReading = false;
        if ((Clientsocket != null) && (Clientsocket.Connected)) { Clientsocket.Close(); }
        _readThread.Join();	//block until exits
        _readThread = null;

        if ((Svrsocket != null) && (Svrsocket.Connected)) Svrsocket.Shutdown(SocketShutdown.Both);
        //Svrsocket.EndAccept(ConnectSocketAsync);
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
              IPEndPoint remoteIpEndPoint = Clientsocket.RemoteEndPoint as IPEndPoint;
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

    /// <summary> Get a new connection. </summary>
    private void ConnectSocketAsync(IAsyncResult result)
    {
      Socket server = (Socket)result.AsyncState;
      Clientsocket = server.EndAccept(result);

      IPEndPoint remoteIpEndPoint = Clientsocket.RemoteEndPoint as IPEndPoint;
      if (remoteIpEndPoint != null)
      {
        if (StatusChanged != null)
          StatusChanged(String.Format("Client connected from:{0}", remoteIpEndPoint.Address));
      }
      server.BeginAccept(ConnectSocketAsync, server); // wait for another connection
    }

    /// <summary> Open the TCP port with current settings. </summary>
    public void Open()
    {
      Close();

      try
      {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, TCPPort);
        Svrsocket.Bind(localEndPoint);
        Svrsocket.Listen(1);

        StartReading();
      }
      catch (IOException)
      {
        if (StatusChanged != null)
          StatusChanged(String.Format("{0} does not exist", TCPPort));
      }
      catch (UnauthorizedAccessException)
      {
        if (StatusChanged != null)
          StatusChanged(String.Format("{0} already in use", TCPPort));
      }
      catch (Exception ex)
      {
        if (StatusChanged != null)
          StatusChanged(String.Format("{0}", ex.ToString()));
      }

      // Update the status
      if (Svrsocket.IsBound)
      {
        IPEndPoint localEndPoint = Svrsocket.LocalEndPoint as IPEndPoint;
        if (StatusChanged != null)
          StatusChanged(String.Format("Listening on port {0}",localEndPoint.Port));
      }
      else
      {
        IPEndPoint localEndPoint = Svrsocket.LocalEndPoint as IPEndPoint;
        if (StatusChanged != null)
          StatusChanged(String.Format("Port {0} already in use", localEndPoint.Port));
      }
    }

    /// <summary> Close the TCP port. </summary>
    public void Close()
    {
      StopReading();
      if ((Clientsocket != null) && (Clientsocket.Connected)) { Clientsocket.Close(); }
      if (Svrsocket.Connected) { Svrsocket.Disconnect(true); }
      // Svrsocket.Close();
      if (StatusChanged != null)
        StatusChanged("connection closed");
    }

    /// <summary> Get the status of the TCP port. </summary>
    public bool IsOpen
    {
      get
      {
        return Svrsocket.IsBound;
      }
    }

    /// <summary>Send data to the TCP port after appending line ending. </summary>
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
    public Boolean Send(byte[] buf, int buflen)
    {
      try
      {
        semaphor.WaitOne();
        if ((Clientsocket != null) && Clientsocket.Connected)
        {
          int BytesSent = Clientsocket.Send(buf, buflen, SocketFlags.None);
          semaphor.Release();

          return (Clientsocket.Connected && (BytesSent == buflen));
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
