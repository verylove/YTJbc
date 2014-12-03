using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace FakedGameClient.ConnectionTest
{
    public partial class FormMain : Form
    {
        int _nextClientID = 1;
        List<SocketClient> _tcpclients;
        int _count = 0;

        FormPlayers _playInfoForm = new FormPlayers();

        public FormMain()
        {
            InitializeComponent();

            skinEngine1.SkinFile = Application.StartupPath + "\\MacOS.ssk";

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //chkMulti.Checked = Properties.Settings.Default.IsMulti;
            //initSockets();
            _tcpclients = new List<SocketClient>();
            AddSocketClient();
            //_thread = new Thread(new ThreadStart(ThreadFunc));
            //_thread.Start();
        }

        private void AddSocketClient()
        {
            SocketClient client = new SocketClient();
            client.ID = _nextClientID;
            client.Name = _nextClientID.ToString("D4");
            client.LogEvent += _tcpClient_LogEvent;
            client.ReceivedDataEvent += _tcpClient_ReceivedDataEvent;

            CheckBox chk = new CheckBox();
            flowPnl.Controls.Add(chk);
            chk.ForeColor = Color.Red;
            chk.AutoSize = true;
            chk.Text = client.Name;
            chk.Checked = true;
            client.CheckBox = chk;
            int count = _tcpclients.Count;
            count = 35 + count % 144;

            client.LogTextColor = Color.FromKnownColor((KnownColor)count);
            _tcpclients.Add(client);

            _nextClientID++;
        }

        private void log(object sender, string content)
        {
            content = content.Replace("\r\n", "**");
            txtLog.AppendText(content + Environment.NewLine);
            txtLog.Select(txtLog.Text.Length - content.Length - 1, content.Length);
            txtLog.SelectionColor = ((SocketClient)sender).LogTextColor;
            if (chkAutoScroll.Checked)
            {
                txtLog.ScrollToCaret();
            }
        }

        private void _tcpClient_LogEvent(object sender, string content)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(() =>
                {
                    log(sender, content);
                }));
            }
            else
            {
                log(sender, content);
            }
        }

        private void _tcpClient_ReceivedDataEvent(object sender, byte[] buffer)
        {
            SocketClient client = (SocketClient)sender;
            string tmp = Encoding.Default.GetString(buffer);

            tmp = tmp.Substring(0, tmp.IndexOf('\0') - 2);

            this.BeginInvoke(new MethodInvoker(() =>
            {
                log(sender, "客户端[" + client.Name + "]接收指令:[" + tmp + "]");

                if (!string.IsNullOrEmpty(tmp))
                {
                    updatePlayers(sender, tmp);
                    _playInfoForm.PlayerList = client.PlayerList.ToList();
                    _count++;
                    Console.WriteLine("count:" + _count.ToString());

                    if (_count >= _tcpclients.Count)
                    {
                        _playInfoForm.UpdatePlayers();
                        _count = 0;
                    }
                }
            }));
        }

        private void SendCmd(SocketClient client, string cmd)
        {
            if (client.CheckBox.Checked)
            {
                cmd = string.Format(cmd, client.ID.ToString("D4"), client.Name);
                client.Send(cmd);
            }
        }

        #region 窗体事件

        private void btnConnect_Click(object sender, EventArgs e)
        {
            foreach (var client in _tcpclients)
            {
                if (client.CheckBox.Checked)
                {
                    //var thread = new Thread(new ThreadStart(client.Connect));
                    //thread.Start();
                    client.Connect();
                    //client.ThreadStart();
                }
            }
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            foreach (var client in _tcpclients)
            {
                if (client.CheckBox.Checked)
                {
                    client.Disconnect();
                }

            }
        }

        private void btnJian_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.JianCmd);
            }
            btn.Enabled = true;
        }

        private void btnBao_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.BaoCmd);
            }
            btn.Enabled = true;
        }

        private void btnChui_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.ChuiCmd);
            }
            btn.Enabled = true;
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.SignCmd);
            }
            btn.Enabled = true;
        }

        private void btnWatching_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.WatchingCmd);
            }
            btn.Enabled = true;
        }

        private void btnQuitGame_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.QuitGameCmd);
            }
            btn.Enabled = true;
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                if (client.CheckBox.Checked)
                {
                    string cmd = string.Format(Properties.Settings.Default.BetCmd, client.ID.ToString("D4"), client.Name, txtBetPoint.Text);
                    client.Send(cmd);
                }
            }
            btn.Enabled = true;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            int count = 0;
            int.TryParse(txtCount.Text, out count);
            for (int i = 0; i < count; i++)
                AddSocketClient();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < _tcpclients.Count; i++)
            {
                if (_tcpclients[i].CheckBox.Checked)
                {
                    _tcpclients[i].Disconnect();
                    flowPnl.Controls.Remove(_tcpclients[i].CheckBox);
                    _tcpclients[i] = null;
                }
            }

            var list = _tcpclients.Where(x => x == null).ToList();
            foreach (var client in list)
            {
                _tcpclients.Remove(client);
            }
        }

        private void btnClearLog_Click(object sender, EventArgs e)
        {
            txtLog.Clear();
        }

        private void btnCheckAll_Click(object sender, EventArgs e)
        {
            foreach (var client in _tcpclients)
            {
                client.CheckBox.Checked = true;
            }
        }

        private void btnReverseCheck_Click(object sender, EventArgs e)
        {
            foreach (var client in _tcpclients)
            {
                client.CheckBox.Checked = !client.CheckBox.Checked;
            }

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, Properties.Settings.Default.ConnectCmd);
            }
            btn.Enabled = true;
        }

        private void btnGameStart_Click(object sender, EventArgs e)
        {
            if (_tcpclients.Count > 0)
            {
                var client = _tcpclients[0];
                string cmd = string.Format(Properties.Settings.Default.GameStart, client.ID.ToString("D4"), client.Name);
                client.Send(cmd);
            }
        }

        private void btnGameEnd_Click(object sender, EventArgs e)
        {
            if (_tcpclients.Count > 0)
            {
                var client = _tcpclients[0];
                string cmd = string.Format(Properties.Settings.Default.GameStart, client.ID.ToString("D4"), client.Name);
                client.Send(cmd);
            }
        }

        private void btnPlayerInfo_Click(object sender, EventArgs e)
        {
            //_flag = true;
            _playInfoForm.Show();
        }

        #endregion 窗体事件

        private void updatePlayers(object sender, string cmd)
        {
            try
            {
                var client = (SocketClient)sender;
                client.PlayerList.Clear();
                //string fulldata = cmd.Substring(0, cmd.LastIndexOf("\r\n"));
                string[] data = cmd.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string[] playersData = data[data.Length - 1].Split(new string[] { "@P@" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var player in playersData)
                {
                    string[] playerData = player.Split(new string[] { "^" }, StringSplitOptions.None);
                    string id = playerData[0];
                    GamePlayer gamePlayer = client.PlayerList.Where(x => x.ID == id).FirstOrDefault();
                    if (null == gamePlayer)
                    {
                        gamePlayer = new GamePlayer();
                        gamePlayer.ID = id;
                        updatePlayer(gamePlayer, playerData);
                        client.PlayerList.Add(gamePlayer);
                    }
                    else
                    {
                        updatePlayer(gamePlayer, playerData);
                    }

                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private void updatePlayer(GamePlayer gamePlayer, string[] playerData)
        {
            try
            {
                gamePlayer.ID = playerData[0];
                gamePlayer.Name = playerData[1];
                gamePlayer.Gesture = (GuessGesture)Convert.ToInt32(string.IsNullOrEmpty(playerData[2]) ? "0" : playerData[2]);
                gamePlayer.RoundResult = (RoundResult)Convert.ToInt32(string.IsNullOrEmpty(playerData[3]) ? "0" : playerData[3]);
                gamePlayer.PlayerStatus = (PlayerStatus)Convert.ToInt32(string.IsNullOrEmpty(playerData[4]) ? "0" : playerData[4]);
                gamePlayer.GameStatus = (GameStatus)Convert.ToInt32(string.IsNullOrEmpty(playerData[5]) ? "0" : playerData[5]);
                gamePlayer.PlayerState = (PlayerState)Convert.ToInt32(string.IsNullOrEmpty(playerData[6]) ? "0" : playerData[6]);
                gamePlayer.GameState = (GameState)Convert.ToInt32(string.IsNullOrEmpty(playerData[7]) ? "0" : playerData[7]);
                gamePlayer.BetPoint = Convert.ToInt32(string.IsNullOrEmpty(playerData[8]) ? "0" : playerData[8]);
                gamePlayer.RivalID = playerData[9];
                gamePlayer.RivalLocX = Convert.ToInt32(string.IsNullOrEmpty(playerData[10]) ? "0" : playerData[10]);
                gamePlayer.LocY = Convert.ToInt32(string.IsNullOrEmpty(playerData[11]) ? "0" : playerData[11]);
                gamePlayer.LocX = Convert.ToInt32(string.IsNullOrEmpty(playerData[12]) ? "0" : playerData[12]);
                gamePlayer.CurPoint = Convert.ToInt32(string.IsNullOrEmpty(playerData[13]) ? "0" : playerData[13]);

                //if (gamePlayer.BetPoint > 0)
                //{
                //    gamePlayer.PlayerState = PlayerState.Betted;
                //}
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //private void ThreadFunc()
        //{
        //    while (_flag)
        //    {
        //        if (_queue.Count > 0)
        //        {
        //            string cmd = _queue.Dequeue();
        //            this.Invoke(new MethodInvoker(() => { updatePlayers(cmd); }));
        //            //Thread.Sleep(200);
        //        }
        //    }
        //}

        private void btnResetData_Click(object sender, EventArgs e)
        {
            if (_tcpclients.Count > 0)
            {
                var client = _tcpclients[0];
                string cmd = string.Format(Properties.Settings.Default.ResetData);
                client.Send(cmd);
            }
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //_flag = false;
            //_thread.Abort();
        }

        private void btnBetStart_Click(object sender, EventArgs e)
        {
            if (_tcpclients.Count > 0)
            {
                var client = _tcpclients[0];
                string cmd = string.Format(Properties.Settings.Default.BetStart);
                client.Send(cmd);
            }
        }

        private void btnBetEnd_Click(object sender, EventArgs e)
        {
            if (_tcpclients.Count > 0)
            {
                var client = _tcpclients[0];
                string cmd = string.Format(Properties.Settings.Default.BetEnd);
                client.Send(cmd);
            }
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;
            foreach (var client in _tcpclients)
            {
                this.SendCmd(client, txtMsg.Text);
            }
            btn.Enabled = true;
        }

    }
}
