using ep.Mobile.Extensions;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using ep.Mobile.Reference;
using ep.Mobile.Validations;
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
        private readonly LoginValidation _loginValidation;
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;
      
        private string _email;
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

                var errorMessage = "Invalid email or password";
                var storedEmail = await SecureStorage.GetAsync(Constant.StorageEmailKey);
                if (storedEmail is null || !storedEmail.Equals(Email))
                {
                    ValidateMessage = errorMessage;
                    return;
                }
                
                var storedPassword = await SecureStorage.GetAsync(Constant.StoragePasswordKey);
                if (storedPassword is null || !storedPassword.Equals(Password))
                {
                    ValidateMessage = errorMessage;
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
