using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Net;
using System.Windows.Forms;
using System.Drawing;

namespace FakedGameClient.ConnectionTest
{
    public class SocketClient
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public CheckBox CheckBox { get; set; }
        public Color LogTextColor { get; set; }

        string _ip;
        int _port;

        const int _bufferSize = 1024 * 40;
        byte[] _readBuffer = new byte[_bufferSize];

        Socket _client;

        public Socket Client
        {
            get { return _client; }
            set { _client = value; }
        }

        List<GamePlayer> _playerList = new List<GamePlayer>();
        public List<GamePlayer> PlayerList
        {
            get { return _playerList; }
            set { _playerList = value; }
        }
        public delegate void LogEventHandler(object sender, string content);
        public event LogEventHandler LogEvent;

        public delegate void ReceivedDataEventHandler(object sender, byte[] buffer);
        public event ReceivedDataEventHandler ReceivedDataEvent;

        public SocketClient()
        {
            initSocket();
        }

        private void initSocket()
        {
            _ip = Properties.Settings.Default.Ip;
            _port = Properties.Settings.Default.Port;

            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(_ip), _port);
            _client = new Socket(iep.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
            _client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _readBuffer = new byte[_bufferSize];
        }

        public void Connect()
        {
            this.LogEvent(this, "链接[" + Name + "]:正在链接到服务器...");

            try
            {
                //this.Disconnect();
                initSocket();
                _client.BeginConnect(_ip, _port, new AsyncCallback(ConnectCallback), _client);
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:链接服务器失败,原因:" + ex.Message);
            }
        }

        public void Disconnect()
        {
            this.LogEvent(this, "链接[" + Name + "]:断开链接链接...");
            try
            {
                _client.Shutdown(SocketShutdown.Both);
                _client.BeginDisconnect(true, new AsyncCallback(DisConnectCallback), _client);
            }
            catch (System.Exception ex)
            {
                //this.LogEvent(this, "链接[" + Name + "]:关闭链接失败,原因:" + ex.Message);
            }
        }

        public void Send(string content)
        {
            try
            {
                if (_client.Connected)
                {
                    byte[] buffer = Encoding.Default.GetBytes(content + "\r\n");
                    this.LogEvent(this, "链接[" + Name + "]:正在发送消息[" + content + "]...");
                    _client.Send(buffer);
                }
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:发送消息失败,原因:" + ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndConnect(ar);

                //string cmd = Properties.Settings.Default.ConnectCmd;
                //cmd = string.Format(cmd, this.ID.ToString("D4"), this.Name);
                //this.Send(cmd);
                this.CheckBox.ForeColor = Color.Blue;

                this.LogEvent(this, "链接[" + Name + "]:链接服务器成功...");
                AsyncReceive();
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:链接服务器失败,原因:" + ex.Message);
            }
        }

        private void DisConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                client.EndDisconnect(ar);
                this.CheckBox.ForeColor = Color.Red;
                _readBuffer = null;
                this.LogEvent(this, "链接[" + Name + "]:成功断开链接...");
            }
            catch (System.Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:断开链接失败,原因:" + ex.Message);
            }
        }

        private void AsyncReceive()
        {
            try
            {
                this.LogEvent(this, "链接[" + Name + "]:开始异步接收数据...");
                _client.BeginReceive(_readBuffer, 0, _bufferSize, 0, new AsyncCallback(ReceiveCallback), _client);

            }
            catch (Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:开始异步接收失败,原因:" + ex.Message);
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
                    Thread.Sleep(400);
                    if (bytesRead > 0)
                    {
                        // 有数据，存储.  
                        this.ReceivedDataEvent(this, _readBuffer);
                        Array.Clear(_readBuffer, 0, _bufferSize);
                        //_readBuffer = new byte[_bufferSize];

                        // 继续读取.  
                        _client.BeginReceive(_readBuffer, 0, _bufferSize, 0, new AsyncCallback(ReceiveCallback), _client);
                    }
                }
            }
            catch (Exception ex)
            {
                this.LogEvent(this, "链接[" + Name + "]:接收数据失败,原因:" + ex.Message);
            }
        }

    }
}
