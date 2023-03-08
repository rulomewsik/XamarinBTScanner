using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinBTScanner.Contracts.Services
{
    public interface IBluetoothPermissionService
    {
        /// <summary>
        /// Validates Bluetooth permission and tries to request if not been granted already
        /// </summary>
        /// <returns></returns>
        Task<PermissionStatus> CheckAndRequestPermission();
    }
}