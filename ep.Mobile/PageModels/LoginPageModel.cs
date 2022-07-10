using ep.Mobile.Interfaces.IServices;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using ep.Mobile.Reference;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        public AsyncCommand LoginCommand { get; }
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;
        private string _email = "maya8624@hotmail.com";
        
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password = "1234";
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginPageModel()
        {
            LoginCommand = new AsyncCommand(LoginAsync);
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
        }

        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    await _pageService.DisplayAlert("Info", "Please enter your email", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    await _pageService.DisplayAlert("Info", "Please enter your password", "OK");
                    return;
                }
                var storedEmail = await SecureStorage.GetAsync(Constant.StorageEmailKey);
                if (storedEmail == null)
                {
                    await _pageService.DisplayAlert("Info", "Email is not registered", "OK");
                    return;
                }
                if (!Email.Equals(storedEmail))
                {
                    await _pageService.DisplayAlert("Info", "Email doesn't match", "OK");
                    return;
                }
                var storedPassword = await SecureStorage.GetAsync(Constant.StoragePasswordKey);
                if (storedPassword == null)
                {
                    await _pageService.DisplayAlert("Info", "Password is not registered", "OK");
                    return;
                }
                if (!storedPassword.Equals(Password))
                {
                    await _pageService.DisplayAlert("Info", "Password doen't match", "OK");
                    return;
                }
                //TODO: don't need???
                var shop = await _shopService.GetShopAsync();
                if (shop == null)
                {
                    await _pageService.DisplayAlert("Info", "Please save your shop information.", "OK");
                    await Shell.Current.GoToAsync($"//{nameof(ShopPage)}");
                    return;
                }

                await Shell.Current.GoToAsync($"//{nameof(OrderPage)}");
                //await Navigation.NavigateToAsync($"{nameof(LiveViewModel)}?name={Name}");
            }
            catch (Exception ex)
            {
                //TODO: change the error message
                await _pageService.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
