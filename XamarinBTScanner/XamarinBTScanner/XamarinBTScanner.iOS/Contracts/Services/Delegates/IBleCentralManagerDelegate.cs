using System;
using CoreBluetooth;

namespace XamarinBTScanner.iOS.Contracts.Services.Delegates
{
    public interface IBleCentralManagerDelegate
    {
        event EventHandler<CBDiscoveredPeripheralEventArgs> OnDiscoveredPeripheral;
        event EventHandler OnStateUpdated;
    }
}