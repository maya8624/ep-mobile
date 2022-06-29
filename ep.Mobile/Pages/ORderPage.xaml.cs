using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Interfaces;
using ep.Mobile.Models;
using ep.Mobile.PageModels;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]  
    public partial class OrderPage : ContentPage
    { 
        public OrderPage()
        {
            InitializeComponent();
        }        
      
        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await Navigation.ShowPopupAsync(new QRPopupPage());
            await DisplayAlert("Result", $"Result:{result}", "OK");
        }
    }
}