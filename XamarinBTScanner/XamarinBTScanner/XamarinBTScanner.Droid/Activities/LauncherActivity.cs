using Android.App;
using Android.Content;
using AndroidX.AppCompat.App;

namespace XamarinBTScanner.Droid.Activities
{
    [Activity(
        Label = "Turtle Scanner",
        Theme = "@style/MainTheme.SplashTheme",
        MainLauncher = true,
        NoHistory = true,
        Icon = "@mipmap/ic_launcher",
        RoundIcon = "@mipmap/ic_launcher_round")]
    public class LauncherActivity : AppCompatActivity
    {
        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof (MainActivity)));
        }
    }
}