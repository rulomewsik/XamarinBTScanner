using System;
using System.Threading.Tasks;
using CoreBluetooth;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.iOS.Services;
using XamarinBTScanner.Models;

[assembly: Dependency(typeof(BluetoothService))]

namespace XamarinBTScanner.iOS.Services
{
    public class BluetoothService : IDisposable, IBluetoothService
    {
        private readonly CBCentralManager _manager = new CBCentralManager();

        public EventHandler<CBCentralManagerState> StateChanged;
        public event EventHandler<BluetoothDevice> DiscoveredDevice;

        public BluetoothService()
        {
            _manager.DiscoveredPeripheral += ManagerOnDiscoveredPeripheral;
            _manager.UpdatedState += ManagerOnUpdatedState;
        }

        private void ManagerOnDiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            var discoveredDevice = new BluetoothDevice
            {
                Name = e.Peripheral.Name,
                Identifier = Guid.Parse(e.Peripheral.Identifier.AsString()),
                Rssi = e.RSSI.Int32Value
            };

            DiscoveredDevice?.Invoke(this, discoveredDevice);
        }

        private void ManagerOnUpdatedState(object sender, EventArgs e)
        {
            StateChanged?.Invoke(sender, _manager.State);
        }

        public async Task StartScanner(int scanDuration, string serviceUuid)
        {
            var uuids = string.IsNullOrEmpty(serviceUuid)
                ? Array.Empty<CBUUID>()
                : new[] {CBUUID.FromString(serviceUuid)};
            _manager.ScanForPeripherals(uuids);

            await Task.Delay(scanDuration);
            StopScanner();
        }

        public void StopScanner()
        {
            _manager.StopScan();
        }

        public void Dispose()
        {
            _manager.DiscoveredPeripheral -= ManagerOnDiscoveredPeripheral;
            _manager.UpdatedState -= ManagerOnUpdatedState;
        }
    }
}