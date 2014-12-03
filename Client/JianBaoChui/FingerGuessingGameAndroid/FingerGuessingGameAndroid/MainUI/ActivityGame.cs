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

        #region ˽�з���
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
            //    case 1:     //�����ɹ�
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 2:     //�ҳ�ȭ
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 3:     //�غϿ�ʼ
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = true;
            //        btnJian.Enabled = true;
            //        btnChui.Enabled = true;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 4:     //�غϽ���
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;
            //    case 5:     //��������
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;
            //        break;

            //    case 6:     //��ע��ʼ
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = true;
            //        cmbPoint.Enabled = true;

            //        break;

            //    case 7:     //��ע����
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

        #endregion ˽�з���

        #region ��Ϸ�¼�

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

                    log(sender, "����ע������ս��...");
                    if (_game.Me.BetPoint > 0)
                    {
                        if (PlayerStatus.Winner != _game.Me.PlayerStatus)
                        {
                            log(sender, "������ע[" + _game.Me.BetPoint + "]��...");
                            string names = "";
                            _game.GetTopMostPointPlayer().ForEach(delegate(string name) { names += name + ","; });
                            log(sender, "��ǰ��ע���Top 5:[" + names.TrimEnd(',') + "]");
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
                log(sender, "�������Ϸ�Ѿ�Բ������,��ϲ[" + _game.Champion.Name + "]Ц�������...");

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
                    log(sender, "��������������[" + _game.Champion.ID + "]����ս��[" + _game.Champion.RivalID + "]�ļ��ҶԾ�!!!");

                    if (!string.IsNullOrEmpty(_game.Me.RivalID))
                    {
                        log(sender, "��Ķ�����[" + _game.Me.RivalID + "],���ȭ...");
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
                    log(sender, "[ռλ��:��ս��ʾ����]...");
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
                                if (RoundResult.Win == _game.Me.RoundResult && gesture != "δ֪")
                                {
                                    log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                    log(sender, "С������,���[" + rivalId + "],��ʶ����ү�������˰�!!!������!!");
                                    log(sender, "�ȴ��µ���ս��...");
                                    setButtonState(4);
                                }
                                else if (RoundResult.Lose == _game.Me.RoundResult && gesture != "δ֪")
                                {
                                    log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                    log(sender, "����[" + _game.Me.Name + "]ʮ�������һ���ú�,��[" + rivalId + "]������!!!");
                                    log(sender, "�Ժ�ʼ��һ����ע...");
                                    setButtonState(4);
                                }
                                else if (RoundResult.Draw == _game.Me.RoundResult && gesture != "δ֪")
                                {
                                    log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                    log(sender, "Ӵ��!ʺ������������[" + rivalId + "]������[" + _game.Me.Name + "]������,����,���Ҳ�������!!!");
                                    log(sender, "�����³�ȭ...");
                                    setButtonState(3);
                                }
                                else if (RoundResult.Unknown == _game.Me.RoundResult)
                                {
                                    log(sender, "�µ���ս�߳�����!!![" + rivalId + "]��������ָ������������,�ɵ���!!!");
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
                                        log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                        log(sender, "��������̫����,��ϲ�����ɻ�ʤ!!!");
                                        log(sender, "�ȴ��µĶ���...");
                                        setButtonState(4);
                                    }
                                    else
                                    {
                                        log(sender, "��Ķ�����[" + rivalId + "],���ȭ...");
                                        setButtonState(3);
                                    }
                                }
                            }
                            else if (RoundResult.Lose == _game.Me.RoundResult)
                            {
                                if (PlayerState.Watching != _game.Me.PlayerState)
                                {
                                    log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                    log(sender, "��Ȼ..����..��!����褺�������!!!");
                                    log(sender, "�Ժ�ת���ս...");
                                    setButtonState(4);
                                }
                            }
                            else if (RoundResult.Unknown == _game.Me.RoundResult && !string.IsNullOrEmpty(rivalId))
                            {
                                log(sender, "��Ķ�����[" + rivalId + "],���ȭ...");
                                setButtonState(3);
                            }
                            else if (RoundResult.Draw == _game.Me.RoundResult)
                            {
                                log(sender, "���[" + gesture + "],���ֳ�[" + rivalGesture + "]...");
                                log(sender, "��Ȼ��ƽ��!�úú������������ʲô��ѽ!!!");
                                log(sender, "�����³�ȭ...");
                                setButtonState(3);
                            }
                            //}
                            //else if (RoundResult.Draw == _game.Me.RoundResult)
                            //{
                            //    log(sender, "��Ȼ��ƽ��!�úú������������ʲô��ѽ!!!", KnownColor.SkyBlue);
                            //    log(sender, "�����³�ȭ...", KnownColor.SkyBlue);
                            //}
                        }
                    }
                    else
                    {
                        //�Զ�����
                        if (GuessGesture.Unknown == _game.Me.Gesture)
                        {
                            log(sender, "�������!��ϲ�㱾�ֻ���Զ������Ļ���!!");
                            log(sender, "���Ժ������һ��!");
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
                            log(sender, "��Ķ����Ѿ���ȭ!");
                        }
                        else if (GuessGesture.Unknown == _game.Me.Gesture)
                        {
                            log(sender, "��Ķ�����[" + _game.Me.RivalID + "],���ȭ...");
                            //log(sender, "���ȭ...", KnownColor.SkyBlue);
                            //btnConnect.Enabled = false;
                            setButtonState(3);
                        }
                    }
                    else
                    {
                        if (GuessGesture.Unknown != _game.Me.Rival.Gesture)
                        {
                            log(sender, "��Ķ����Ѿ���ȭ!");
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

                log(sender, "��������!!!���������ǽ��Ŵ̼�����̨��ս...");

                _game.updateChampion();
                if (_game.Champion != null)
                {
                    log(sender, "�ھ���[" + _game.Champion.ID + "]");

                    if (_game.Champion.ID == _game.Me.ID)
                    {
                        //this.WindowState = FormWindowState.Maximized;
                        log(sender, "��������,�Ⱥȸ�С��,�ȵ���ս�߰�...");
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
            log(sender, "������ʽ��ʼ...");
            //        setButtonState(4);
            //    }
            //    ));
        }

        private void _game_WatchingEvent(object sender)
        {
            // this.BeginInvoke(
            //     new MethodInvoker(() =>
            //     {
            log(sender, "Χ����...");
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
                log(sender, "�����ɹ�...");
                setButtonState(1);
            }
            else
            {
                log(sender, "�������ɹ�...");
            }
            //    }
            //));
        }

        private void _game_QuitGameEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            log(sender, "�˳���Ϸ?����ڵ�...");
            //    }
            //));
        }

        private void _game_DisConnectedEvent(object sender)
        {
            //this.Invoke(
            //    new MethodInvoker(() =>
            //    {
            //        btnConnect.Text = "����";
            //        btnSign.Enabled = false;
            //        btnWatching.Enabled = false;

            //        btnBao.Enabled = false;
            //        btnJian.Enabled = false;
            //        btnChui.Enabled = false;

            //        btnBet.Enabled = false;
            //        cmbPoint.Enabled = false;

            log(sender, "�Ͽ�����...");
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
                //btnConnect.Text = "�Ͽ�";
                log(sender, "��¼��������");
                //btnSign.Enabled = true;
                //btnWatching.Enabled = true;

            }
            else
            {
                //btnConnect.Text = "����";
                log(sender, "��¼ʧ��");
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
            log(sender, "[ϵͳ����]:" + content);
            //        MessageBox.Show(content, "ϵͳ����");
            //    }));
        }

        #endregion  ��Ϸ�¼�

        #region �ؼ��¼�
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

        #endregion �ؼ��¼�
    }
}