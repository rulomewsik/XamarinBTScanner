using System;
using System.Linq;
using Android.Bluetooth.LE;
using XamarinBTScanner.Droid.Adapters;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.Droid.Services.Callbacks
{
    public class ScannerCallback : ScanCallback
    {
        private readonly BluetoothAdapter _bluetoothAdapter;

        public ScannerCallback(BluetoothAdapter bluetoothAdapter)
        {
            _bluetoothAdapter = bluetoothAdapter;
        }

        public override void OnScanFailed(ScanFailure errorCode)
        {
            _bluetoothAdapter.HandleFailedScan(errorCode);
        }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            var device = new BluetoothDevice
            {
                Name = string.IsNullOrEmpty(result.Device?.Name) ? "N/A" : result.Device?.Name,
                Identifier = ParseDeviceId(result.Device?.Address),
                Address = result.Device?.Address,
                Rssi = result.Rssi
            };

            _bluetoothAdapter.HandleDiscoveredDevice(device);
        }

        private static Guid ParseDeviceId(string deviceAddress)
        {
            var deviceGuid = new byte[16];
            var macWithoutColons = deviceAddress.Replace(":", "");
            var macBytes = Enumerable.Range(0, macWithoutColons.Length)
                .Where(x => x % 2 == 0)
                .Select(x => Convert.ToByte(macWithoutColons.Substring(x, 2), 16))
                .ToArray();
            macBytes.CopyTo(deviceGuid, 10);
            return new Guid(deviceGuid);
        }
    }
}