using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using MvvmHelpers.Commands;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        private readonly IShopService _shopService;
        private readonly IPageService _pageService;
        
        public AsyncCommand LoginCommand { get; }

        private string _email = "maya8624@hotmail.com";
        private string _password = "Fk2g20.1";

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginPageModel()
        {
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
            LoginCommand = new AsyncCommand(Login);
        }

        private async Task Login()
        {
            try
            {
                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    await _pageService.DisplayAlert("Info", "Please enter your credentials", "OK");
                    return;
                }

                var shop = await App.Database.GetShopAsync();
                if (shop == null)
                {
                    await _pageService.DisplayAlert("Info", "Please register your shop details to log in", "OK");
                    return;
                }
                
                if (!shop.Email.Equals(Email))
                {
                    await _pageService.DisplayAlert("Info", "Email doesn't match", "OK");
                    return;
                }
                
                var savedPassword = await SecureStorage.GetAsync("password");
                if (string.IsNullOrEmpty(savedPassword) || !savedPassword.Equals(Password))
                {
                    //await SecureStorage.SetAsync("password", "Fk2g20.1");
                    await _pageService.DisplayAlert("Info", "Password is in valid", "OK");
                    return;
                }
                await Shell.Current.GoToAsync($"//{nameof(OrderPage)}");
                //await Navigation.NavigateToAsync($"{nameof(LiveViewModel)}?name={Name}");

            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
