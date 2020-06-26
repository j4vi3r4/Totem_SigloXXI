using System;
using Totem_SigloXXI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Totem_SigloXXI
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new BienvenidosPage());

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
