using System.Threading.Tasks;
using CoreBluetooth;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.iOS.Services;

[assembly: Dependency(typeof(BluetoothPermissionService))]

namespace XamarinBTScanner.iOS.Services
{
    public class BluetoothPermissionService : IBluetoothPermissionService
    {
        public async Task<PermissionStatus> CheckAndRequestPermission()
        {
            var cbCentralManager = new CBCentralManager();
            PermissionStatus status;
            do
            {
                status = await CheckPermission();
                await Task.Delay(200);
            } while (status == PermissionStatus.Unknown);

            return status;
        }

        private static Task<PermissionStatus> CheckPermission()
        {
            switch (CBManager.Authorization)
            {
                case CBManagerAuthorization.AllowedAlways:
                    return Task.FromResult(PermissionStatus.Granted);
                case CBManagerAuthorization.Restricted:
                    return Task.FromResult(PermissionStatus.Restricted);
                case CBManagerAuthorization.NotDetermined:
                    return Task.FromResult(PermissionStatus.Unknown);
                case CBManagerAuthorization.Denied:
                default:
                    return Task.FromResult(PermissionStatus.Denied);
            }
        }
    }
}