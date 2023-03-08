using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region Properties

        private CancellationTokenSource _cancellationTokenSource;
        
        private bool _isScanning;
        public bool IsScanning
        {
            private set
            {
                if (_isScanning == value) return;
                _isScanning = value;
                OnPropertyChanged();
            }

            get => _isScanning;
        }
        
        private ObservableCollection<BluetoothDevice> _discoveredDevices;
        public ObservableCollection<BluetoothDevice> DiscoveredDevices
        {
            private set
            {
                if (_discoveredDevices == value) return;
                _discoveredDevices = value;
                OnPropertyChanged();
            }

            get => _discoveredDevices;
        }
        
        #endregion
        
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        #region Services

        private readonly IBluetoothPermissionService _bluetoothPermissionService;
        private readonly IBluetoothService _bluetoothService;

        #endregion

        #region Commands

        public ICommand StartScannerCommand { get; set; }

        #endregion
        
        public MainViewModel()
        {
            _bluetoothService = DependencyService.Get<IBluetoothService>();
            _bluetoothPermissionService = DependencyService.Get<IBluetoothPermissionService>();

            StartScannerCommand = new Command(StartScanner);
            _bluetoothService.Adapter.DiscoveredDevice += BluetoothServiceOnDiscoveredDevice;
            _bluetoothService.Adapter.ScanFailed += BluetoothServiceOnScanFailed;
        }

        private async void StartScanner()
        {
            if (IsScanning)
            {
                await _bluetoothService.Adapter.StopScanner();
                IsScanning = _bluetoothService.Adapter.IsScanning;
                return;
            }
            
            var status = await _bluetoothPermissionService.CheckAndRequestPermission();
            switch (status)
            {
                case PermissionStatus.Unknown:
                    break;
                case PermissionStatus.Denied:
                    break;
                case PermissionStatus.Disabled:
                    break;
                case PermissionStatus.Granted:
                    _cancellationTokenSource = new CancellationTokenSource();
                    await _bluetoothService.Adapter.StartScanner(6000, _cancellationTokenSource.Token);
                    IsScanning = _bluetoothService.Adapter.IsScanning;
                    break;
                case PermissionStatus.Restricted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void BluetoothServiceOnDiscoveredDevice(object sender, BluetoothDevice e)
        {
            DiscoveredDevices = new ObservableCollection<BluetoothDevice>(_bluetoothService.Adapter.DiscoveredDevices);
            IsScanning = _bluetoothService.Adapter.IsScanning;
        }
        
        private void BluetoothServiceOnScanFailed(object sender, string e)
        {
            throw new NotImplementedException();
        }
    }
}