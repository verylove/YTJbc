using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.Drawing;

namespace FakedGameClient
{
    public class SocketClient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CheckBox CheckBox { get; set; }
        public Color LogTextColor { get; set; }

        string _ip;
        int _port;
        StringBuilder _receiveSb;

        const int _bufferSize = 1024 * 40;
        byte[] _readBuffer = new byte[_bufferSize];

        Socket _client;

        bool _connected = false;
        public bool Connected
        {
            get { return _connected; }
        }
        public Socket Client
        {
            get { return _client; }
            set { _client = value; }
        }

        public delegate void LogEventHandler(object sender, string content);
        public event LogEventHandler LogEvent;

        public delegate void ReceivedDataEventHandler(object sender, byte[] buffer);
        public event ReceivedDataEventHandler ReceivedDataEvent;

        public delegate void ConnectedEventHandler(object sender);
        public event ConnectedEventHandler ConnectedEvent;

        public delegate void DisConnectedEventHandler(object sender);
        public event DisConnectedEventHandler DisConnectedEvent;

        public SocketClient()
        {
            //initSocket();
        }

        private void initSocket()
        {
            _ip = Properties.Settings.Default.Ip;
            _port = Properties.Settings.Default.Port;

            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(_ip), _port);
            _client = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            _client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _receiveSb = new StringBuilder();
        }

        public void Connect()
        {
            try
            {
                this.LogEvent(this, "连接服务器...");
                if (_client != null && _client.Connected)
                {
                    this.Disconnect();
                }
                initSocket();
                _client.BeginConnect(_ip, _port, new AsyncCallback(ConnectCallback), _client);
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "连接服务器失败,原因:" + ex.Message);
            }
        }

        public void Disconnect()
        {
            try
            {
                this.LogEvent(this, "断开连接...");
                _client.Shutdown(SocketShutdown.Both);
                _client.BeginDisconnect(true, new AsyncCallback(DisConnectCallback), _client);
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "断开连接失败,原因:" + ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.Close();
                _connected = false;
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "连接关闭失败,原因:" + ex.Message);
            }
        }

        public void Send(string content)
        {
            try
            {
                if (_client.Connected)
                {
                    byte[] buffer = Encoding.Default.GetBytes(content);
                    _client.Send(buffer);
                    this.LogEvent(this, "正在发送消息[" + content.Replace("\r\n", "<br>") + "]...");
                }
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "发送消息失败,原因:" + ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                _connected = true;
                this.LogEvent(this, "连接服务器成功...");

                AsyncReceive();
                this.ConnectedEvent(this);
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "连接服务器失败,原因:" + ex.Message);
            }
        }

        private void DisConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndDisconnect(ar);
                client.Close();
                _connected = false;
                this.LogEvent(this, "成功断开连接...");
                this.DisConnectedEvent(this);
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "断开连接失败,原因:" + ex.Message);
            }
        }

        private void AsyncReceive()
        {
            try
            {
                this.LogEvent(this, "开始异步接收数据...");
                _client.BeginReceive(_readBuffer, 0, _bufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), _client);

            }
            catch (Exception ex)
            {
                this.LogEvent(this, "开始异步接收失败,原因:" + ex.Message);
            }

        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                if (_client.Connected)
                {
                    //从远程设备读取数据  
                    int bytesRead = _client.EndReceive(ar);
                    if (bytesRead > 0)
                    {
                        // 有数据，存储.  
                        this.ReceivedDataEvent(this, _readBuffer);
                        Array.Clear(_readBuffer, 0, _readBuffer.Length);

                        // 继续读取.  
                        _client.BeginReceive(_readBuffer, 0, _bufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), _client);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogEvent(this, "接收数据失败,原因:" + ex.Message);
            }
        }

    }
}
