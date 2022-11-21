using ep.Mobile.Crypto;
using ep.Mobile.Extensions;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.ViewModels.Base;
using ep.Mobile.Views;
using ep.Mobile.Reference;
using ep.Mobile.Validations;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MvvmHelpers.Commands;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.ViewModels
{
    public class LoginPageModel : ViewModelBase
    {
        public AsyncCommand LoginCommand { get; }
        private readonly LoginValidation _loginValidation;
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;

        private string _email = "maya8624@hotmail.com";
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _password;
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private string _validateMessage;
        public string ValidateMessage
        {
            get => _validateMessage;
            set => SetProperty(ref _validateMessage, value);
        }

        public LoginPageModel()
        {
            LoginCommand = new AsyncCommand(LoginAsync);
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
            _loginValidation = new LoginValidation();
        }

        public override async Task InitializeAsync(object parameter)
        {
            await GetShopAsync();
            //await base.InitializeAsync(parameter);
        }

        private async Task GetShopAsync()
        {
            var shop = await _shopService.GetShopAsync();
            if (shop is null)
            {
                await _pageService.DisplayAlert("Info", "Please save your business information to use the app", "OK");
                await Shell.Current.GoToAsync($"//{nameof(ShopPage)}");
                return;
            }
        }

        private async Task LoginAsync()
        {
            try
            {
                var validation = await _loginValidation.ValidateAsync(this);
                if (validation.IsValid is false)
                {
                    ValidateMessage = validation.GetErrorMesages();
                    return;
                }

                var storedEmail = await SecureStorage.GetAsync(Constant.StorageEmailKey);
                if (storedEmail is null || !storedEmail.Equals(Email))
                {
                    ValidateMessage = Constant.InvalidLoginMessage;
                    return;
                }

                var hashedText = await CryptoService.GetHashedText(Password);
                var storedPassword = await SecureStorage.GetAsync(Constant.StoragePasswordKey);
                if (storedPassword is null || !storedPassword.Equals(hashedText))
                {
                    ValidateMessage = Constant.InvalidLoginMessage;
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
