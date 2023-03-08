using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.OS;
using XamarinBTScanner.Contracts.Adapters;
using XamarinBTScanner.Droid.Services.Callbacks;
using BluetoothDevice = XamarinBTScanner.Models.BluetoothDevice;
using ScanMode = Android.Bluetooth.LE.ScanMode;

namespace XamarinBTScanner.Droid.Adapters
{
    public class BluetoothAdapter : IBluetoothAdapter
    {
        private CancellationTokenSource _scanCancellationTokenSource;
        private readonly Android.Bluetooth.BluetoothAdapter _bluetoothAdapter;
        private readonly LegacyScannerCallback _legacyScannerCallback;
        private readonly ScannerCallback _scannerCallback;

        public bool IsScanning { get; private set; }
        public ObservableCollection<BluetoothDevice> DiscoveredDevices { get; set; }


        public event EventHandler<BluetoothDevice> DiscoveredDevice;
        public event EventHandler ScanTimeout;
        public event EventHandler<string> ScanFailed;

        public BluetoothAdapter(BluetoothManager bluetoothManager)
        {
            DiscoveredDevices = new ObservableCollection<BluetoothDevice>();
            _bluetoothAdapter = bluetoothManager.Adapter;

            if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
            {
                _scannerCallback = new ScannerCallback(this);
            }
            else
            {
                _legacyScannerCallback = new LegacyScannerCallback(this);
            }
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
                    await StartNativeScanner(_scanCancellationTokenSource.Token);
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

        private Task StartNativeScanner(CancellationToken scanCancellationToken)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                StartScanningLegacy();
            }
            else
            {
                StartScanning();
            }

            return Task.FromResult(true);
        }

        private void StartScanningLegacy()
        {
            _bluetoothAdapter.StartLeScan(null, _legacyScannerCallback);
        }

        private void StartScanning()
        {
            var ssb = new ScanSettings.Builder();
            ssb.SetScanMode(ScanMode.LowPower);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                ssb.SetLegacy(false);
            }

            _bluetoothAdapter.BluetoothLeScanner?.StartScan(null, ssb.Build(), _scannerCallback);
        }

        public void HandleFailedScan(ScanFailure scanFailure)
        {
            ScanFailed?.Invoke(this, scanFailure.ToString());
        }
        
        public void HandleDiscoveredDevice(BluetoothDevice device)
        {
            if (DiscoveredDevices.Any(d => d.Identifier == device.Identifier))
            {
                var repeatedDevice = DiscoveredDevices.FirstOrDefault(d => d.Identifier == device.Identifier);
                DiscoveredDevices[DiscoveredDevices.IndexOf(repeatedDevice)] = device;
            }
            else
            {
                DiscoveredDevices.Add(device);
            }

            DiscoveredDevices =
                new ObservableCollection<BluetoothDevice>(DiscoveredDevices.OrderByDescending(d => d.Rssi));
            DiscoveredDevice?.Invoke(this, device);
        }

        public Task StopScanner()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop)
            {
                _bluetoothAdapter.StopLeScan(_legacyScannerCallback);
            }
            else
            {
                _bluetoothAdapter.BluetoothLeScanner?.StopScan(_scannerCallback);
            }

            if (_scanCancellationTokenSource != null)
            {
                _scanCancellationTokenSource.Dispose();
                _scanCancellationTokenSource = null;
            }

            IsScanning = false;

            return Task.FromResult(0);
        }
    }
}