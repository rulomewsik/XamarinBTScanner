using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoreBluetooth;
using XamarinBTScanner.Contracts.Adapters;
using XamarinBTScanner.iOS.Contracts.Services.Delegates;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.iOS.Adapters
{
    public class BluetoothAdapter : IBluetoothAdapter
    {
        private CancellationTokenSource _scanCancellationTokenSource;
        private readonly CBCentralManager _centralManager;

        public bool IsScanning { get; private set; }
        public ObservableCollection<BluetoothDevice> DiscoveredDevices { get; private set; }
        public event EventHandler<BluetoothDevice> DiscoveredDevice;
        public event EventHandler ScanTimeout;
        public event EventHandler<string> ScanFailed;

        public BluetoothAdapter(CBCentralManager centralManager, IBleCentralManagerDelegate bleCentralManagerDelegate)
        {
            DiscoveredDevices = new ObservableCollection<BluetoothDevice>();
            _centralManager = centralManager;

            bleCentralManagerDelegate.OnDiscoveredPeripheral += BleCentralManagerDelegateOnOnDiscoveredPeripheral;
        }

        private void BleCentralManagerDelegateOnOnDiscoveredPeripheral(object sender, CBDiscoveredPeripheralEventArgs e)
        {
            var discoveredDevice = new BluetoothDevice
            {
                Name = string.IsNullOrEmpty(e.Peripheral.Name) ? "N/A" : e.Peripheral.Name,
                Identifier = Guid.Parse(e.Peripheral.Identifier.AsString()),
                Address = Guid.Parse(e.Peripheral.Identifier.AsString()).ToString(),
                Rssi = e.RSSI.Int32Value
            };

            if (DiscoveredDevices.Any(d => d.Identifier == discoveredDevice.Identifier))
            {
                var repeatedDevice = DiscoveredDevices.FirstOrDefault(d => d.Identifier == discoveredDevice.Identifier);
                DiscoveredDevices[DiscoveredDevices.IndexOf(repeatedDevice)] = discoveredDevice;
            }
            else
            {
                DiscoveredDevices.Add(discoveredDevice);
            }

            DiscoveredDevices =
                new ObservableCollection<BluetoothDevice>(DiscoveredDevices.OrderByDescending(d => d.Rssi));

            DiscoveredDevice?.Invoke(this, discoveredDevice);
        }

        public async Task StartScanner(int scanTimeout, CancellationToken cancellationToken = default)
        {
            if (IsScanning) return;

            IsScanning = true;
            _scanCancellationTokenSource = new CancellationTokenSource();

            try
            {
                DiscoveredDevices.Clear();

                await using (cancellationToken.Register(() => _scanCancellationTokenSource?.Cancel()))
                {
                    _centralManager.ScanForPeripherals(Array.Empty<CBUUID>());
                    await Task.Delay(scanTimeout, _scanCancellationTokenSource.Token);
                    await StopScanner();
                    ScanTimeout?.Invoke(this, EventArgs.Empty);
                }
            }
            catch (TaskCanceledException e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task StopScanner()
        {
            _centralManager.StopScan();
            IsScanning = false;
            return Task.FromResult(0);
        }
    }
}