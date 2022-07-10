using ep.Mobile.PageModels;
using System;
//using Xamarin.CommunityToolkit.Extensions;
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

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    var result = await Navigation.ShowPopupAsync(new QRPopupPage());
        //    await DisplayAlert("Result", $"Result:{result}", "OK");
        //}

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await (BindingContext as OrderPageModel).InitializeAsync(string.Empty);
        }
    }
}