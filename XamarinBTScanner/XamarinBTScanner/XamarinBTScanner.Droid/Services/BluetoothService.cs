using System;
using System.Threading;
using Android.Bluetooth;
using Android.Content;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Adapters;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.Droid.Services;
using Application = Android.App.Application;
using BluetoothAdapter = XamarinBTScanner.Droid.Adapters.BluetoothAdapter;

[assembly: Dependency(typeof(BluetoothService))]

namespace XamarinBTScanner.Droid.Services
{
    public class BluetoothService : IBluetoothService
    {
        private readonly Context _context = Application.Context;
        private readonly BluetoothManager _bluetoothManager;
        private readonly Lazy<IBluetoothAdapter> _bluetoothAdapter;

        public BluetoothService()
        {
            _bluetoothManager = _context.GetSystemService(Context.BluetoothService) as BluetoothManager;
            _bluetoothAdapter =
                new Lazy<IBluetoothAdapter>(GetNativeAdapter, LazyThreadSafetyMode.PublicationOnly);
        }

        public IBluetoothAdapter Adapter => _bluetoothAdapter.Value;

        private IBluetoothAdapter GetNativeAdapter() => new BluetoothAdapter(_bluetoothManager);
    }
}