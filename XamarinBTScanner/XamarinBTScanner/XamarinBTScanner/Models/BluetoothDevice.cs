#nullable enable
using System;

namespace XamarinBTScanner.Models
{
    public class BluetoothDevice
    {
        public string? Name { get; set; }
        public Guid Identifier { get; set; }
        public int? Rssi { get; set; }
    }
}