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
    public partial class FormAutoGame : Form
    {
        public List<Game> _gameList = new List<Game>();
        public List<Thread> _threadList = new List<Thread>();
        public int _onlyOneFlag = 0;
        public FormAutoGame()
        {
            InitializeComponent();

        }

        #region 私有方法

        private void log(object sender, string content, KnownColor color = KnownColor.White)
        {
            string id = string.Empty;
            if (sender.GetType() == typeof(SocketClient))
            {
                var client = (SocketClient)sender;
                id = client.Name.ToString();
            }

            if (sender.GetType() == typeof(Game))
            {
                var game = (Game)sender;
                id = game.ID;
            }

            content = content.Replace("\0", "");
            string logText = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]" + "<" + id + ">：" + content + Environment.NewLine;
            txtLog.AppendText(logText);
            txtLog.Select(txtLog.TextLength - logText.Length + 1, logText.Length - 1);
            txtLog.SelectionColor = Color.FromKnownColor(color);
            txtLog.ScrollToCaret();
        }

        private void report(object sender, string content, KnownColor color = KnownColor.DarkGreen)
        {
            var game = (Game)sender;
            string logText = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]" + "<" + game.ID + ">：" + content + Environment.NewLine;
            txtReport.AppendText(logText);
            txtReport.Select(txtReport.TextLength - logText.Length + 1, logText.Length - 1);
            txtReport.SelectionColor = Color.FromKnownColor(color);
            txtReport.ScrollToCaret();
        }

        //private void updateUI()
        //{
        //    while (true)
        //    {
        //        if (_game != null)
        //        {
        //            this.Invoke(
        //                new MethodInvoker(() =>
        //                {
        //                    //update player panel
        //                    lblID.Text = _game.ID;
        //                    if (_game.Me != null)
        //                    {
        //                        //lblID.Text = _game.Me.ID;
        //                        lblName.Text = _game.Me.Name;
        //                        lblGroupNo.Text = _game.Me.LocY + _game.Me.LocX.ToString();
        //                        lblPlayerState.Text = _game.Me.PlayerState.ToString();
        //                        lblStatus.Text = _game.Me.GameStatus.ToString();
        //                        lblRivalName.Text = _game.Me.RivalID;
        //                        lblRivalGesture.Text = _game.Me.Rival == null ? string.Empty : _game.Me.Rival.Gesture.ToString();
        //                        lblPoint.Text = _game.Me.BetPoint.ToString();
        //                        lblGameState.Text = _game.Me.GameState.ToString();
        //                    }

        //                }));
        //        }
        //        Thread.Sleep(3000);
        //    }
        //}

        #endregion

        #region 窗体事件
        private void FormAutoGame_Load(object sender, EventArgs e)
        {
            int count = Properties.Settings.Default.ThreadCount;
            for (int i = 0; i < count; i++)
            {
                var game = new Game("T_" + i.ToString());
                game.ConnectedEvent += game_ConnectedEvent;
                game.SignedEvent += game_SignedEvent;
                game.BeforeKOStartEvent += game_BeforeKOStartEvent;
                game.KOStartEvent += game_KOStartEvent;
                game.RoundStartEvent += game_RoundStartEvent;
                game.RoundEndEvent += game_RoundEndEvent;
                game.KOEndEvent += game_KOEndEvent;
                game.PKStartEvent += game_PKStartEvent;
                game.PKEndEvent += game_PKEndEvent;
                game.LogEvent += game_LogEvent;
                game.DisConnectedEvent += game_DisConnectedEvent;
                game.BeforeRoundStartEvent += game_BeforeRoundStartEvent;
                game.KOEvent += game_KOEvent;
                game.WatchingEvent += game_WatchingEvent;
                game.GameReportEvent += game_GameReportEvent;
                game.Init();
                _gameList.Add(game);
            }

            foreach (var game in _gameList)
            {
                _threadList.Add(new Thread(new ThreadStart(game.Connect)) { Name = game.ID });
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            foreach (var thread in _threadList)
            {
                thread.IsBackground = true;
                thread.Start();
            }
        }

        private void btnQuitGame_Click(object sender, EventArgs e)
        {
            foreach (var thread in _threadList)
            {
                thread.Abort();
            }

        }
        private void btnShowAllPlayer_Click(object sender, EventArgs e)
        {
            FormPlayers form = new FormPlayers(_gameList[0].AllPlayers);
            form.ShowDialog(this);
        }

        private void FormAutoGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        #endregion

        #region 游戏事件
        private void game_LogEvent(object sender, string content, KnownColor color = KnownColor.White)
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

        private void game_BetEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "开始下注...", KnownColor.LimeGreen);

                    var game = ((Game)sender);
                    int point = GameUtil.Random(5) * 100;
                    int waitingSeconds = GameUtil.Random(5);
                    game.Bet(point);
                    Application.DoEvents();
                    Thread.Sleep(waitingSeconds * 1000);
                    log(sender, "下注[" + point + "]点", KnownColor.Yellow);

                }
            ));
        }

        private void game_PKEndEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    GamePlayer champion = ((Game)sender).Champion;
                    log(sender, "今天的游戏已经圆满结束,恭喜[" + champion.Name + "]笑到了最后...", KnownColor.LimeGreen);
                }
            ));
        }

        private void game_PKStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    GamePlayer champion = ((Game)sender).Champion;
                    if (champion != null)
                    {
                        log(sender, "擂台赛正式开始,本轮擂主是[" + champion.Name + "]...", KnownColor.SkyBlue);
                    }
                }
            ));
        }

        private void game_RoundEndEvent(object sender)
        {
            var game = (Game)sender;
            this.Invoke(
                new MethodInvoker(() =>
                    {
                        log(sender, "对手出的是[" + (game.Me.Rival == null ? "未知对手" : GameUtil.GetGestureName(game.Me.Rival.Gesture)) + "]...", KnownColor.SkyBlue);

                        if (RoundResult.Win == game.Me.RoundResult)
                        {
                            log(sender, "看来对手太菜了,恭喜你轻松获胜!!!", KnownColor.SkyBlue);
                        }
                        else if (RoundResult.Lose == game.Me.RoundResult)
                        {
                            log(sender, "居然..输了..唉!既生瑜何生亮啊!!!", KnownColor.SkyBlue);
                        }
                        else if (RoundResult.Draw == game.Me.RoundResult)
                        {
                            log(sender, "居然是平局!得好好想想接下来出什么了呀!!!", KnownColor.SkyBlue);
                        }

                        log(sender, "请稍后...", KnownColor.SkyBlue);
                        System.Windows.Forms.Application.DoEvents();
                        int waitingSeconds = GameUtil.Random(5);
                        Thread.Sleep(waitingSeconds * 1000);
                    }
            ));
        }

        private void game_RoundStartEvent(object sender)
        {
            var game = ((Game)sender);
            this.Invoke(
                new MethodInvoker(() =>
                {
                    var rival = game.Me.Rival;
                    string rivalName = string.Empty;
                    if (rival == null)
                    {
                        rivalName = "未知";
                    }
                    else
                    {
                        rivalName = rival.Name;
                    }

                    log(sender, "对手是[" + rivalName + "]...", KnownColor.SkyBlue);
                    log(sender, "准备出拳...", KnownColor.SkyBlue);


                    int gesture = GameUtil.Random(3);
                    game.Guess(gesture);
                    Application.DoEvents();
                    int waitingSeconds = GameUtil.Random(5);
                    Thread.Sleep(waitingSeconds * 1000);
                    log(sender, "出拳...[" + GameUtil.GetGestureName((GuessGesture)gesture) + "]", KnownColor.Yellow);
                }
                ));
        }

        private void game_BeforeRoundStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "等待对手...", KnownColor.SkyBlue);
                }
            ));
        }

        private void game_KOEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "比赛中...");
                })
                );
        }

        private void game_KOEndEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "比赛结束...");
                    log(sender, "稍后PK开始...");
                })
                );
        }

        private void game_KOStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "比赛开始...", KnownColor.SkyBlue);
                }
                ));
        }

        private void game_WatchingEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "围观...", KnownColor.SkyBlue);
                }
           ));
        }

        private void game_SignedEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    if (PlayerState.Signed == ((Game)sender).CurrentPlayerState)
                    {
                        log(sender, "报名成功...", KnownColor.SkyBlue);

                    }
                    else
                    {
                        log(sender, "报名不成功...", KnownColor.Red);
                    }
                }
            ));
        }

        private void game_QuitGameEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "退出游戏...", KnownColor.SkyBlue);
                }
            ));
        }

        private void game_BeforeKOStartEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "稍后比赛开始...", KnownColor.SkyBlue);
                }
            ));
        }

        private void game_DisConnectedEvent(object sender)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    log(sender, "断开连接...", KnownColor.SkyBlue);
                }
            ));
        }

        private void game_ConnectedEvent(object sender)
        {
            var game = (Game)sender;
            this.Invoke(
                new MethodInvoker(() =>
                {
                    if (game.Connected)
                    {
                        log(sender, "登录到服务器...", KnownColor.SkyBlue);


                        log(sender, "提交报名请求...", KnownColor.SkyBlue);
                        game.Sign();
                        Application.DoEvents();
                        int waitingSeconds = GameUtil.Random(5);
                        Thread.Sleep(waitingSeconds * 1000);
                    }
                    else
                    {
                        log(sender, "登录失败...", KnownColor.Red);
                    }
                }
            ));
        }

        private void game_GameReportEvent(object sender, string content, KnownColor color = KnownColor.DarkGreen)
        {
            this.Invoke(
                new MethodInvoker(() =>
                {
                    report(sender, content, color);
                }));
        }

        #endregion  游戏事件

    }
}
