using System;
using System.Threading;
using CoreBluetooth;
using CoreFoundation;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Adapters;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.iOS.Adapters;
using XamarinBTScanner.iOS.Contracts.Services.Delegates;
using XamarinBTScanner.iOS.Services;
using XamarinBTScanner.iOS.Services.Delegates;

[assembly: Dependency(typeof(BluetoothService))]

namespace XamarinBTScanner.iOS.Services
{
    public class BluetoothService : IBluetoothService
    {
        private readonly CBCentralManager _centralManager;
        private readonly IBleCentralManagerDelegate _bleCentralManagerDelegate;
        private readonly Lazy<IBluetoothAdapter> _bluetoothAdapter;

        public BluetoothService()
        {
            var cmDelegate = new BleCentralManagerDelegate();
            _bleCentralManagerDelegate = cmDelegate;
            _centralManager = new CBCentralManager(cmDelegate, DispatchQueue.CurrentQueue);

            _bluetoothAdapter =
                new Lazy<IBluetoothAdapter>(GetNativeAdapter, LazyThreadSafetyMode.PublicationOnly);
        }

        public IBluetoothAdapter Adapter => _bluetoothAdapter.Value;

        private IBluetoothAdapter GetNativeAdapter() =>
            new BluetoothAdapter(_centralManager, _bleCentralManagerDelegate);
    }
}