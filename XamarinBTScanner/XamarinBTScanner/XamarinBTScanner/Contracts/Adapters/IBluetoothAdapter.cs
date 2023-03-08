using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.Contracts.Adapters
{
    public interface IBluetoothAdapter
    {
        /// <summary>
        /// Indicates if the adapter is currently scanning for devices.
        /// </summary>
        bool IsScanning { get; }
        
        /// <summary>
        /// List of discovered devices.
        /// </summary>
        ObservableCollection<BluetoothDevice> DiscoveredDevices { get; }
        
        /// <summary>
        /// When the adapter receives a new Bluetooth Device.
        /// </summary>
        event EventHandler<BluetoothDevice> DiscoveredDevice;
        
        /// <summary>
        /// Occurs when the scan has been stopped after a timeout.
        /// </summary>
        event EventHandler ScanTimeout;
        
        /// <summary>
        /// Occurs when the scan failed to start.
        /// </summary>
        event EventHandler<string> ScanFailed;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scanTimeout"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task StartScanner(int scanTimeout, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// 
        /// </summary>
        Task StopScanner();
    }
}