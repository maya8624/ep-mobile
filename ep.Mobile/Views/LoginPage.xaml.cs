using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Pages;
using ep.Mobile.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ep.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            //this.BindingContext = new LoginViewModel();
        }

        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(id.Text) || string.IsNullOrEmpty(password.Text))
        //        {
        //            await DisplayAlert("Info", "Please enter your credentials", "OK");
        //            return;
        //        }              
        //        var email = await SecureStorage.GetAsync("email");
        //        var psssword = await SecureStorage.GetAsync("password");
        //        if (email == null || password == null)
        //        {
        //            await DisplayAlert("Info", "Please register your shop details to log in", "OK");
        //            return;
        //        }
        //        if (!email.Equals(id.Text.Trim()) || !psssword.Equals(password.Text.Trim()))
        //        {
        //            await DisplayAlert("Credential", "Username or Password is not correct", "OK");
        //            return;
        //        }
        //        await Shell.Current.GoToAsync("//WeatherPage");
        //    }
        //    catch (Exception ex)
        //    {
        //        await DisplayAlert("Error", ex.Message, "OK");
        //    }
        //}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(ShopPage)}");
        }

        private async void TapGestureRecognizer_ResetPassword(object sender, EventArgs e)
        {
            await Shell.Current.Navigation.PushAsync(new ResetPassword());
        }     
    }
}