using System;
using System.Diagnostics;
using System.Globalization;
using Xamarin.Forms;

namespace XamarinBTScanner.Converters
{
    public class RssiToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return "ic_wifi_4.png";

            if (!int.TryParse(value.ToString(), out var rssi)) return "ic_wifi_4.png";
            
            Debug.WriteLine("Value " + rssi);
            return rssi switch
            {
                0 => "ic_wifi_4.png",
                < 0 and >= -40 => "ic_wifi_3.png",
                < -40 and >= -80 => "ic_wifi_2.png",
                < -80 and >= -120 => "ic_wifi_1.png",
                _ => "ic_wifi_1.png"
            };

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}