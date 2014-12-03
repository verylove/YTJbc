using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FakedGameClient
{
    public class GameSetting
    {
        //授权码
        public string AuthCode { get; set; }

        //房间号
        public string ID { get; set; }

        //等待时间(秒)
        public int WaitingTime { get; set; }

        //结束符
        public string EOF { get; set; }

        //应答包分隔符
        public string ResponseSeparator { get; set; }

        //应答包数据部分分隔符
        public string DataSeparator { get; set; }

        //数据部分玩家信息分隔符
        public string PlayerSeparator { get; set; }

        public string Ip { get; set; }
        public int Port { get; set; }

        public string CmdLogin { get; set; }
        public string CmdBet { get; set; }
        public string CmdSign { get; set; }
        public string CmdWatching { get; set; }
        public string CmdQuitGame { get; set; }
        public string CmdBao { get; set; }
        public string CmdJian { get; set; }
        public string CmdChui { get; set; }

        //显示Socket日志
        public bool ShowSocketLog { get; set; }

        public GameSetting()
        {
            this.ID = Properties.Settings.Default.ID;
            this.AuthCode = Properties.Settings.Default.AuthCode;
            this.WaitingTime = Properties.Settings.Default.WaitingTime;
            this.EOF = Properties.Settings.Default.EOF == "\\r\\n" ? Environment.NewLine : Properties.Settings.Default.EOF;
            this.ResponseSeparator = Properties.Settings.Default.ResponseSeparator;
            this.DataSeparator = Properties.Settings.Default.DataSeparator;
            this.PlayerSeparator = Properties.Settings.Default.PlayerSeparator;
            this.Ip = Properties.Settings.Default.Ip;
            this.Port = Properties.Settings.Default.Port;
            this.CmdBao = Properties.Settings.Default.BaoCmd;
            this.CmdBet = Properties.Settings.Default.BetCmd;
            this.CmdChui = Properties.Settings.Default.ChuiCmd;
            this.CmdLogin = Properties.Settings.Default.LoginCmd;
            this.CmdJian = Properties.Settings.Default.JianCmd;
            this.CmdQuitGame = Properties.Settings.Default.QuitGameCmd;
            this.CmdSign = Properties.Settings.Default.SignCmd;
            this.CmdWatching = Properties.Settings.Default.WatchingCmd;
            this.ShowSocketLog = Properties.Settings.Default.ShowSocketLog;
        }
    }
}
