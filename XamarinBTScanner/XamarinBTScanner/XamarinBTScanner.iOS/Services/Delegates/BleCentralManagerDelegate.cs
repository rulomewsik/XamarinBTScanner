using System;
using CoreBluetooth;
using Foundation;
using XamarinBTScanner.iOS.Contracts.Services.Delegates;

namespace XamarinBTScanner.iOS.Services.Delegates
{
    public class BleCentralManagerDelegate : CBCentralManagerDelegate, IBleCentralManagerDelegate
    {
        public event EventHandler<CBDiscoveredPeripheralEventArgs> OnDiscoveredPeripheral;

        public override void DiscoveredPeripheral(CBCentralManager central, CBPeripheral peripheral,
            NSDictionary advertisementData, NSNumber rssi)
        {
            OnDiscoveredPeripheral?.Invoke(this,
                new CBDiscoveredPeripheralEventArgs(peripheral, advertisementData, rssi));
        }

        public event EventHandler OnStateUpdated;

        public override void UpdatedState(CBCentralManager central)
        {
            OnStateUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}