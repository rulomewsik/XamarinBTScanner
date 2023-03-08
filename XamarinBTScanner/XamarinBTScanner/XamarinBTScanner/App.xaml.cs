using System;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinBTScanner.Contracts.Services;
using XamarinBTScanner.Services;
using XamarinBTScanner.ViewModels;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XamarinBTScanner
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; set; }
        
        public App()
        {
            InitializeComponent();
            SetupServices();
            
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private static void SetupServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<MainViewModel>();
            services.AddSingleton<IDialogService, DialogService>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}