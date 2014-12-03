using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FingerGuessingGameAndroid.MainUI;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace FingerGuessingGameAndroid.MainUI
{
    [Activity(Label = "石头剪子布", MainLauncher = true, Icon = "@drawable/icon")]
    public class ActivityMain : Activity
    {
        Game _game = null;
        ProgressDialog _progressDlg = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            InitUIControl();
            InitGame();
        }

        #region 控件初始化

        private void InitUIControl()
        {
            //服务器IP
            var txtServerIP = FindViewById<EditText>(Resource.Id.txtServerIP);

            // Get our button from the layout resource,
            // and attach an event to it
            Button btnConnect = FindViewById<Button>(Resource.Id.btnConnect);
            btnConnect.Click += btnConnect_Click;
        }

        #region 控件事件

        void btnConnect_Click(object sender, EventArgs e)
        {
            _progressDlg = ProgressDialog.Show(this, "游戏控制器", "正在连接服务器...", true, true);
            _game.Connect();
            _progressDlg.CancelEvent += _progressDlg_CancelEvent;
        }

        void _progressDlg_CancelEvent(object sender, EventArgs e)
        {
            ShowMessage(this, "连接取消!");
        }

        #endregion 控件事件

        #endregion

        #region 异步方式显示AlertDialog
        delegate void ShowMessageDelegate(Activity activity, string content);
        private ShowMessageDelegate _ShowMessageHandler = delegate(Activity activity, string content)
        {
            try
            {
                Looper.Prepare();
                var builder = new AlertDialog.Builder(activity.ApplicationContext);
                builder.SetTitle("游戏控制器");
                builder.SetMessage(content);
                builder.SetPositiveButton("我知道了", (sender, args) => { });
                builder.Show();
                Looper.Loop();
            }
            catch (System.Exception ex)
            {
                Toast.MakeText(activity.ApplicationContext, ex.Message, ToastLength.Long);
            }
        };

        private void ShowMessage(Activity activity, string content)
        {
            _ShowMessageHandler.BeginInvoke(activity, content, null, null);
        }
        #endregion

        #region 游戏控制类初始化
        private void InitGame()
        {
            _game = ((GameApplication)Application).GameController;            
            //_game.ShowSocketLog = false;
            //_game.LogEvent += _game_LogEvent;
            _game.ConnectedEvent += _game_ConnectedEvent;
            _game.Init();
        }

        void _game_LogEvent(object sender, string content)
        {
            ShowMessage(this, content);
            _progressDlg.Dismiss();
        }

        void _game_ConnectedEvent(object sender)
        {
            try
            {
                _progressDlg.Dismiss();
                ShowMessage(this, "连接服务器成功!");

                GameApplication gameApp = (GameApplication)Application;
                gameApp.GameController = _game;
                Intent intent = new Intent();
                intent.SetClass(this, typeof(ActivityGame));
                this.StartActivity(intent);
            }
            catch (System.Exception ex)
            {
                ShowMessage(this, ex.Message);
            }
        }
        #endregion 游戏控制类初始化

    }


}

