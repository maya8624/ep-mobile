using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Reference;
using ep.Mobile.Utils;
using ep.Mobile.Views;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class ShopPageModel : PageModelBase
    {
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;
        public AsyncCommand SaveCommand { get; }
        //public ICommand GetPlacesCommand { get; set; }
        //public ObservableCollection<string> Places = new ObservableCollection<string>();

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

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
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
       
        public ShopPageModel()
        {
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
            SaveCommand = new AsyncCommand(SaveAsync);
            //GetPlacesCommand = new Xamarin.Forms.Command(async() => await GetPlacesByName());
            Task.Run(async () => await GetShopAsync());
        }
        
        //private async Task GetPlacesByName()
        //{
        //    try
        //    {
        //        string text = _address;
        //       var endPoint = $"{Constant.GoogleApiBaseUrl}input={Uri.EscapeUriString(text)}&types=establishment&components=country:au&key=AIzaSyBgYSuNmeLsZQnYv4d1cOlDpuURxRVndNE";

        //        var response = await _client.GetStringAsync(endPoint);
            
        //        var items = JsonConvert.DeserializeObject<Root>(response);
        //        if (items.predictions.Any())
        //        {
        //            foreach (var item in items.predictions)
        //            {
        //                Places.Add(item.description);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //await DisplayAlert("Error", ex.Message, "OK");
        //        throw;
        //    }
        //}

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
                    Name = shop.Name;
                    Owner = shop.Owner;
                    Phone = shop.Phone;
                }
                else
                {
                    await _pageService.DisplayAlert("Info", $"Shop Information is not found", "OK");
                }
            }
            catch (Exception)
            {
                await _pageService.DisplayAlert("Error", "can't get the shop details", "Cancel");
                throw;
            }
        }

        private async Task SaveAsync()
        {
            try
            {
                if (!await ValidateEntriesAsync()) return;
                await SaveCredentialsAsync();
                var shop = new Shop
                {
                    ABN = ABN,
                    Address = Address,
                    Email = Email,
                    Name = Name,
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
                    shop.UpdatedOn = DateTimeOffset.UtcNow;
                    await _shopService.UpdateShopAsync(shop);
                }
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
                // TODO: Apply Cryptography
                await SecureStorage.SetAsync(Constant.StorageEmailKey, Email);
                await SecureStorage.SetAsync(Constant.StoragePasswordKey, Password);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<bool> ValidateEntriesAsync()
        {
            var result = false;
            if (string.IsNullOrEmpty(ABN))
            {
                await _pageService.DisplayAlert("Info", "Please enter ABN", "OK");
            }
            else if (string.IsNullOrEmpty(Address))
            {
                await _pageService.DisplayAlert("Info", "Please enter address", "OK");
            }
            else if (string.IsNullOrEmpty(Name))
            {
                await _pageService.DisplayAlert("Info", "Please enter name", "OK");
            }
            else if (string.IsNullOrEmpty(Owner))
            {
                await _pageService.DisplayAlert("Info", "Please enter owner", "OK");
            }
            else if (string.IsNullOrEmpty(Phone))
            {
                await _pageService.DisplayAlert("Info", "Please enter phone", "OK");
            }
            else if (string.IsNullOrEmpty(Email))
            {
                await _pageService.DisplayAlert("Info", "Please enter email", "OK");
            }
            else if (string.IsNullOrEmpty(Password))
            {
                await _pageService.DisplayAlert("Info", "Please enter password", "OK");
            }
            else if (string.IsNullOrEmpty(ConfirmPassword))
            {
                await _pageService.DisplayAlert("Info", "Please confirmPassword", "OK");
            }
            else if (!Password.Equals(ConfirmPassword))
            {
                await _pageService.DisplayAlert("Info", "Password doen't match", "OK");
            }
            else
            {
                result = true;
            }
            return result;
        }

        private async Task<string> CreateTempPassword()
        {
            var randomPassword = PasswordGenerator.GeneratePassword();
            await SecureStorage.SetAsync("password", randomPassword);
            //TODO: send a temp password must be hashed with salt
            return await SecureStorage.GetAsync("password");
        }
    }
}
