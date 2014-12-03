using System;

namespace FingerGuessingGameAndroid.MainUI
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
            this.ID = "1001";// Properties.Settings.Default.ID;
            this.AuthCode = "yang";// Properties.Settings.Default.AuthCode;
            this.WaitingTime = 10;//  Properties.Settings.Default.WaitingTime;
            this.EOF = "\r\n";// Properties.Settings.Default.EOF == "\\r\\n" ? Environment.NewLine : Properties.Settings.Default.EOF;
            this.ResponseSeparator = "|";// Properties.Settings.Default.ResponseSeparator;
            this.DataSeparator = "^";// Properties.Settings.Default.DataSeparator;
            this.PlayerSeparator = "@P@";// Properties.Settings.Default.PlayerSeparator;
            this.Ip = "192.168.16.11";//  Properties.Settings.Default.Ip;
            this.Port = 8888;//  Properties.Settings.Default.Port;
            this.CmdBao = "{0}^{1}^{2}^0003^2";//  Properties.Settings.Default.BaoCmd;
            this.CmdBet = "{0}^{1}^{2}^0004^{3}";//  Properties.Settings.Default.BetCmd;
            this.CmdChui = "{0}^{1}^{2}^0003^3";//  Properties.Settings.Default.ChuiCmd;
            this.CmdLogin = "{0}^{1}^{2}^0001^0";//  Properties.Settings.Default.LoginCmd;
            this.CmdJian = "{0}^{1}^{2}^0003^1";//  Properties.Settings.Default.JianCmd;
            this.CmdQuitGame = "{0}^{1}^{2}^0000^0";//  Properties.Settings.Default.QuitGameCmd;
            this.CmdSign = "{0}^{1}^{2}^0002^00";//  Properties.Settings.Default.SignCmd;
            this.CmdWatching = "{0}^{1}^{2}^0006^0";//  Properties.Settings.Default.WatchingCmd;
            this.ShowSocketLog = true;//  Properties.Settings.Default.ShowSocketLog;
        }
    }
}
