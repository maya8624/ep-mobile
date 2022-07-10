﻿using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Pages;
using MvvmHelpers.Commands;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class CustomerPageModel : PageModelBase
    {
        private readonly ICustomerService _customerService;
        private readonly IShopService _shopService;
        private readonly IPageService _pageService;
        public AsyncCommand SaveCommand { get; private set; }
             
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _mobile;
        public string Mobile
        {
            get => _mobile;
            set => SetProperty(ref _mobile, value);
        }

        private string _orderNo;
        public string OrderNo
        {
            get => _orderNo;
            set => SetProperty(ref _orderNo, value);
        }

        public CustomerPageModel()
        {
            SaveCommand = new AsyncCommand(OnSave);
            _customerService = DependencyService.Get<ICustomerService>();
            _pageService = DependencyService.Get<IPageService>();
            _shopService = DependencyService.Get<IShopService>();
        }

        private async Task OnSave()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    await _pageService.DisplayAlert("Validation", "Name is required", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(Mobile))
                {
                    await _pageService.DisplayAlert("Validation", "Mobile is required", "Ok");
                    return;
                }
                if (string.IsNullOrEmpty(OrderNo))
                {
                    await _pageService.DisplayAlert("Validation", "Order No is required", "Ok");
                    return;
                }
                var shop = await _shopService.GetShopAsync();
                if (shop == null)
                {
                    await _pageService.DisplayAlert("Validation", "There is no shop details", "Ok");
                    return;
                }
                var customer = new Customer
                {
                    Name = _name,
                    Mobile = _mobile,
                    OrderNo = _orderNo
                };
                await _customerService.SaveCustomer(customer);
                ResetEntries();
                await Shell.Current.GoToAsync($"//{nameof(OrderPage)}?orderNo={customer.OrderNo}");
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{ex.Message}", "Ok");
                throw;
            }
        }

        private void ResetEntries()
        {
            Name = "";
            Mobile = "";
            OrderNo = "";
        }
    }
}
