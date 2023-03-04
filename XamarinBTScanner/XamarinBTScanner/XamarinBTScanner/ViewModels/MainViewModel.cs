using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        private BluetoothDevice _discoveredDevice;
        public BluetoothDevice DiscoveredDevice
        {
            private set
            {
                if (_discoveredDevice == value) return;
                _discoveredDevice = value;
                OnPropertyChanged();
            }

            get => _discoveredDevice;
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
            DiscoveredDevices = new ObservableCollection<BluetoothDevice>();
            _bluetoothService.DiscoveredDevice += BluetoothServiceOnDiscoveredDevice;
        }

        private async void StartScanner()
        {
            var status = await _bluetoothPermissionService.RequestPermission();
            switch (status)
            {
                case PermissionStatus.Unknown:
                    break;
                case PermissionStatus.Denied:
                    break;
                case PermissionStatus.Disabled:
                    break;
                case PermissionStatus.Granted:
                    await _bluetoothService.StartScanner(60000, "");
                    break;
                case PermissionStatus.Restricted:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void BluetoothServiceOnDiscoveredDevice(object sender, BluetoothDevice e)
        {
            DiscoveredDevice = e;
            DiscoveredDevices.Add(e);
        }
    }
}