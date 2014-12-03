using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace FingerGuessingGameAndroid.MainUI
{
    [Serializable]
    public partial class Game
    {
        string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        SocketClient _client = null;
        GameSetting _setting = null;
        GamePlayer _playerInfo = null;
        GamePlayer _lastPlayInfo = null;

        public GamePlayer OldMe
        {
            get { return _lastPlayInfo; }
            set { _lastPlayInfo = value; }
        }


        private bool _isPromoted = false;
        //是否晋级(level改变)
        public bool IsPromoted
        {
            get { return _isPromoted; }
            set { _isPromoted = value; }
        }

        private GamePlayer _champion = null;
        public FingerGuessingGameAndroid.MainUI.GamePlayer Champion
        {
            get { return _champion; }
            set { _champion = value; }
        }

        private bool _inPlayerList = false;
        public FingerGuessingGameAndroid.MainUI.GamePlayer Me
        {
            get { return _playerInfo; }
            set { _playerInfo = value; }
        }

        private List<GamePlayer> _allPlayers = null;
        public List<GamePlayer> AllPlayers
        {
            get { return _allPlayers; }
            set { _allPlayers = value; }
        }

        private List<GamePlayer> _lastPlayerList = null;
        public List<GamePlayer> LastPlayerList
        {
            get { return _lastPlayerList; }
            set { _lastPlayerList = value; }
        }

        bool _isOnline = true;
        static Queue<string> _msgQueue = null;
        Thread _msgThread = null;

        private bool _showSocketLog = false;
        public bool ShowSocketLog
        {
            get { return _showSocketLog; }
            set { _showSocketLog = value; }
        }

        public bool Connected
        {
            get { return _client.Connected; }
        }

        public PlayerState CurrentPlayerState
        {
            get
            {
                return _playerInfo.PlayerState;
            }
            set
            {
                _playerInfo.PlayerState = value;
            }
        }

        public GameState CurrentGameState
        {
            get
            {
                return _playerInfo.GameState;
            }
            set
            {
                _playerInfo.GameState = value;
            }
        }

        public Game(string id)
        {
            _setting = new GameSetting();
            _id = id;
        }

        public Game()
        {
            _setting = new GameSetting();
            _id = _setting.ID;
        }

        private void ShowQueueCount()
        {
            while (true)
            {
                Console.WriteLine(_msgQueue.Count);
                Thread.Sleep(1000);
            }
        }

        private bool PlayerChanged(GamePlayer oldPlayer, GamePlayer newPlayer)
        {
            bool result = false;
            if (oldPlayer == null ||
                    (newPlayer.ID == oldPlayer.ID &&
                        (newPlayer.PlayerState != oldPlayer.PlayerState
                        || newPlayer.BetPoint != oldPlayer.BetPoint
                        || newPlayer.GameStatus != oldPlayer.GameStatus
                        || newPlayer.GameState != oldPlayer.GameState
                        || newPlayer.Gesture != oldPlayer.Gesture
                        || newPlayer.PlayerStatus != oldPlayer.PlayerStatus
                        || newPlayer.Name != oldPlayer.Name
                        || newPlayer.LocX != oldPlayer.LocY
                        || newPlayer.LocY != oldPlayer.LocY
                        || newPlayer.RivalLocX != oldPlayer.RivalLocX
                        || newPlayer.RivalID != oldPlayer.RivalID
                        || newPlayer.RoundResult != oldPlayer.RoundResult)))
            {
                result = true;
            }
            return result;
        }

        #region 公开方法

        //初始化
        public void Init()
        {
            _client = new SocketClient();

            _setting.ID = _id;
            _allPlayers = new List<GamePlayer>();
            _msgThread = new Thread(handleMsg);
            _msgQueue = new Queue<string>();


            _msgThread.Start();

            initClient();
        }

        public void Connect()
        {
            //_isOnline = true;
            _client.Connect();
            if (null == _allPlayers)
            {
                _allPlayers = new List<GamePlayer>();
            }
        }

        public void Disconnect()
        {
            //_isOnline = false;
            _client.Disconnect();
            _allPlayers.Clear();
            _allPlayers = null;
        }

        public void Start()
        {
            _isOnline = true;
        }

        public void Stop()
        {
            _isOnline = false;
            _msgThread.Abort();
            _client.Disconnect();
            //_client.Client.Close();
        }

        //报名
        public void Sign()
        {
            //this.sendCmd(Properties.Settings.Default.SignCmd);
            sendCmd(_setting.CmdSign);
        }

        //出拳(1,剪;2,包;3,锤)
        public void Guess(int gesture)
        {
            switch (gesture)
            {
                case 1:
                    //this.sendCmd(Properties.Settings.Default.JianCmd);
                    sendCmd(_setting.CmdJian);
                    break;
                case 2:
                    //this.sendCmd(Properties.Settings.Default.BaoCmd);
                    sendCmd(_setting.CmdBao);
                    break;
                case 3:
                    //this.sendCmd(Properties.Settings.Default.ChuiCmd);
                    sendCmd(_setting.CmdChui);
                    break;
                default:
                    break;
            }
            //this.GuessEvent(this);
        }

        //观战
        public void Watch()
        {
            //this.sendCmd(Properties.Settings.Default.WatchingCmd);
            sendCmd(_setting.CmdWatching);

        }

        //下注
        public void Bet(int point)
        {
            //string cmd = string.Format(Properties.Settings.Default.BetCmd, _setting.AuthCode, _id, _id, point) + Environment.NewLine;
            string cmd = string.Format(_setting.CmdBet, _setting.AuthCode, _id, _id, point) + Environment.NewLine;
            _client.Send(cmd);
        }

        //退出游戏
        public void QuitGame()
        {
            //this.sendCmd(Properties.Settings.Default.QuitGameCmd);
            sendCmd(_setting.CmdQuitGame);
        }

        public void DoSomethingSpecial()
        {
            //this.sendCmd(Properties.Settings.Default.SpecialCmd);
        }

        /// <summary>
        /// 获得PlayerState发生改变的玩家列表
        /// </summary>
        /// <returns></returns>
        public List<GamePlayer> GetChangedPlayerList()
        {
            List<GamePlayer> list = new List<GamePlayer>();
            //var list = _allPlayers;

            try
            {
                //新增的和改变的
                foreach (var newPlayer in _allPlayers)
                {
                    var oldPlayer = _lastPlayerList.Where(x => x.ID == newPlayer.ID).FirstOrDefault();
                    if (null == oldPlayer)
                    {
                        list.Add((GamePlayer)newPlayer.Clone());
                    }
                    else
                    {
                        if (newPlayer.PlayerState != oldPlayer.PlayerState || newPlayer.BetPoint != oldPlayer.BetPoint
                            || newPlayer.GameStatus != oldPlayer.GameStatus || newPlayer.GameState != oldPlayer.GameState
                            || newPlayer.Gesture != oldPlayer.Gesture || newPlayer.PlayerStatus != oldPlayer.PlayerStatus
                            || newPlayer.LocY != oldPlayer.LocY || newPlayer.RivalID != oldPlayer.RivalID)
                        {
                            list.Add((GamePlayer)newPlayer.Clone());
                        }
                    }
                }

                //减少的(断开的)
                foreach (var player in _lastPlayerList)
                {
                    var connectedPlayer = _allPlayers.Where(x => x.ID == player.ID).FirstOrDefault();
                    if (null == connectedPlayer)
                    {
                        player.PlayerState = PlayerState.Unknown;
                        list.Add((GamePlayer)player.Clone());
                    }
                }
            }
            catch (System.Exception ex)
            {

            }

            return list;
        }

        /// <summary>
        /// 找擂主
        /// </summary>
        /// <returns></returns>
        public void updateChampion()
        {
            try
            {
                _champion = _allPlayers.Where(x => x.PlayerStatus == PlayerStatus.Winner).FirstOrDefault();

                //else
                //{
                //    if (GameState.KOEnd == this.CurrentGameState)
                //    {
                //        _champion = _allPlayers.Where(x => x.PlayerStatus == PlayerStatus.Winner && x.GameState >= GameState.KOEnd).FirstOrDefault();
                //    }
                //    else if (GameState.PK == this.CurrentGameState && PlayerState.RoundEnd != this.CurrentPlayerState)
                //    {
                //        //找出本轮对战的两个人,那么肯定一个是擂主一个是挑战者
                //        var players = _allPlayers.Where(x => !string.IsNullOrEmpty(x.RivalID) && x.GameState > GameState.KOEnd);

                //        //如果其中有一个人的ID和_champion.ID一致,说明擂主连擂
                //        player = players.Where(x => x.ID == _champion.ID).FirstOrDefault();
                //        if (player == null)
                //        {//如果找不到,那么就找ID和_champion.RivalID一致的,说明擂主下擂
                //            player = players.Where(x => x.ID == _champion.RivalID).FirstOrDefault();
                //        }
                //        if (player != null)
                //        {
                //            _champion = player;
                //        }
                //    }
                //}
                //if (_champion != null)
                //{
                //    this.LogEvent(this, "_champion.ID:" + _champion.ID, KnownColor.Red);
                //}
            }
            catch (System.Exception ex)
            {
                throw new Exception("updateChampion:" + ex.Message);
            }
        }

        /// <summary>
        /// 获得下注点数前N名的名字
        /// </summary>
        /// <param name="topCount">N</param>
        /// <returns></returns>
        public List<string> GetTopMostPointPlayer(int topCount = 5)
        {
            List<string> list = new List<string>();
            try
            {
                list = _allPlayers.OrderByDescending(x => x.BetPoint).Take(topCount).Select(x => x.Name).ToList();
            }
            catch (Exception ex)
            {

            }
            return list;
        }

        #endregion 公开方法

        #region  SocketClient事件

        private void _client_LogEvent(object sender, string content)
        {
            if (_showSocketLog)
            {
                //this.LogEvent(sender, "<调试信息> 客户端[" + _client.Name + "]:" + content, KnownColor.Blue);
                if (this.LogEvent != null)
                    this.LogEvent(sender, "<调试信息> 客户端[" + _client.Name + "]:" + content);
            }
        }

        //数据接收
        private void _client_ReceivedDataEvent(object sender, byte[] buffer)
        {
            string fulldata = Encoding.Default.GetString(buffer);
            this._client_LogEvent(sender, "接收信息[" + fulldata.Replace("\r\n", "<br>") + "]");
            //File.AppendAllText("log_" + _id + ".txt", "[" + DateTime.Now.ToLongTimeString() + "]:" + fulldata + Environment.NewLine);

            string[] data;

            fulldata = fulldata.Substring(0, fulldata.LastIndexOf("\r\n"));
            data = fulldata.Split(new string[] { _setting.EOF }, StringSplitOptions.RemoveEmptyEntries);
            if (data.Length <= 0)
            {
                return;
            }

            foreach (var tmp in data)
            {
                _msgQueue.Enqueue(tmp);
            }

        }

        private void _client_DisConnectedEvent(object sender)
        {
            this._client_LogEvent(sender, "Socket连接丢失...");
            //sendCmd(Properties.Settings.Default.QuitGameCmd);
            sendCmd(_setting.CmdQuitGame);
        }

        private void _client_ConnectedEvent(object sender)
        {
            this._client_LogEvent(sender, "Socket连接建立...");
            //sendCmd(Properties.Settings.Default.LoginCmd);
            sendCmd(_setting.CmdLogin);
        }

        #endregion  SocketClient事件

        #region 私有方法
        //初始化socket
        private void initClient()
        {
            //_showSocketLog = _setting.ShowSocketLog;
            _client.Name = _id;
            _client.ConnectedEvent += _client_ConnectedEvent;
            _client.DisConnectedEvent += _client_DisConnectedEvent;

            _client.ReceivedDataEvent += _client_ReceivedDataEvent;
            _client.LogEvent += _client_LogEvent;

            _lastPlayerList = new List<GamePlayer>();
        }

        //发送指令
        private void sendCmd(string cmd)
        {
            cmd = string.Format(cmd, _setting.AuthCode, _id, _id);
            _client.Send(cmd + _setting.EOF);
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
                gamePlayer.RivalLocX = Convert.ToInt32(string.IsNullOrEmpty(playerData[10]) ? "-1" : playerData[10]);
                gamePlayer.LocY = Convert.ToInt32(string.IsNullOrEmpty(playerData[11]) ? "-1" : playerData[11]);
                gamePlayer.LocX = Convert.ToInt32(string.IsNullOrEmpty(playerData[12]) ? "-1" : playerData[12]);

                string strFormat = "[ID]<{0}> [Name]<{1}> [出拳]<{2}> [回合结果]<{3}> [是否擂主]<{4}> "
                    + "[游戏阶段]<{5}> [玩家状态]<{6}> [游戏状态]<{7}> [下注点数]<{8}> [对手ID]<{9}> "
                    + "[对手组号]<{10}> [我的等级]<{11}> [我的组号]<{12}>\r\n";
                string strPlayerInfo = string.Format(strFormat, playerData[0], playerData[1], playerData[2],
                    playerData[3], playerData[4], playerData[5], playerData[6], playerData[7], playerData[8],
                    playerData[9], playerData[10], playerData[11], playerData[12]);

                if (GameReportEvent != null)
                {
                    //this.GameReportEvent(this, "<" + DateTime.Now.ToString("HH:mm:ss.fff") + ">" + strPlayerInfo, KnownColor.SandyBrown);
                    this.GameReportEvent(this, "<" + DateTime.Now.ToString("HH:mm:ss.fff") + ">" + strPlayerInfo);
                }

            }
            catch (System.Exception ex)
            {
                throw new Exception("updatePlayer");
            }
        }

        //更新玩家列表
        private void updatePlayers(string cmd, out bool meModified)
        {
            meModified = false;
            if (string.IsNullOrEmpty(cmd))
            {
                return;
            }
            try
            {
                _lastPlayerList.Clear();
                _allPlayers.ForEach(x => _lastPlayerList.Add((GamePlayer)x.Clone()));

                string[] playersData = cmd.Split(new string[] { _setting.PlayerSeparator }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var player in playersData)
                {
                    string[] playerData = player.Split(new string[] { _setting.DataSeparator }, StringSplitOptions.None);
                    string id = playerData[0];
                    GamePlayer gamePlayer = _allPlayers.Where(x => x.ID == id).FirstOrDefault();
                    if (null == gamePlayer)
                    {
                        gamePlayer = new GamePlayer();
                        gamePlayer.ID = id;
                        updatePlayer(gamePlayer, playerData);
                        _allPlayers.Add(gamePlayer);
                    }
                    else
                    {
                        updatePlayer(gamePlayer, playerData);
                    }

                    if (_id == gamePlayer.ID)
                    {
                        _inPlayerList = true;
                        if (_playerInfo == null)
                        {
                            _lastPlayInfo = (GamePlayer)gamePlayer.Clone();
                            _playerInfo = (GamePlayer)gamePlayer.Clone();
                        }
                        else if (PlayerChanged(_playerInfo, gamePlayer))
                        {
                            int lastLocY = _playerInfo.LocY;
                            _lastPlayInfo = (GamePlayer)_playerInfo.Clone();
                            _playerInfo = (GamePlayer)gamePlayer.Clone();
                            _playerInfo.LastLocY = lastLocY;
                        }
                    }
                }

                //更新对手
                foreach (var player in _allPlayers)
                {
                    GamePlayer rival = null;
                    if (!string.IsNullOrEmpty(player.RivalID))
                    {
                        rival = _allPlayers.Where(x => x.ID == player.RivalID).FirstOrDefault();
                        if (player.ID == _playerInfo.ID)
                        {
                            _lastPlayInfo.Rival = _playerInfo.Rival;
                            _playerInfo.Rival = rival;
                        }

                        player.Rival = rival;

                    }
                    else
                    {
                        player.Rival = null;
                    }
                }

                //this.LogEvent(this, string.Format("对手情况:I[{0}],R[{1}],R.ID[{2}]", _playerInfo.ID.ToString(), _playerInfo.RivalID, _playerInfo.Rival == null ? "null" : _playerInfo.Rival.ID), KnownColor.Orange);


                var playerList = this.GetChangedPlayerList();
                foreach (var player in playerList)
                {
                    if (player.ID == _id)
                    {
                        meModified = true;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new Exception("更新Player信息列表出错:" + ex.Message);
            }
        }

        private bool checkPlayerModified(GamePlayer oldPlayer, GamePlayer newPlayer)
        {
            bool result = false;
            foreach (var pi in typeof(GamePlayer).GetProperties())
            {
                if (pi.GetValue(oldPlayer, null).ToString() != pi.GetValue(newPlayer, null).ToString())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private void handleMsg()
        {
            string lastCmd = string.Empty;
            while (_isOnline)
            {
                try
                {
                    if (_msgQueue.Count == 0)
                    {
                        continue;
                    }

                    string currentCmd = _msgQueue.Dequeue();

                    //相邻的指令如果重复,只处理1次
                    if (lastCmd == currentCmd)
                    {
                        this._client_LogEvent(this, "重复指令,不作处理:[" + currentCmd + "]");
                        continue;
                    }

                    lastCmd = currentCmd;

                    //处理公告
                    if (currentCmd.IndexOf("##", currentCmd.IndexOf("##") + 2) > 0)
                    {
                        if (this.AnnounceEvent != null)
                        {
                            string tmp = currentCmd.Substring(currentCmd.IndexOf("##") + 2, currentCmd.LastIndexOf("##") - 2);
                            string[] data = tmp.Split('^');
                            if (2 == data.Length && "01" == data[0])
                            {
                                this.AnnounceEvent(this, data[1]);
                                continue;
                            }
                        }
                    }

                    //更新本地玩家列表
                    bool meModified = true;
                    updatePlayers(currentCmd, out meModified);

                    ////验证
                    //if (!verify())
                    //{
                    //    this.LogEvent(this, "未被允许进入游戏,准备断开连接...");
                    //    this.Disconnect();
                    //    this.CurrentPlayerState = GameState.DisConnected;
                    //}

                    if (_inPlayerList)
                    {
                        if (GameState.Unknown == this.CurrentGameState && meModified)
                        {
                            if (PlayerState.Connected == this.CurrentPlayerState)
                            {
                                if (this.ConnectedEvent != null)
                                {
                                    this.ConnectedEvent(this);
                                }
                            }
                            else if (PlayerState.Signed == this.CurrentPlayerState)
                            {
                                if (this.SignedEvent != null)
                                {
                                    this.SignedEvent(this);
                                }
                            }
                            else if (PlayerState.Watching == this.CurrentPlayerState)
                            {
                                if (this.WatchingEvent != null)
                                {
                                    this.WatchingEvent(this);
                                }
                            }
                        }
                        else if (GameState.KOStart == this.CurrentGameState && meModified)
                        {
                            if (PlayerState.RoundStart == this.CurrentPlayerState)
                            {
                                if (this.RoundStartEvent != null)
                                {
                                    this.RoundStartEvent(this);
                                }
                            }
                            else if (PlayerState.RoundEnd == this.CurrentPlayerState)
                            {
                                if (this.RoundEndEvent != null)
                                {
                                    this.RoundEndEvent(this);
                                }
                            }
                            else if (PlayerState.Watching == this.CurrentPlayerState)
                            {
                                if (this.WatchingEvent != null)
                                {
                                    this.WatchingEvent(this);
                                }
                            }
                            else
                            {
                                if (this.KOStartEvent != null)
                                {
                                    this.KOStartEvent(this);
                                }
                                if (this.RoundStartEvent != null)
                                {
                                    this.RoundStartEvent(this);
                                }
                            }
                        }
                        else if (GameState.KOEnd == this.CurrentGameState)
                        {
                            if (PlayerState.RoundEnd == this.CurrentPlayerState)
                            {
                                if (this.RoundEndEvent != null)
                                {
                                    this.RoundEndEvent(this);
                                }
                            }


                            if (this.KOEndEvent != null)
                            {
                                this.KOEndEvent(this);
                            }
                        }
                        else if (GameState.PKStart == this.CurrentGameState && meModified)
                        {
                            if (this.BetStartEvent != null)
                            {
                                this.BetStartEvent(this);
                            }
                        }
                        else if (GameState.PK == this.CurrentGameState && meModified)
                        {
                            if (PlayerState.RoundStart == this.CurrentPlayerState)
                            {
                                if (this.RoundStartEvent != null)
                                {
                                    this.RoundStartEvent(this);
                                }
                            }
                            else if (PlayerState.RoundEnd == this.CurrentPlayerState)
                            {
                                if (this.RoundEndEvent != null)
                                {
                                    this.RoundEndEvent(this);
                                }
                            }
                            else
                            {
                                if (this.BetEndEvent != null)
                                {
                                    this.BetEndEvent(this);
                                }
                            }
                        }
                        else if (GameState.PKEnd == this.CurrentGameState)
                        {

                            if (this.PKEndEvent != null)
                            {
                                this.PKEndEvent(this);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    if (_showSocketLog)
                    {
                        //this.LogEvent(this, "<调试信息> 客户端[" + _client.ID + "]:消息处理失败,正文[" + lastCmd + "],原因:" + ex.Message, KnownColor.Blue);
                        if (this.LogEvent != null)
                            this.LogEvent(this, "<调试信息> 客户端[" + _client.ID + "]:消息处理失败,正文[" + lastCmd + "],原因:" + ex.Message);
                    }
                    lastCmd = string.Empty;
                    continue;
                }
                finally
                {
                    //Thread.Sleep(1000);
                }
            }
            lastCmd = string.Empty;
        }

        private string GetPlayerInfo(string id)
        {
            string result = string.Empty;
            var player = _allPlayers.Where(x => x.ID == id).FirstOrDefault();
            if (null == player)
            {
                result = "没找到玩家";
            }
            else
            {
                string strFormat = "玩家ID[{0}] 本人出拳[{1}] 对手ID[{2}] 对手出拳[{3}] 输赢结果[{4}] Level[{5}] LvlNo[{6}] RivalLvlNo[{7}] 游戏阶段[{8}] 玩家状态[{9}] 游戏状态[{10}] 是否擂主[{11}] 下注点数[{12}]";
                result = string.Format(strFormat, player.ID, GameUtil.GetGestureName(player.Gesture)
                    , player.RivalID, player.Rival == null ? "无对手" : GameUtil.GetGestureName(player.Rival.Gesture)
                    , GameUtil.GetRoundResultName(player.RoundResult), player.LocY
                    , player.LocX, player.RivalLocX
                    , GameUtil.GetGameStatusName(player.GameStatus), GameUtil.GetPlayerStateName(player.PlayerState)
                    , GameUtil.GetGameStateName(player.GameState), GameUtil.GetPlayerStatusName(player.PlayerStatus)
                    , player.BetPoint);
            }
            return result;
        }
        #endregion


        //public IntPtr Handle
        //{
        //    get { return IntPtr.Zero; }
        //}

        //public void Dispose()
        //{
        //    _allPlayers = null;
        //    _champion = null;
        //    _client = null;
        //    _lastPlayInfo = null;
        //    _msgQueue = null;
        //    _playerInfo = null;
        //    _setting = null;
        //}
    }
}
