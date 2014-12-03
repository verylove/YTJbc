using Android.App;
using Android.Content.PM;
using Android.OS;

namespace GameCore
{
    [Activity(Label = "GameCore"
        , MainLauncher = true
        , Icon = "@drawable/icon"
        , Theme = "@style/Theme.Splash"
        , AlwaysRetainTaskState = true
        , LaunchMode = Android.Content.PM.LaunchMode.SingleInstance
        , ScreenOrientation = ScreenOrientation.SensorLandscape
        , ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.Keyboard | ConfigChanges.KeyboardHidden)]
    public class ActivityCore : Microsoft.Xna.Framework.AndroidGameActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            GameEngine.Activity = this;
            var g = new GameEngine();
            SetContentView(g.Window);
            g.Run();
        }
    }
}

