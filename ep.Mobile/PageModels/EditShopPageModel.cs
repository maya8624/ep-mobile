using ep.Mobile.Extensions;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using ep.Mobile.Reference;
using ep.Mobile.Utils;
using ep.Mobile.Validations;
using ep.Mobile.Views;
using FluentValidation;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class EditShopPageModel : PageModelBase
    {
        private readonly IPageService _pageService;
        private readonly IShopService _shopService;
        private readonly EditShopValidation _validation;

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

        public EditShopPageModel()
        {
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
            SaveCommand = new AsyncCommand(SaveAsync);
            _validation = new EditShopValidation();
            Task.Run(async () => await GetShopAsync());
        }

        private async void OnCancel()
        {
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
                else
                {
                    await _pageService.DisplayAlert("Info", $"Business Information is not found", "OK");
                }
            }
            catch (Exception)
            {
                await _pageService.DisplayAlert("Error", "can't get the business details", "Cancel");
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
                    await _pageService.DisplayAlert("Info", $"Business Information is not found", "OK");
                }
                else
                {
                    shop.Id = local.Id;
                    shop.UpdatedOn = DateTime.Now;
                    await _shopService.UpdateShopAsync(shop);
                }
                await _pageService.DisplayAlert("Info", "Business information saved", "OK");
                await Shell.Current.GoToAsync($"//{nameof(OrderPage)}");
            }
            catch (Exception ex)
            {
                //TODO: change message
                await _pageService.DisplayAlert("Error", $"Message:{ex.Message}", "OK");
                throw;
            }
        }
    }
}
