using System;
using System.Linq;
using Android.Bluetooth;
using Object = Java.Lang.Object;

namespace XamarinBTScanner.Droid.Services.Callbacks
{
    public class LegacyScannerCallback : Object, BluetoothAdapter.ILeScanCallback
    {
        private readonly XamarinBTScanner.Droid.Adapters.BluetoothAdapter _bluetoothAdapter;

        public LegacyScannerCallback(XamarinBTScanner.Droid.Adapters.BluetoothAdapter bluetoothAdapter)
        {
            _bluetoothAdapter = bluetoothAdapter;
        }

        public void OnLeScan(BluetoothDevice bleDevice, int rssi, byte[] scanRecord)
        {
            var device = new XamarinBTScanner.Models.BluetoothDevice
            {
                Name = string.IsNullOrEmpty(bleDevice.Name) ? "N/A" : bleDevice.Name,
                Identifier = ParseDeviceId(bleDevice.Address),
                Address = bleDevice.Address,
                Rssi = rssi
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