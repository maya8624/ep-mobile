using System;
using System.Collections.Generic;
using ep.Mobile.Views;
using ep.Mobile.ViewModels;
using Xamarin.Forms;

namespace ep.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();           
            //Routing.RegisterRoute(nameof(ShopPage), typeof(ShopPage));
            //Routing.RegisterRoute()
        }
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
        }
    }
}
