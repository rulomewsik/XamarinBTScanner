using System.Threading.Tasks;
using Android.OS;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.Droid.Services;
using XamarinBTScanner.Droid.Utils;

[assembly: Dependency(typeof(BluetoothPermissionService))]

namespace XamarinBTScanner.Droid.Services
{
    public class BluetoothPermissionService : IBluetoothPermissionService
    {
        public async Task<PermissionStatus> CheckAndRequestPermission()
        {
            PermissionStatus status;
            if (Build.VERSION.SdkInt >= BuildVersionCodes.S)
            {
                status = await Permissions.CheckStatusAsync<AndroidSBluetoothPermission>();

                if (status == PermissionStatus.Granted)
                    return status;

                status = await Permissions.RequestAsync<AndroidSBluetoothPermission>();
            }
            else
            {
                status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                    return status;


                if (Permissions.ShouldShowRationale<Permissions.LocationWhenInUse>())
                {
                    await Application.Current.MainPage.DisplayAlert("Permission required",
                        "Location permission is required for bluetooth scanning.", "OK");
                }

                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            return status;
        }
    }
}