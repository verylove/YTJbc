using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Runtime;

namespace FingerGuessingGameAndroid.MainUI
{
    [Application(Label = "GuessGame")]
    class GameApplication : Application
    {
        public Game GameController { get; set; }
        public GameApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override void OnCreate()
        {
            base.OnCreate();
            GameController = new Game();
        }
    }
}
