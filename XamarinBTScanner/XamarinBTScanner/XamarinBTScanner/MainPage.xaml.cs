using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using XamarinBTScanner.ViewModels;

namespace XamarinBTScanner
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = App.ServiceProvider.GetService<MainViewModel>();
            On<iOS>().SetUseSafeArea(true);
        }
    }
}