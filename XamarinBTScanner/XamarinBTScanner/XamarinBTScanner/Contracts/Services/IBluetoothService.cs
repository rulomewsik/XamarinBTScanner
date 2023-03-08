using XamarinBTScanner.Contracts.Adapters;

namespace XamarinBTScanner.Contracts.Services
{
    public interface IBluetoothService
    {
        /// <summary>
        /// Adapter class that provides access to the physical bluetooth adapter.
        /// </summary>
        IBluetoothAdapter Adapter { get; }
    }
}