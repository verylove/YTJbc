using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using FingerGuessingGameAndroid.MainUI;
using System.Threading;
using Java.Lang;

namespace FingerGuessingGameAndroid.MainUI
{
    [Activity(Label = "Game Activity")]
    public class ActivityGame : Activity
    {
        Game _game = null;
        TextView _txtLog = null;
        Handler _uiHandler = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.Game);
            InitActivity();
            InitControl();
            InitGame();
        }

        #region 私有方法
        private void InitActivity()
        {
            var gameApp = (GameApplication)Application;
            _game = gameApp.GameController;
            _uiHandler = new Handler();
        }

        private void InitControl()
        {
            _txtLog = FindViewById<TextView>(Resource.Id.txtLog);
            _txtLog.Text = string.Empty;

            Button btnBack = FindViewById<Button>(Resource.Id.btnBack);
            btnBack.Click += btnBack_Click;

            Button btnBao = FindViewById<Button>(Resource.Id.btnBao);
            btnBao.Click += btnBao_Click;

            Button btnBet = FindViewById<Button>(Resource.Id.btnBet);
            btnBet.Click += btnBet_Click;

            Button btnChui = FindViewById<Button>(Resource.Id.btnChui);
            btnChui.Click += btnChui_Click;
            Button btnJian = FindViewById<Button>(Resource.Id.btnJian);
            btnJian.Click += btnJian_Click;

            Button btnSign = FindViewById<Button>(Resource.Id.btnSign);
            btnSign.Click += btnSign_Click;
        }

        private void InitGame()
        {
            try
            {
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
                //_game.Init();
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this.ApplicationContext, ex.Message, ToastLength.Short);
            }
        }

        private void ShowMessage(string content)
        {
            Runnable runable = new Runnable(() =>
             {
                 //Looper.Prepare();
                 try
                 {
                     TextView txt = this.FindViewById<TextView>(Resource.Id.txtLog);
                     txt.Append(content + "\r\n");
                 }
                 catch (System.Exception ex)
                 {
                     Toast.MakeText(this.ApplicationContext, ex.Message, ToastLength.Long);
                 }
                 //Looper.Loop();
             });
            _uiHandler.Post(runable);
            //_ShowMessageHandler.BeginInvoke(activity, content, null, null);
        }

        private void ShowMessage(Activity activity, string content)
        {
            ShowMessage(content);
        }

        private void log(object sender, string content)
        {
            string timestamp = DateTime.Now.ToString("<HH:mm:ss.fff> ");
            content = timestamp + content.Replace("\0", "");
            ShowMessage(content);

            //txtLog.AppendText(content + Environment.NewLine);
            //txtLog.Select(txtLog.TextLength - content.Length - 1, content.Length);
            //txtLog.SelectionColor = Color.FromKnownColor(color);
            //txtLog.ScrollToCaret();
        }

        private void report(string content)
        {
            //txtReport.AppendText(content + Environment.NewLine);
            ////txtReport.Select(txtReport.TextLength - content.Length - 1, content.Length);
            ////txtReport.SelectionColor = Color.FromKnownColor(color);
            //txtReport.ScrollToCaret();
        }

        private void updateUI()
        {
            while (true)
            {
                //if (_game != null)
                //{
                //    this.BeginInvoke(
                //        new MethodInvoker(() =>
                //        {
                //            //update player panel
                //            pi.ID = _game.ID;
                //            pi.ID = _game.ID;
                //            if (_game.Me != null)
                //            {
                //                pi.BetPoint = _game.Me.BetPoint.ToString();
                //                pi.GameState = GameUtil.GetGameStateName(_game.Me.GameState);
                //                pi.GameStatus = GameUtil.GetGameStatusName(_game.Me.GameStatus);
                //                pi.Gesture = GameUtil.GetGestureName(_game.Me.Gesture);
                //                pi.Level = _game.Me.LocY.ToString();
                //                pi.LvNo = _game.Me.LocX.ToString();
                //                pi.PlayerName = _game.Me.Name;
                //                pi.PlayerState = GameUtil.GetPlayerStateName(_game.Me.PlayerState);
                //                pi.PlayerStatus = GameUtil.GetPlayerStatusName(_game.Me.PlayerStatus);
                //                pi.RivalID = _game.Me.RivalID.ToString();
                //                pi.RivalLvNo = _game.Me.RivalLocX.ToString();

                //                //piOld.BetPoint = _game.OldMe.BetPoint.ToString();
                //                //piOld.GameState = GameUtil.GetGameStateName(_game.OldMe.GameState);
                //                //piOld.GameStatus = GameUtil.GetGameStatusName(_game.OldMe.GameStatus);
                //                //piOld.Gesture = GameUtil.GetGestureName(_game.OldMe.Gesture);
                //                //piOld.Level = _game.OldMe.LocY.ToString();
                //                //piOld.LvNo = _game.OldMe.LocX.ToString();
                //                //piOld.PlayerName = _game.OldMe.Name;
                //                //piOld.PlayerState = GameUtil.GetPlayerStateName(_game.OldMe.PlayerState);
                //                //piOld.PlayerStatus = GameUtil.GetPlayerStatusName(_game.OldMe.PlayerStatus);
                //                //piOld.RivalID = _game.OldMe.RivalID.ToString();
                //                //piOld.RivalLvNo = _game.OldMe.RivalLocX.ToString();
                //            }
                //        }));
                //}
                System.Threading.Thread.Sleep(2000);
            }
        }

        private void setButtonState(int nEvent)
        {
            //switch (nEvent)
            //{
            //    case 1:     //报名成功
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 2:     //我出拳
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 3:     //回合开始
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = true;
            //        btnJian.Enabled = true;
            //        btnChui.Enabled = true;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 4:     //回合结束
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 5:     //比赛结束
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;

            //    case 6:     //下注开始
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = true;
            //        cmbPoint.Enabled = true;

            //        break;

            //    case 7:     //下注结束
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;

            //        break;
            //}
        }

        #endregion 私有方法

        #region 游戏事件

        private void _game_LogEvent(object sender, string content)
        {
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new Game.LogEventHandler(log), new object[] { sender, content, color });
            //}
            //else
            //{
            //    log(sender, content);
            //}
        }

        private void _game_BetStartEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            try
            {
                //            //btnConnect.Enabled = false;
                //            btnSign.Enabled = false;
                //            btnWatching.Enabled = false;

                //            btnBao.Enabled = false;
                //            btnJian.Enabled = false;
                //            btnChui.Enabled = false;

                _game.updateChampion();

                if (PlayerStatus.Defier == _game.Me.PlayerStatus)
                {
                    //cmbPoint.Enabled = true;
                    //btnBet.Enabled = true;

                    log(sender, "请下注决定挑战者...");
                    if (_game.Me.BetPoint > 0)
                    {
                        if (PlayerStatus.Winner != _game.Me.PlayerStatus)
                        {
                            log(sender, "您已下注[" + _game.Me.BetPoint + "]点...");
                            string names = "";
                            _game.GetTopMostPointPlayer().ForEach(delegate(string name) { names += name + ","; });
                            log(sender, "当前下注最高Top 5:[" + names.TrimEnd(',') + "]");
                        }
                    }
                }
                //            //}
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_BetStartEvent:" + ex.Message);
            }
            //}
            //));
        }

        private void _game_PKEndEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            try
            {
                log(sender, "今天的游戏已经圆满结束,恭喜[" + _game.Champion.Name + "]笑到了最后...");

                //            btnSign.Enabled = false;
                //            btnWatching.Enabled = false;

                //            btnBao.Enabled = false;
                //            btnJian.Enabled = false;
                //            btnChui.Enabled = false;

                //            cmbPoint.Enabled = false;
                //            btnBet.Enabled = false;
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_PKEndEvent:" + ex.Message);
            }

            //    }
            //));
        }

        private void _game_BetEndEvent(object sender)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
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
                        log(sender, "你的对手是[" + _game.Me.RivalID + "],请出拳...");
                        setButtonState(3);
                    }
                }
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_BetEndEvent:" + ex.Message);
            }
            //    }
            //));
        }

        private void _game_GameReportEvent(object sender, string content)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
            report(content);
            //    }));
        }

        private void _game_RoundEndEvent(object sender)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
            try
            {
                if (PlayerState.Watching == _game.CurrentPlayerState)
                {
                    log(sender, "[占位符:观战显示内容]...");
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
                                    log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                    log(sender, "小样儿吧,你个[" + rivalId + "],见识到大爷的厉害了吧!!!哈哈哈!!");
                                    log(sender, "等待新的挑战者...");
                                    setButtonState(4);
                                }
                                else if (RoundResult.Lose == _game.Me.RoundResult && gesture != "未知")
                                {
                                    log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                    log(sender, "老子[" + _game.Me.Name + "]十八年后还是一条好汉,你[" + rivalId + "]等着瞧!!!");
                                    log(sender, "稍后开始新一轮下注...");
                                    setButtonState(4);
                                }
                                else if (RoundResult.Draw == _game.Me.RoundResult && gesture != "未知")
                                {
                                    log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                    log(sender, "哟呵!屎壳郎拦吉普你[" + rivalId + "]还和我[" + _game.Me.Name + "]顶上了,再来,看我不灭了你!!!");
                                    log(sender, "请重新出拳...");
                                    setButtonState(3);
                                }
                                else if (RoundResult.Unknown == _game.Me.RoundResult)
                                {
                                    log(sender, "新的挑战者出现了!!![" + rivalId + "]竖起了手指在向你挑衅了,干掉他!!!");
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
                                        log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                        log(sender, "看来对手太菜了,恭喜你轻松获胜!!!");
                                        log(sender, "等待新的对手...");
                                        setButtonState(4);
                                    }
                                    else
                                    {
                                        log(sender, "你的对手是[" + rivalId + "],请出拳...");
                                        setButtonState(3);
                                    }
                                }
                            }
                            else if (RoundResult.Lose == _game.Me.RoundResult)
                            {
                                if (PlayerState.Watching != _game.Me.PlayerState)
                                {
                                    log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                    log(sender, "居然..输了..唉!既生瑜何生亮啊!!!");
                                    log(sender, "稍后转入观战...");
                                    setButtonState(4);
                                }
                            }
                            else if (RoundResult.Unknown == _game.Me.RoundResult && !string.IsNullOrEmpty(rivalId))
                            {
                                log(sender, "你的对手是[" + rivalId + "],请出拳...");
                                setButtonState(3);
                            }
                            else if (RoundResult.Draw == _game.Me.RoundResult)
                            {
                                log(sender, "你出[" + gesture + "],对手出[" + rivalGesture + "]...");
                                log(sender, "居然是平局!得好好想想接下来出什么了呀!!!");
                                log(sender, "请重新出拳...");
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
                            log(sender, "运气真好!恭喜你本轮获得自动晋级的机会!!");
                            log(sender, "请稍后进入下一轮!");
                            setButtonState(4);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_RoundEndEvent:" + ex.Message);
            }
            //    }
            //));
        }

        private void _game_RoundStartEvent(object sender)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
            try
            {
                if (!string.IsNullOrEmpty(_game.Me.RivalID))
                {
                    if (_game.Me.GameState < GameState.PKStart)
                    {
                        if (GuessGesture.Unknown != _game.Me.Rival.Gesture)
                        {
                            log(sender, "你的对手已经出拳!");
                        }
                        else if (GuessGesture.Unknown == _game.Me.Gesture)
                        {
                            log(sender, "你的对手是[" + _game.Me.RivalID + "],请出拳...");
                            //log(sender, "请出拳...", KnownColor.SkyBlue);
                            //btnConnect.Enabled = false;
                            setButtonState(3);
                        }
                    }
                    else
                    {
                        if (GuessGesture.Unknown != _game.Me.Rival.Gesture)
                        {
                            log(sender, "你的对手已经出拳!");
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_RoundStartEvent:" + ex.Message);
            }
            //    }

            //    ));
        }

        private void _game_KOEndEvent(object sender)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
            try
            {
                setButtonState(4);

                log(sender, "比赛结束!!!接下来将是紧张刺激的擂台大战...");

                _game.updateChampion();
                if (_game.Champion != null)
                {
                    log(sender, "冠军是[" + _game.Champion.ID + "]");

                    if (_game.Champion.ID == _game.Me.ID)
                    {
                        //this.WindowState = FormWindowState.Maximized;
                        log(sender, "擂主大人,先喝个小酒,等等挑战者吧...");
                        //_shineTimer.Start();
                    }
                }
            }
            catch (System.Exception ex)
            {
                log(sender, "_game_KOEndEvent:" + ex.Message);
            }
            //    })
            //    );
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //_shineTimes++;
            //this.BeginInvoke(new MethodInvoker(() =>
            //{
            //    if (txtLog.BackColor == Color.Black)
            //    {
            //        txtLog.BackColor = Color.SlateBlue;
            //    }
            //    else
            //    {
            //        txtLog.BackColor = Color.Black;
            //    }
            //}));
            //if (_shineTimes == _TotalShineTimes)
            //{
            //    _shineTimes = 0;
            //    _shineTimer.Stop();
            //}
        }

        private void _game_KOStartEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            log(sender, "比赛正式开始...");
            //        setButtonState(4);
            //    }
            //    ));
        }

        private void _game_WatchingEvent(object sender)
        {
            // this.BeginInvoke(
            //     new MethodInvoker(() =>
            //     {
            log(sender, "围观中...");
            //         setButtonState(4);
            //     }
            //));
        }

        void _game_SignedEvent(object sender)
        {
            //this.BeginInvoke(
            //    new MethodInvoker(() =>
            //    {
            if (PlayerState.Signed == _game.CurrentPlayerState)
            {
                log(sender, "报名成功...");
                setButtonState(1);
            }
            else
            {
                log(sender, "报名不成功...");
            }
            //    }
            //));
        }

        private void _game_QuitGameEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            log(sender, "退出游戏?你会后悔的...");
            //    }
            //));
        }

        private void _game_DisConnectedEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            //        btnConnect.Text = "连接";
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;

            log(sender, "断开连接...");
            //    }
            //));
        }

        private void _game_ConnectedEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            if (_game.Connected)
            {
                //btnConnect.Text = "断开";
                log(sender, "登录到服务器");
                //btnSign.Enabled = true;
                //btnWatching.Enabled = true;

            }
            else
            {
                //btnConnect.Text = "连接";
                log(sender, "登录失败");
                //btnSign.Enabled = false;
                //btnWatching.Enabled = false;
            }
            //    }
            //));
        }

        private void _game_AnnounceEvent(object sender, string content)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            log(sender, "[系统公告]:" + content);
            //        MessageBox.Show(content, "系统公告");
            //    }));
        }

        #endregion  游戏事件

        #region 控件事件
        void btnSign_Click(object sender, EventArgs e)
        {
            try
            {
                _game.Sign();
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(this.ApplicationContext, ex.Message, ToastLength.Short);
            }
        }

        void btnJian_Click(object sender, EventArgs e)
        {
            _game.Guess((int)GuessGesture.Jian);
        }

        void btnChui_Click(object sender, EventArgs e)
        {
            _game.Guess((int)GuessGesture.Chui);
        }

        void btnBet_Click(object sender, EventArgs e)
        {
            _game.Bet(200);
        }

        void btnBao_Click(object sender, EventArgs e)
        {
            _game.Guess((int)GuessGesture.Bao);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ActivityMain));
            this.StartActivity(intent);
            this.Finish();
        }

        #endregion 控件事件
    }
}