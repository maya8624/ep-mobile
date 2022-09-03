using ep.Mobile.Interfaces.IServices;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using ep.Mobile.Reference;
using ep.Mobile.Validations;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class LoginPageModel : PageModelBase
    {
        public AsyncCommand LoginCommand { get; }
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;

        //private ValidatableObject<string> _email;
        //public ValidatableObject<string> Email
        //{
        //    get => _email;
        //    set => SetProperty(ref _email, value);
        //}

        //private ValidatableObject<string> _password;
        //public ValidatableObject<string> Password
        //{
        //    get => _password;
        //    set => SetProperty(ref _password, value);
        //}

        //private ValidatableObject<string> _username;

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

        public ICommand ValidateCommand { get; private set; }

        public LoginPageModel()
        {
            LoginCommand = new AsyncCommand(LoginAsync);
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();

            //_email = new ValidatableObject<string>();
            //_password = new ValidatableObject<string>();
            //_username = new ValidatableObject<string>();

            //ValidateCommand = new AsyncCommand(Validate); 


            //AddValidations();
        }

        //public ValidatableObject<string> Username
        //{
        //    get => _username;
        //    set
        //    {
        //        _username = value;
        //        RaisePropertyChanged(() => Username);
        //    }
        //}

        //private async Task<bool> Validate()
        //{
        //    await Task.CompletedTask;
        //    bool isValidEmail = ValidateEmail();
        //    //bool isValidUser = ValidateUserName();
        //    bool isValidPassword = ValidatePassword();
        //    //return isValidUser && isValidPassword && isValidEmail;
        //    return isValidPassword && isValidEmail;
        //}
        //private bool ValidateEmail()
        //{
        //    return _email.Validate();
        //}

        //private bool ValidateUserName()
        //{
        //    return _username.Validate();
        //}

        //private bool ValidatePassword()
        //{
        //    return _password.Validate();
        //}

        //private void AddValidations()
        //{
        //    _email.Validations.Add(new IsNotNullOrEmptyRule<string> 
        //    { 
        //        ValidationMessage = "A email is required." 
        //    });
        //    _email.Validations.Add(new EmailRule<string>
        //    {
        //        ValidationMessage = "Invalid email address."
        //    });
        //    _password.Validations.Add(new IsNotNullOrEmptyRule<string> 
        //    { 
        //        ValidationMessage = "A password is required." 
        //    });
        //    //_username.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "A username is required." });
        //}

        public override async Task InitializeAsync(object parameter)
        {
            await GetShopAsync();
            //await base.InitializeAsync(parameter);
        }

        private async Task GetShopAsync()
        {
            var shop = await _shopService.GetShopAsync();
            if (shop == null)
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
                await GetShopAsync();
                //bool isValid = await Validate();
                //if (!isValid)
                //{
                //    await _pageService.DisplayAlert("Info", "Required", "OK");
                //    return;
                //}
                //if (string.IsNullOrEmpty(Email))
                //{
                //    await _pageService.DisplayAlert("Info", "Please enter your email", "OK");
                //    return;
                //}
                //if (string.IsNullOrEmpty(Password))
                //{
                //    await _pageService.DisplayAlert("Info", "Please enter your password", "OK");
                //    return;
                //}
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
