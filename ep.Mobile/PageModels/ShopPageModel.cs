using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Utils;
using ep.Mobile.Views;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class ShopPageModel : PageModelBase
    {
        private string _abn;
        private string _address;
        private string _confirmPassword;
        private string _email;
        private string _name;
        private string _owner;
        private string _password;
        private string _telephone;

        public string ABN
        {
            get => _abn;
            set => SetProperty(ref _abn, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public string Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string Telephone
        {
            get => _telephone;
            set => SetProperty(ref _telephone, value);
        }
             
        public ICommand GetPlacesCommand { get; set; }
        public AsyncCommand SaveCommand { get; }
        private readonly IShopService _shopService;
        
        private readonly HttpClient _client;
        private const string _baseUrl = "https://maps.googleapis.com/maps/api/place/autocomplete/json?";
        public ObservableCollection<string> Places = new ObservableCollection<string>();
        public Shop Shop { get; set; }

        public ShopPageModel()
        {            
            _shopService = DependencyService.Get<IShopService>();
            SaveCommand = new AsyncCommand(OnSave);
            //GetPlacesCommand = new Xamarin.Forms.Command(async() => await GetPlacesByName());
            _client = new HttpClient();
            //Task.Run(async () => await GetShop());
        }
        
        private async Task GetPlacesByName()
        {
            try
            {
                string text = _address;
               var endPoint = $"{_baseUrl}input={Uri.EscapeUriString(text)}&types=establishment&components=country:au&key=AIzaSyBgYSuNmeLsZQnYv4d1cOlDpuURxRVndNE";

                var response = await _client.GetStringAsync(endPoint);
            
                var items = JsonConvert.DeserializeObject<Root>(response);
                if (items.predictions.Any())
                {
                    foreach (var item in items.predictions)
                    {
                        Places.Add(item.description);
                    }
                }
            }
            catch (Exception ex)
            {
                //await DisplayAlert("Error", ex.Message, "OK");
                throw;
            }
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(_abn)
                && !string.IsNullOrWhiteSpace(_address);
        }

        private async Task GetShop()
        {
            try
            {
                var shop = await App.Database.GetShopAsync();
                if (shop != null)
                {
                    ABN = shop.ABN;
                    Address = shop.Address;
                    Email = shop.Email;
                    Name = shop.Name;
                    Owner = shop.Owner;
                    Telephone = shop.Telephone;
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "can't get the shop details", "Cancel");
                throw;
            }
        }

        private async Task OnSave()
        {
            try
            {
                var local = await App.Database.GetShopAsync();
                // TODO: Validate entries
                var shop = new Shop
                {
                    ABN = ABN,
                    Address = Address,
                    CreatedOn = DateTimeOffset.UtcNow,
                    Email = Email,
                    Name = Name,
                    Owner = Owner,
                    Telephone = Telephone,
                };

                if (local == null)
                {
                    await _shopService.CreateShopAsync(shop);                    
                    //var pass = CreateTempPassword();
                }
                //TODO: update shop in azure db
                //else 
                //{
                //shop = local
                //    shop.Id = local.Id;
                //    shop.UpdatedOn = DateTimeOffset.UtcNow;
                //    await App.Database.UpdateShopAsync(shop);
                //}
                
                // This will pop the current page off the navigation stack
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", "can't register", "Cancel");
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
    }

    public class MatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class MainTextMatchedSubstring
    {
        public int length { get; set; }
        public int offset { get; set; }
    }

    public class StructuredFormatting
    {
        public string main_text { get; set; }
        public List<MainTextMatchedSubstring> main_text_matched_substrings { get; set; }
        public string secondary_text { get; set; }
    }

    public class Term
    {
        public int offset { get; set; }
        public string value { get; set; }
    }

    public class Prediction
    {
        public string description { get; set; }
        public List<MatchedSubstring> matched_substrings { get; set; }
        public string place_id { get; set; }
        public string reference { get; set; }
        public StructuredFormatting structured_formatting { get; set; }
        public List<Term> terms { get; set; }
        public List<string> types { get; set; }
    }

    public class Root
    {
        public List<Prediction> predictions { get; set; }
        public string status { get; set; }
    }
}
