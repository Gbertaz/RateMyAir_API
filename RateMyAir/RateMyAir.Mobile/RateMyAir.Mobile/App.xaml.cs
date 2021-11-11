using RateMyAir.Mobile.Services;
using RateMyAir.Mobile.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RateMyAir.Mobile
{
    public partial class App : Application
    {

        public App()
        {
            string syncFusionKey = "YOUR SYNCFUSION KEY";

            //Register Syncfusion license
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(syncFusionKey);

            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
