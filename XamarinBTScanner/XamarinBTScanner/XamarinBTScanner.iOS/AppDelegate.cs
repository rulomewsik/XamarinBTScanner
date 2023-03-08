using FFImageLoading.Forms.Platform;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinBTScanner.Contracts.Adapters;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.iOS.Adapters;
using XamarinBTScanner.iOS.Services;

namespace XamarinBTScanner.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            CachedImageRenderer.Init();
            CachedImageRenderer.InitImageSourceHandler();
            Forms.Init();
            LoadApplication(new App());

            RegisterServices();

            return base.FinishedLaunching(app, options);
        }

        private static void RegisterServices()
        {
            DependencyService.Register<IBluetoothPermissionService, BluetoothPermissionService>();
            DependencyService.Register<IBluetoothService, BluetoothService>();
            DependencyService.Register<IBluetoothAdapter, BluetoothAdapter>();
        }
    }
}