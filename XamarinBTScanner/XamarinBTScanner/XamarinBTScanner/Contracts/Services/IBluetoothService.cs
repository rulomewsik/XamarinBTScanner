using System;
using System.Threading.Tasks;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.Contracts.Services
{
    public interface IBluetoothService
    {
        Task StartScanner(int scanDuration, string serviceUuid);
        
        void StopScanner();
        
        event EventHandler<BluetoothDevice> DiscoveredDevice;
    }
}