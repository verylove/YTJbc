using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using FakedGameClient.ServerController.Database;

namespace FakedGameClient.ServerController
{
    public partial class FormMain : Form
    {
        private bool _iconed = false;
        private FormWaiting _frmWaiting;
        private FormPlayers _frmPlayers;
        private List<GamePlayer> _playerList = new List<GamePlayer>();

        private const string _CfgFileName = "config.ini";
        private const string _AwardFileName = "award.png";
        private const int _PicFileSize = 1;    //单位:MB

        private GamePlayer _champion = null;
        DbEntities _db = new DbEntities();

        #region Socket配置
        private SocketClient _socket = null;
        public SocketClient Socket
        {
            get { return _socket; }
            set { _socket = value; }
        }

        private void InitSocket()
        {
            _socket = new SocketClient();
            _socket.ConnectingEvent += _socket_ConnectingEvent;
            _socket.ConnectedEvent += _socket_ConnectedEvent;
            _socket.ConnectFailedEvent += _socket_ConnectFailedEvent;
            _socket.ConnectionLostEvent += _socket_ConnectionLostEvent;
            _socket.LogEvent += _socket_LogEvent;
            _socket.ReceivedDataEvent += _socket_ReceivedDataEvent;
        }

        private void _socket_ConnectionLostEvent(object sender)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                MessageBox.Show(this, "与服务器的连接已断开...", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnConnect.Visible = true;
                btnConnect.Enabled = true;
            }));
        }

        private void _socket_ReceivedDataEvent(object sender, byte[] buffer)
        {
            SocketClient client = (SocketClient)sender;
            string tmp = Encoding.Default.GetString(buffer);

            tmp = tmp.Substring(0, tmp.IndexOf('\0') - 2);

            if (!string.IsNullOrEmpty(tmp))
            {
                updatePlayers(sender, tmp);
                //更新玩家窗口
                if (_frmPlayers != null)
                {
                    _frmPlayers.PlayerList = _playerList;
                }

                //保存擂主
                var champion = _playerList.Where(x => x.PlayerStatus == PlayerStatus.Winner).FirstOrDefault();
                if (champion != null)
                {
                    _champion = champion;
                }

                this.BeginInvoke(new MethodInvoker(() =>
                {
                    log(sender, "接收到:[" + tmp.Replace("\r\n", "<br>") + "]");
                }));
            }

        }

        private void _socket_ConnectFailedEvent(object sender, string msg)
        {
            this.Invoke(new MethodInvoker(() =>
            {
                MessageBox.Show("连接服务器失败,原因:" + msg, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                _frmWaiting.Hide();
                this.Enabled = true;
                btnConnect.Enabled = true;
                btnConnect.Visible = true;
                this.Focus();
            }));
        }

        private void _socket_LogEvent(object sender, string content)
        {
            //记录日志或者弹出提示
            this.Invoke(new MethodInvoker(() =>
            {
                log(sender, content);
            }));
        }

        private void _socket_ConnectingEvent(object sender)
        {
            //显示正在连接中
            this.Invoke(new MethodInvoker(() =>
            {
                _frmWaiting.Title = "剪包锤游戏管理程序";
                _frmWaiting.Label = "正在连接服务器,请稍后...";
                _frmWaiting.Show();
            }));
            this.Enabled = false;
        }

        private void _socket_ConnectedEvent(object sender)
        {
            //关闭正在连接中
            this.Invoke(new MethodInvoker(() =>
            {
                _frmWaiting.Hide();
                btnConnect.Enabled = false;
                btnConnect.Visible = false;
                this.Enabled = true;
            }));

            if (!_socket.Connected)
            {
                MessageBox.Show(this, "连接服务器失败", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Focus();
        }

        #endregion

        #region 调试用
        private void log(object sender, string content)
        {
            content = content.Replace("\r\n", "**");
            txtLog.AppendText(content + Environment.NewLine);
            txtLog.Select(txtLog.Text.Length - content.Length - 1, content.Length);
            txtLog.SelectionColor = Color.SlateBlue;
            txtLog.ScrollToCaret();
        }

        #endregion

        private void InitFrmWaiting()
        {
            _frmWaiting = new FormWaiting();
            _frmWaiting.ShowInTaskbar = false;
            _frmWaiting.StartPosition = FormStartPosition.CenterScreen;
        }

        private void updatePlayers(object sender, string cmd)
        {
            try
            {
                var client = (SocketClient)sender;
                _playerList.Clear();
                //string fulldata = cmd.Substring(0, cmd.LastIndexOf("\r\n"));
                string[] data = cmd.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                string[] playersData = data[data.Length - 1].Split(new string[] { "@P@" }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var player in playersData)
                {
                    string[] playerData = player.Split(new string[] { "^" }, StringSplitOptions.None);
                    string id = playerData[0];
                    GamePlayer gamePlayer = _playerList.Where(x => x.ID == id).FirstOrDefault();
                    if (null == gamePlayer)
                    {
                        gamePlayer = new GamePlayer();
                        gamePlayer.ID = id;
                        updatePlayer(gamePlayer, playerData);
                        _playerList.Add(gamePlayer);
                    }
                    else
                    {
                        updatePlayer(gamePlayer, playerData);
                    }
                }
            }
            catch (System.Exception ex)
            {
                _socket_LogEvent(sender, "updatePlayers()异常:" + ex.Message);
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
            }
            catch (System.Exception ex)
            {
                _socket_LogEvent(this, "updatePlayer()异常:" + ex.Message);
            }
        }

        private void updatePoint()
        {
            try
            {
                //更新积分
                foreach (var player in _playerList)
                {
                    var room = _db.Rooms.Where(x => x.Code == player.ID).FirstOrDefault();

                    if (room == null)
                    {
                        continue;
                    }

                    room.Point = player.CurPoint;

                    _db.Entry(room).State = EntityState.Modified;
                }
                _db.SaveChanges();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("更新积分失败,原因:" + ex.Message, "剪包锤游戏管理程序");
            }
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            InitFrmWaiting();
            InitSocket();
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            _socket.Connect();
        }

        private void btnSelectPic_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "奖品图片文件(*.PNG)|*.PNG";
            dlg.Multiselect = false;
            DialogResult dr = dlg.ShowDialog();
            if (!string.IsNullOrEmpty(dlg.FileName))
            {
                txtPicPath.Text = dlg.FileName;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //if (_socket.Connected)
            //{
            if (_iconed)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                _iconed = false;
            }
            else
            {
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                _iconed = true;
            }
            //}
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StartGame);
        }

        private void btnStopGame_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StopGame);
        }

        private void btnResetGame_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.ResetGame);
        }

        private void btnStartKO_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StartKO);
        }

        private void btnStopKO_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StopKO);
        }

        private void btnStartPk_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StartPK);
        }

        private void btnStopPK_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StopPK);
        }

        private void btnStartBet_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StartBet);
        }

        private void btnStopBet_Click(object sender, EventArgs e)
        {
            _socket.Send(GameCommand.StopBet);
        }

        private void btnNotice_Click(object sender, EventArgs e)
        {
            string content = txtNotice.Text;
            _socket.Send(string.Format(GameCommand.SystemNotice, content));
        }

        private void btnSendMsg_Click(object sender, EventArgs e)
        {
            string content = txtMsg.Text;
            _socket.Send(string.Format(GameCommand.GameMessage, content));
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            InitFrmWaiting();
            _socket.Connect();
        }

        private void btnShowPlayer_Click(object sender, EventArgs e)
        {
            if (_frmPlayers == null)
            {
                _frmPlayers = new FormPlayers();
                _frmPlayers.ShowInTaskbar = false;
            }

            _frmPlayers.Show();
            _frmPlayers.Focus();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string cfgFilePath = Application.StartupPath + "\\" + _CfgFileName;

            if (File.Exists(_CfgFileName))
            {
                try
                {
                    File.Delete(Application.StartupPath + "\\" + _CfgFileName);
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(this, "配置文件无法更新,请确认本程序目录下的Config.dat文件没有被其它程序打开!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                IniFile ini = new IniFile(cfgFilePath);
                ini.WriteValue("baseconf", "maxconnect", numMaxRoom.Value.ToString());
                ini.WriteValue("baseconf", "isauto", chkAuto.Checked ? "1" : "0");
                ini.WriteValue("baseconf", "award", txtAward.Text);
                ini.WriteValue("gameconf", "gamestart", tpGameBeginTime.Value.ToString("HH:mm:ss"));
                ini.WriteValue("gameconf", "gameend", tpGameEndTime.Value.ToString("HH:mm:ss"));
                ini.WriteValue("gameconf", "intervaltime", numIntervaltime.Value.ToString());
                ini.WriteValue("gameconf", "bittime", numBetTime.Value.ToString());
                ini.WriteValue("gameconf", "takttime", numTaktTime.Value.ToString());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(this, "游戏配置文件生成失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Properties.Settings.Default.UploadUrl))
            {
                MessageBox.Show(this, "保存游戏配置失败,未设置服务器上传地址!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                progressBar1.Visible = true;
                //lblProcess.Visible = true;
                GameUtil.UploadRequest(Properties.Settings.Default.UploadUrl, cfgFilePath, _CfgFileName, 1, progressBar1);
                MessageBox.Show(this, "保存游戏配置成功!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //发送服务器更新配置指令
                _socket.Send(GameCommand.UpdateSetting);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "保存游戏配置失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                progressBar1.Visible = false;
                //lblProcess.Visible = false;
            }
        }

        private void btnUploadPic_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtPicPath.Text) && File.Exists(txtPicPath.Text))
            {
                FileInfo fi = new FileInfo(txtPicPath.Text);

                if (fi.Length >= _PicFileSize * 1024 * 1024)
                {
                    MessageBox.Show(this, "图片文件大小超过" + _PicFileSize + "MB,请重新选择!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    progressBar1.Visible = true;
                    //lblProcess.Visible = true;

                    int result = GameUtil.UploadRequest(Properties.Settings.Default.UploadUrl, txtPicPath.Text, _AwardFileName, 2, this.progressBar1);
                    if (result == 1)
                    {
                        MessageBox.Show(this, "保存大奖图片成功!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //发送服务器更新配置指令
                        _socket.Send(GameCommand.UpdateSetting);
                    }
                    else
                    {
                        MessageBox.Show(this, "保存大奖图片失败!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "保存大奖图片失败!原因:" + ex.Message, "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    progressBar1.Visible = false;
                    //lblProcess.Visible = false;
                }
            }
        }

        private void btnPoint_Click(object sender, EventArgs e)
        {
            FormRooms frm = new FormRooms(_socket);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }

        private void btnShowChampion_Click(object sender, EventArgs e)
        {
            if (_champion != null)
            {
                MessageBox.Show("当前擂主是:  房间[" + _champion.ID + "]", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("目前还没有擂主,或者擂台挑战还没有结束!", "剪包锤游戏管理程序", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnShowPoint_Click(object sender, EventArgs e)
        {
            FormRoomsPoint frm = new FormRoomsPoint();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog();
        }
    }
}
