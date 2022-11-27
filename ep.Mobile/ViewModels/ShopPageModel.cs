using ep.Mobile.Crypto;
using ep.Mobile.Extensions;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.ViewModels.Base;
using ep.Mobile.Views;
using ep.Mobile.Reference;
using ep.Mobile.Utils;
using ep.Mobile.Validations;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.ViewModels
{
    public class ShopPageModel : ViewModelBase
    {
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;
        private readonly ShopValidation _validation;

        public AsyncCommand SaveCommand { get; }

        private string _abn;
        public string ABN
        {
            get => _abn;
            set => SetProperty(ref _abn, value);
        }

        private string _address;
        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _businessName;
        public string BusinessName
        {
            get => _businessName;
            set => SetProperty(ref _businessName, value);
        }

        private string _owner;
        public string Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
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

        private string _phone;
        public string Phone
        {
            get => _phone;
            set => SetProperty(ref _phone, value);
        }

        private string _validateMessage;
        public string ValidateMessage
        {
            get => _validateMessage;
            set => SetProperty(ref _validateMessage, value);
        }

        private bool _isEditMode;
        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public ShopPageModel()
        {
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
            SaveCommand = new AsyncCommand(SaveAsync);
            _validation = new ShopValidation();
        }
       
        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async Task GetShopAsync()
        {
            try
            {
                var shop = await _shopService.GetShopAsync();
                if (shop != null)
                {
                    ABN = shop.ABN;
                    Address = shop.Address;
                    Email = shop.Email;
                    BusinessName = shop.BusinessName;
                    Owner = shop.Owner;
                    Phone = shop.Phone;
                }                
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"can't get the business details: {ex.Message}", "Cancel");
                throw;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                var validation = await _validation.ValidateAsync(this);
                if (validation.IsValid is false)
                {
                    ValidateMessage = validation.GetErrorMesages();
                    return;
                }

                await SaveCredentialsAsync();
                var shop = new Shop
                {
                    ABN = ABN,
                    Address = Address,
                    Email = Email,
                    BusinessName = BusinessName,
                    Owner = Owner,
                    Phone = Phone,
                };

                var local = await _shopService.GetShopAsync();
                if (local == null)
                {
                    await _shopService.CreateShopAsync(shop);
                }
                else
                {
                    shop.Id = local.Id;
                    shop.UpdatedOn = DateTime.Now;
                    await _shopService.UpdateShopAsync(shop);
                }
                await _pageService.DisplayAlert("Info", "Business information saved", "OK");
                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                //TODO: change message
                await _pageService.DisplayAlert("Error", $"Message:{ex.Message}", "OK");
                throw;
            }
        }

        private async Task SaveCredentialsAsync()
        {
            try
            {
                var salt = CryptoService.GetSalt();
                await SecureStorage.SetAsync(Constant.StorageSaltKey, salt);
                var hasedPassword = CryptoService.GetHash(Password, salt);
                await SecureStorage.SetAsync(Constant.StoragePasswordKey, hasedPassword);
                await SecureStorage.SetAsync(Constant.StorageEmailKey, Email);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<string> CreateTempPassword()
        {
            var randomPassword = PasswordGenerator.GeneratePassword();
            await SecureStorage.SetAsync("password", randomPassword);
            //TODO: send a temp password must be hashed with salt
            return await SecureStorage.GetAsync("password");
        }

        public override async Task InitializeAsync(object parameter)
        {
            await GetShopAsync();
        }
    }
}
