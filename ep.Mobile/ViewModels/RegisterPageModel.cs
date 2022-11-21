using ep.Mobile.Interfaces.IServices;
using ep.Mobile.ViewModels.Base;
using ep.Mobile.Views;
using ep.Mobile.Reference;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.ViewModels
{
    public class RegisterPageModel : ViewModelBase
    {
        private readonly IPageService _pageService;
        public AsyncCommand RegisterCommand { get; }

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

        private string _confirmPassword;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public RegisterPageModel()
        {
            RegisterCommand = new AsyncCommand(RegisterAsync);
            _pageService = DependencyService.Get<IPageService>();
        }

        private async Task RegisterAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Email))
                {
                    await _pageService.DisplayAlert("Info", "Please enter email", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(Password))
                {
                    await _pageService.DisplayAlert("Info", "Please enter password", "OK");
                    return;
                }
                if (string.IsNullOrEmpty(ConfirmPassword))
                {
                    await _pageService.DisplayAlert("Info", "Please enter confirm password", "OK");
                    return;
                }
                if (!Password.Equals(ConfirmPassword))
                {
                    await _pageService.DisplayAlert("Info", "Your password and confirm password doesn't match", "OK");
                    return;
                }
                await SecureStorage.SetAsync(Constant.StorageEmailKey, Email);
                await SecureStorage.SetAsync(Constant.StoragePasswordKey, Password);
                await Shell.Current.GoToAsync($"//{nameof(ShopPage)}");
            }
            catch (Exception)
            {
                await _pageService.DisplayAlert("Error", "An error occured while registering", "OK");
                throw;
            }
        }
    }
}
