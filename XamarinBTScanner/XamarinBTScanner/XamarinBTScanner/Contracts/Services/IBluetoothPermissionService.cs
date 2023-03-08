using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XamarinBTScanner.Contracts.Services
{
    public interface IBluetoothPermissionService
    {
        Task<PermissionStatus> CheckAndRequestPermission();
    }
}