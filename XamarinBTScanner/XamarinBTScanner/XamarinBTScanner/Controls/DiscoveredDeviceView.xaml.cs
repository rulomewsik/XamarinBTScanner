using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinBTScanner.Models;

namespace XamarinBTScanner.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiscoveredDeviceView : ContentView
    {
        #region Properties

        /// <summary>
        /// Gets or sets the Bluetooth Device.
        /// </summary>
        public static readonly BindableProperty DeviceProperty = BindableProperty.Create(
            nameof(Device),
            typeof(BluetoothDevice),
            typeof(DiscoveredDeviceView),
            null,
            BindingMode.Default
        );

        public BluetoothDevice Device
        {
            get => (BluetoothDevice) GetValue(DeviceProperty);
            set => SetValue(DeviceProperty, value);
        }

        #endregion

        public DiscoveredDeviceView()
        {
            InitializeComponent();
        }
        
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}