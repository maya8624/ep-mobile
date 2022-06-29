using System;
using System.Collections.Generic;
using ep.Mobile.Pages;
using ep.Mobile.ViewModels;
using ep.Mobile.Views;
using Xamarin.Forms;

namespace ep.Mobile
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            //Routing.RegisterRoute(nameof(ShopPage), typeof(ShopPage));
            //Routing.RegisterRoute()
        }
        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
        }
    }
}
