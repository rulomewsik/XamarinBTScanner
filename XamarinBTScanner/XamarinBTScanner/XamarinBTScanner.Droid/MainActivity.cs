using Android.App;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.Droid.Services;
using Platform = Xamarin.Essentials.Platform;

namespace XamarinBTScanner.Droid
{
    [Activity(Label = "XamarinBTScanner", Theme = "@style/MainTheme", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            CachedImageRenderer.Init(false);
            CachedImageRenderer.InitImageViewHandler();
            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            
            RegisterServices();
            LoadApplication(new App());
        }

        private static void RegisterServices()
        {
            DependencyService.Register<IBluetoothPermissionService, BluetoothPermissionService>();
            DependencyService.Register<IBluetoothService, BluetoothService>();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}