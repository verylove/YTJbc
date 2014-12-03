using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FakedGameClient
{
    public partial class FormGame : Form
    {
        public Game _game = null;
        public Thread _updateThread = null;
        System.Timers.Timer _shineTimer = new System.Timers.Timer();
        int _shineTimes;
        const int _TotalShineTimes = 8;

        public FormGame()
        {
            InitializeComponent();
        }

        #region 私有方法

        private void log(object sender, string content, KnownColor color = KnownColor.White)
        {
            string timestamp = DateTime.Now.ToString("<HH:mm:ss.fff> ");
            content = timestamp + content.Replace("\0", "");
            txtLog.AppendText(content + Environment.NewLine);
            txtLog.Select(txtLog.TextLength - content.Length - 1, content.Length);
            txtLog.SelectionColor = Color.FromKnownColor(color);
            txtLog.ScrollToCaret();
        }

        private void report(string content, KnownColor color = KnownColor.DarkGreen)
        {
            txtReport.AppendText(content + Environment.NewLine);
            //txtReport.Select(txtReport.TextLength - content.Length - 1, content.Length);
            //txtReport.SelectionColor = Color.FromKnownColor(color);
            txtReport.ScrollToCaret();
        }

        private void updateUI()
        {
            while (true)
            {
                if (_game != null)
                {
                    this.BeginInvoke(
                        new MethodInvoker(() =>
                        {
                            //update player panel
                            pi.ID = _game.ID;
                            pi.ID = _game.ID;
                            if (_game.Me != null)
                            {
                                pi.BetPoint = _game.Me.BetPoint.ToString();
                                pi.GameState = GameUtil.GetGameStateName(_game.Me.GameState);
                                pi.GameStatus = GameUtil.GetGameStatusName(_game.Me.GameStatus);
                                pi.Gesture = GameUtil.GetGestureName(_game.Me.Gesture);
                                pi.Level = _game.Me.LocY.ToString();
                                pi.LvNo = _game.Me.LocX.ToString();
                                pi.PlayerName = _game.Me.Name;
                                pi.PlayerState = GameUtil.GetPlayerStateName(_game.Me.PlayerState);
                                pi.PlayerStatus = GameUtil.GetPlayerStatusName(_game.Me.PlayerStatus);
                                pi.RivalID = _game.Me.RivalID.ToString();
                                pi.RivalLvNo = _game.Me.RivalLocX.ToString();

                                //piOld.BetPoint = _game.OldMe.BetPoint.ToString();
                                //piOld.GameState = GameUtil.GetGameStateName(_game.OldMe.GameState);
                                //piOld.GameStatus = GameUtil.GetGameStatusName(_game.OldMe.GameStatus);
                                //piOld.Gesture = GameUtil.GetGestureName(_game.OldMe.Gesture);
                                //piOld.Level = _game.OldMe.LocY.ToString();
                                //piOld.LvNo = _game.OldMe.LocX.ToString();
                                //piOld.PlayerName = _game.OldMe.Name;
                                //piOld.PlayerState = GameUtil.GetPlayerStateName(_game.OldMe.PlayerState);
                                //piOld.PlayerStatus = GameUtil.GetPlayerStatusName(_game.OldMe.PlayerStatus);
                                //piOld.RivalID = _game.OldMe.RivalID.ToString();
                                //piOld.RivalLvNo = _game.OldMe.RivalLocX.ToString();
                            }
                        }));
                }
                Thread.Sleep(2000);
            }
        }

        private void setButtonState(int nEvent)
        {
            switch (nEvent)
            {
                case 1:     //报名成功
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;
                    break;
                case 2:     //我出拳
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;
                    break;
                case 3:     //回合开始
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = true;
                    btnJian.Enabled = true;
                    btnChui.Enabled = true;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;
                    break;
                case 4:     //回合结束
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;
                    break;
                case 5:     //比赛结束
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;
                    break;

                case 6:     //下注开始
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = true;
                    cmbPoint.Enabled = true;

                    break;

                case 7:     //下注结束
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;

                    break;
            }
        }

        #endregion

        #region 窗体事件

        private void FormGame_Load(object sender, EventArgs e)
        {
            _game = new Game();
            _game.ConnectedEvent += _game_ConnectedEvent;
            _game.SignedEvent += _game_SignedEvent;
            _game.KOStartEvent += _game_KOStartEvent;
            _game.RoundStartEvent += _game_RoundStartEvent;
            _game.RoundEndEvent += _game_RoundEndEvent;
            _game.KOEndEvent += _game_KOEndEvent;
            _game.PKEndEvent += _game_PKEndEvent;
            _game.LogEvent += _game_LogEvent;
            _game.DisConnectedEvent += _game_DisConnectedEvent;
            _game.WatchingEvent += _game_WatchingEvent;
            _game.BetStartEvent += _game_BetStartEvent;
            _game.BetEndEvent += _game_BetEndEvent;
            _game.GameReportEvent += _game_GameReportEvent;
            _game.AnnounceEvent += _game_AnnounceEvent;
            _game.Init();

            this.Text = "[" + _game.ID + "]...游戏主窗口";

            _shineTimer.Interval = 500;
            _shineTimes = 0;
            _shineTimer.Elapsed += _timer_Elapsed;

            _updateThread = new Thread(new ThreadStart(updateUI));
            _updateThread.Start();
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            log(sender, "已发送[报名]请求...", KnownColor.Yellow);
            _game.Sign();
        }

        private void btnJian_Click(object sender, EventArgs e)
        {
            log(sender, "我 一![剪]!没!", KnownColor.Yellow);
            _game.Guess(1);
            setButtonState(2);
        }

        private void btnBao_Click(object sender, EventArgs e)
        {
            log(sender, "我 求![包]!养!", KnownColor.Yellow);
            _game.Guess(2);
            setButtonState(2);
        }

        private void btnChui_Click(object sender, EventArgs e)
        {
            log(sender, "我 锤!你!妹!", KnownColor.Yellow);
            _game.Guess(3);
            setButtonState(2);
        }

        private void btnWatching_Click(object sender, EventArgs e)
        {
            log(sender, "登出中...", KnownColor.Yellow);
            _game.Watch();
        }

        private void btnBet_Click(object sender, EventArgs e)
        {
            int point = 0;
            try
            {
                point = Convert.ToInt32(cmbPoint.Text);
                log(sender, "下注[" + cmbPoint.Text + "]点", KnownColor.Yellow);
                _game.Bet(point);
                setButtonState(7);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("点数输入错误!");
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (!_game.Connected)
            {
                log(sender, "登录中...", KnownColor.Yellow);
                btnConnect.Text = "断开";
                _game.Connect();
            }
            else
            {
                _game.Disconnect();
                btnConnect.Text = "连接";
            }
        }

        private void btnQuitGame_Click(object sender, EventArgs e)
        {
            _game.QuitGame();
        }

        private void FormGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            _updateThread.Abort();
            _game.Stop();
        }

        private void btnStartRace_Click(object sender, EventArgs e)
        {
            _game.DoSomethingSpecial();
        }

        #endregion

        #region 游戏事件
        private void _game_LogEvent(object sender, string content, KnownColor color = KnownColor.White)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Game.LogEventHandler(log), new object[] { sender, content, color });
            }
            else
            {
                log(sender, content, color);
            }
        }

        private void _game_BetStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    try
                    {
                        //btnConnect.Enabled = false;
                        btnSign.Enabled = false;
                        btnWatching.Enabled = false;

                        btnBao.Enabled = false;
                        btnJian.Enabled = false;
                        btnChui.Enabled = false;

                        _game.updateChampion();

                        if (PlayerStatus.Defier == _game.Me.PlayerStatus)
                        {
                            cmbPoint.Enabled = true;
                            btnBet.Enabled = true;

                            log(sender, "请下注决定挑战者...", KnownColor.SkyBlue);
                            if (_game.Me.BetPoint > 0)
                            {
                                if (PlayerStatus.Winner != _game.Me.PlayerStatus)
                                {
                                    log(sender, "您已下注[" + _game.Me.BetPoint + "]点...", KnownColor.SkyBlue);
                                    string names = "";
                                    _game.GetTopMostPointPlayer().ForEach(delegate(string name) { names += name + ","; });
                                    log(sender, "当前下注最高Top 5:[" + names.TrimEnd(',') + "]", KnownColor.SkyBlue);
                                }
                            }
                        }
                        //}
                    }
                    catch (System.Exception ex)
                    {
                        log(sender, "_game_BetStartEvent:" + ex.Message, KnownColor.SkyBlue);
                    }
                }
            ));
        }

        private void _game_PKEndEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    try
                    {
                        log(sender, "今天的游戏已经圆满结束,恭喜[" + _game.Champion.Name + "]笑到了最后...", KnownColor.LimeGreen);

                        btnSign.Enabled = false;
                        btnWatching.Enabled = false;

                        btnBao.Enabled = false;
                        btnJian.Enabled = false;
                        btnChui.Enabled = false;

                        cmbPoint.Enabled = false;
                        btnBet.Enabled = false;
                    }
                    catch (System.Exception ex)
                    {
                        log(sender, "_game_PKEndEvent:" + ex.Message, KnownColor.SkyBlue);
                    }

                }
            ));
        }

        private void _game_BetEndEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    try
                    {
                        string names = "";
                        if (PlayerStatus.Winner != _game.Me.PlayerStatus && _game.Me.RoundResult > RoundResult.Unknown)
                        {
                            //log(sender, "_game.Me.PlayerStatus:" + _game.Me.PlayerStatus.ToString() + "| _game.Me.RoundResult:" + _game.Me.RoundResult.ToString());
                        }
                        else
                        {
                            setButtonState(7);
                            log(sender, "接下来将是擂主[" + _game.Champion.ID + "]和挑战者[" + _game.Champion.RivalID + "]的激烈对决!!!");

                            if (!string.IsNullOrEmpty(_game.Me.RivalID))
                            {
                                log(sender, "你的对手是[" + _game.Me.RivalID + "],请出拳...", KnownColor.SkyBlue);
                                setButtonState(3);
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log(sender, "_game_BetEndEvent:" + ex.Message, KnownColor.SkyBlue);
                    }
                }
            ));
        }

        private void _game_GameReportEvent(object sender, string content, KnownColor color = KnownColor.DarkGreen)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    report(content, color);
                }));
        }

        private void _game_RoundEndEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                    {
                        try
                        {
                            if (PlayerState.Watching == _game.CurrentPlayerState)
                            {
                                log(sender, "[占位符:观战显示内容]...", KnownColor.DarkGreen);
                            }
                            else
                            {
                                string rivalGesture = string.Empty;
                                string rivalId = string.Empty;
                                string gesture = string.Empty;

                                if (!string.IsNullOrEmpty(_game.Me.RivalID) && _game.Me.Rival != null)
                                {
                                    rivalGesture = GameUtil.GetGestureName(_game.Me.Rival.Gesture);
                                    rivalId = _game.Me.RivalID;
                                    gesture = GameUtil.GetGestureName(_game.Me.Gesture);
                                    if (_game.CurrentGameState >= GameState.PKStart)
                                    {
                                        if (!string.IsNullOrEmpty(rivalId))
                                        {
                                            if (RoundResult.Win == _game.Me.RoundResult && gesture != "未知")
                                            {
                                                log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                                log(sender, "小样儿吧,你个[" + rivalId + "],见识到大爷的厉害了吧!!!哈哈哈!!", KnownColor.Orange);
                                                log(sender, "等待新的挑战者...", KnownColor.SkyBlue);
                                                setButtonState(4);
                                            }
                                            else if (RoundResult.Lose == _game.Me.RoundResult && gesture != "未知")
                                            {
                                                log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                                log(sender, "老子[" + _game.Me.Name + "]十八年后还是一条好汉,你[" + rivalId + "]等着瞧!!!", KnownColor.Orange);
                                                log(sender, "稍后开始新一轮下注...", KnownColor.SkyBlue);
                                                setButtonState(4);
                                            }
                                            else if (RoundResult.Draw == _game.Me.RoundResult && gesture != "未知")
                                            {
                                                log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                                log(sender, "哟呵!屎壳郎拦吉普你[" + rivalId + "]还和我[" + _game.Me.Name + "]顶上了,再来,看我不灭了你!!!", KnownColor.Orange);
                                                log(sender, "请重新出拳...", KnownColor.SkyBlue);
                                                setButtonState(3);
                                            }
                                            else if (RoundResult.Unknown == _game.Me.RoundResult)
                                            {
                                                log(sender, "新的挑战者出现了!!![" + rivalId + "]竖起了手指在向你挑衅了,干掉他!!!", KnownColor.Green);
                                                setButtonState(4);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //if (_game.Me.LocY != _game.OldMe.LocY)
                                        //{
                                        if (RoundResult.Win == _game.Me.RoundResult)
                                        {
                                            if (!string.IsNullOrEmpty(rivalId))
                                            {
                                                if (_game.Me.LocY == _game.Me.LastLocY)
                                                {
                                                    log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                                    log(sender, "看来对手太菜了,恭喜你轻松获胜!!!", KnownColor.Lime);
                                                    log(sender, "等待新的对手...", KnownColor.SkyBlue);
                                                    setButtonState(4);
                                                }
                                                else
                                                {
                                                    log(sender, "你的对手是[" + rivalId + "],请出拳...", KnownColor.SkyBlue);
                                                    setButtonState(3);
                                                }
                                            }
                                        }
                                        else if (RoundResult.Lose == _game.Me.RoundResult)
                                        {
                                            if (PlayerState.Watching != _game.Me.PlayerState)
                                            {
                                                log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                                log(sender, "居然..输了..唉!既生瑜何生亮啊!!!", KnownColor.Lime);
                                                log(sender, "稍后转入观战...", KnownColor.SkyBlue);
                                                setButtonState(4);
                                            }
                                        }
                                        else if (RoundResult.Unknown == _game.Me.RoundResult && !string.IsNullOrEmpty(rivalId))
                                        {
                                            log(sender, "你的对手是[" + rivalId + "],请出拳...", KnownColor.SkyBlue);
                                            setButtonState(3);
                                        }
                                        else if (RoundResult.Draw == _game.Me.RoundResult)
                                        {
                                            log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...", KnownColor.SkyBlue);
                                            log(sender, "居然是平局!得好好想想接下来出什么了呀!!!", KnownColor.Lime);
                                            log(sender, "请重新出拳...", KnownColor.SkyBlue);
                                            setButtonState(3);
                                        }
                                        //}
                                        //else if (RoundResult.Draw == _game.Me.RoundResult)
                                        //{
                                        //    log(sender, "居然是平局!得好好想想接下来出什么了呀!!!", KnownColor.SkyBlue);
                                        //    log(sender, "请重新出拳...", KnownColor.SkyBlue);
                                        //}
                                    }
                                }
                                else
                                {
                                    //自动晋级
                                    if (GuessGesture.Unknown == _game.Me.Gesture)
                                    {
                                        log(sender, "运气真好!恭喜你本轮获得自动晋级的机会!!", KnownColor.Orange);
                                        log(sender, "请稍后进入下一轮!", KnownColor.SkyBlue);
                                        setButtonState(4);
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            log(sender, "_game_RoundEndEvent:" + ex.Message, KnownColor.SkyBlue);
                        }
                    }
            ));
        }

        private void _game_RoundStartEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(_game.Me.RivalID))
                        {
                            if (_game.Me.GameState < GameState.PKStart)
                            {
                                if (GuessGesture.Unknown != _game.Me.Rival.Gesture)
                                {
                                    log(sender, "你的对手已经出拳!", KnownColor.SkyBlue);
                                }
                                else if (GuessGesture.Unknown == _game.Me.Gesture)
                                {
                                    log(sender, "你的对手是[" + _game.Me.RivalID + "],请出拳...", KnownColor.SkyBlue);
                                    //log(sender, "请出拳...", KnownColor.SkyBlue);
                                    //btnConnect.Enabled = false;
                                    setButtonState(3);
                                }
                            }
                            else
                            {
                                if (GuessGesture.Unknown != _game.Me.Rival.Gesture)
                                {
                                    log(sender, "你的对手已经出拳!", KnownColor.SkyBlue);
                                }
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log(sender, "_game_RoundStartEvent:" + ex.Message, KnownColor.SkyBlue);
                    }
                }

                ));
        }

        private void _game_KOEndEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    try
                    {
                        setButtonState(4);

                        log(sender, "比赛结束!!!接下来将是紧张刺激的擂台大战...");

                        _game.updateChampion();
                        if (_game.Champion != null)
                        {
                            log(sender, "冠军是[" + _game.Champion.ID + "]", KnownColor.SkyBlue);

                            if (_game.Champion.ID == _game.Me.ID)
                            {
                                //this.WindowState = FormWindowState.Maximized;
                                log(sender, "擂主大人,先喝个小酒,等等挑战者吧...", KnownColor.Green);
                                _shineTimer.Start();
                            }
                        }
                    }
                    catch (System.Exception ex)
                    {
                        log(sender, "_game_KOEndEvent:" + ex.Message, KnownColor.SkyBlue);
                    }
                })
                );
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _shineTimes++;
            this.BeginInvoke(new MethodInvoker(() =>
            {
                if (txtLog.BackColor == Color.Black)
                {
                    txtLog.BackColor = Color.SlateBlue;
                }
                else
                {
                    txtLog.BackColor = Color.Black;
                }
            }));
            if (_shineTimes == _TotalShineTimes)
            {
                _shineTimes = 0;
                _shineTimer.Stop();
            }
        }

        private void _game_KOStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "比赛正式开始...", KnownColor.SkyBlue);
                    setButtonState(4);
                }
                ));
        }

        private void _game_WatchingEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    log(sender, "围观中...", KnownColor.SkyBlue);
                    setButtonState(4);
                }
           ));
        }

        private void _game_SignedEvent(object sender)
        {
            this.BeginInvoke(
                new MethodInvoker(() =>
                {
                    if (PlayerState.Signed == _game.CurrentPlayerState)
                    {
                        log(sender, "报名成功...", KnownColor.SkyBlue);
                        setButtonState(1);
                    }
                    else
                    {
                        log(sender, "报名不成功...", KnownColor.Red);
                    }
                }
            ));
        }

        private void _game_QuitGameEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "退出游戏?你会后悔的...", KnownColor.SkyBlue);
                }
            ));
        }

        private void _game_DisConnectedEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    btnConnect.Text = "连接";
                    btnSign.Enabled = false;
                    btnWatching.Enabled = false;

                    btnBao.Enabled = false;
                    btnJian.Enabled = false;
                    btnChui.Enabled = false;

                    btnBet.Enabled = false;
                    cmbPoint.Enabled = false;

                    log(sender, "断开连接...", KnownColor.SkyBlue);
                }
            ));
        }

        private void _game_ConnectedEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    if (_game.Connected)
                    {
                        btnConnect.Text = "断开";
                        log(sender, "登录到服务器", KnownColor.SkyBlue);
                        btnSign.Enabled = true;
                        btnWatching.Enabled = true;

                    }
                    else
                    {
                        btnConnect.Text = "连接";
                        log(sender, "登录失败", KnownColor.Red);
                        btnSign.Enabled = false;
                        btnWatching.Enabled = false;
                    }
                }
            ));
        }

        private void _game_AnnounceEvent(object sender, string content, KnownColor color = KnownColor.Orange)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    //log(sender,"[系统公告]:"+ content, color);
                    MessageBox.Show(content, "系统公告");
                }));
        }


        #endregion  游戏事件
    }
}
