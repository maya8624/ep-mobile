using ep.Mobile.Enums;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using Microsoft.AspNetCore.SignalR.Client;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class OrderPageModel : PageModelBase, IQueryAttributable
    {
        private readonly ICustomerService _customerService;
        private readonly IPageService _pageService;
        private bool _connected;
        public AsyncCommand<OrderItem> CompleteCommand { get; private set; }
        public AsyncCommand<OrderItem> DeleteCommand { get; private set; }
        public AsyncCommand RefreshCommand { get; private set; }
        private HubConnection HubConnection { get; set; }
        public ObservableCollection<OrderItem> OrderItems { get; private set; } = new ObservableCollection<OrderItem>();
        public AsyncCommand<OrderItem> SMSCommand { get; private set; }
        public AsyncCommand<MessageStatus> SummaryCommand { get; private set; }

        private int _prep;
        public int Prep
        {
            get => _prep;
            set => SetProperty(ref _prep, value);
        }

        private int _sent;
        public int Sent
        {
            get => _sent;
            set => SetProperty(ref _sent, value);
        }

        private int _resent;
        public int Resent
        {
            get => _resent;
            set => SetProperty(ref _resent, value);
        }

        private int _completed;
        public int Completed
        {
            get => _completed;
            set => SetProperty(ref _completed, value);
        }

        private string _currentDate;
        public string CurrentDate
        {
            get => _currentDate;
            set => SetProperty(ref _currentDate, value);
        }

        private int _total;
        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        private string _businessName;
        public string BusinessName
        {
            get => _businessName;
            set => SetProperty(ref _businessName, value);
        }

        private bool _showClose;
        public bool ShowClose
        {
            get => _showClose;
            set => SetProperty(ref _showClose, value);
        }

        private bool _showSMS;
        public bool ShowSMS
        {
            get => _showSMS;
            set => SetProperty(ref _showSMS, value);
        }

        public bool _isRefreshing;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public OrderPageModel()
        {
            _currentDate = DateTime.Now.ToString("MMM dd, yyyy");
            _customerService = DependencyService.Get<ICustomerService>();
            _pageService = DependencyService.Get<IPageService>();
            CompleteCommand = new AsyncCommand<OrderItem>(CompleteAsync);
            DeleteCommand = new AsyncCommand<OrderItem>(DeleteAsync);
            RefreshCommand = new AsyncCommand(RefreshAsync);
            SummaryCommand = new AsyncCommand<MessageStatus>(SummaryAsync);
            SMSCommand = new AsyncCommand<OrderItem>(SendMessageAsync);
        }

        private async Task CompleteAsync(OrderItem orderItem)
        {
            try
            {
                orderItem.MessageStatus = MessageStatus.Completed;
                await _customerService.SendSmsAsync(orderItem, DeviceInfo.Platform);
                OrderItems.Remove(orderItem);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(CompleteAsync)}|message: {ex.Message}", "Close");
                throw;
            }
        }

        private async Task DeleteAsync(OrderItem orderItem)
        {
            try
            {
                var result = await _pageService.DisplayAlert
                (
                    "Info",
                    $"Are you sure you want to delete this order:{orderItem.OrderNo}?",
                    "OK",
                    "Close"
                );
                if (!result) return;
                await _customerService.UpdateCustomerAsync(orderItem.CustomerId);
                OrderItems.Remove(orderItem);
                await SummaryAsync(MessageStatus.Other);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(DeleteAsync)}|message: {ex.Message}", "Close");
                throw;
            }
        }

        private async Task GetOrderItemsAsync()
        {
            try
            {
                if (OrderItems.Any()) return;
                var orderItems = await _customerService.GetOrderItemsAsync();
                //TODO: check Load method
                OrderItems.Clear();
                foreach (var orderItem in orderItems)
                {
                    OrderItems.Add(orderItem);
                }
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(GetOrderItemsAsync)}|message: {ex.Message}", "Ok");
                throw;
            }
        }

        private async Task GetOrderSummaryAsync()
        {
            try
            {
                var summary = await _customerService.GetOrderSummaryAsync();
                Completed = summary.Completed;
                Prep = summary.Prep;
                Resent = summary.Resent;
                Sent = summary.Sent;
                BusinessName = summary.BusinessName;
                Total = summary.Total;
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(GetOrderSummaryAsync)}|message: {ex.Message}", "Close");
                throw;
            }
        }

        public override async Task InitializeAsync(object parameter)
        {
            await InitLoadAsync();

            //HubConnection = new HubConnectionBuilder()
            //    .WithUrl($"{Constant.ApiBaseUrl}/hub/customer")
            //    .ConfigureLogging(logging =>
            //    {
            //        logging.AddFilter("SignalR", LogLevel.Debug);
            //    })
            //    .Build();
            //HubOn();
        }

        private async Task InitLoadAsync()
        {
            await GetOrderItemsAsync();
            await GetOrderSummaryAsync();
        }

        private async Task RefreshAsync()
        {
            IsRefreshing = true;
            await InitLoadAsync();
            IsRefreshing = false;
        }

        private async Task SendMessageAsync(OrderItem orderItem)
        {
            try
            {
                var result = await _pageService.DisplayAlert
                (
                    "Info", 
                    $"Are you sure you want to send a message to {orderItem.Name}?", 
                    "OK", 
                    "Close"
                );
                if (!result) return;
                var updatedItem = await _customerService.SendSmsAsync(orderItem, DeviceInfo.Platform);
                await _pageService.DisplayAlert("Success", $"Message sent to {orderItem.Name}", "Close");
                OrderItems.Remove(orderItem);
                OrderItems.Add(updatedItem);
                await SummaryAsync(MessageStatus.Other);
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(SendMessageAsync)}|message: {ex.Message}", "Close");
                throw;
            }
        }

        private async Task SummaryAsync(MessageStatus status)
        {
            try
            {
                OrderItems.Clear();
                var orderItems = await _customerService.GetOrderItemsByMessageStatus(status);
                foreach (var orderItem in orderItems)
                {
                    OrderItems.Add(orderItem);
                }
                await GetOrderSummaryAsync();
            }
            catch (Exception ex)
            {
                await _pageService.DisplayAlert("Error", $"{nameof(SummaryAsync)}|message: {ex.Message}", "Close");
                throw;
            }
        }
        
        public void SelectedItemList(OrderItem item)
        {
            if (item == null)
            {
                return;
            }
        }
      
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!query.Any())
            {
                return;
            }
            string orderNo = HttpUtility.UrlDecode(query["orderNo"]);
            Task.Run(async () => await LoadCustomerAsync(orderNo));
        }

        private async Task LoadCustomerAsync(string orderNo)
        {
            try
            {
                var newOrderItem = await _customerService.GetCustomerByOrderNoAsync(orderNo);
                var orderItem = OrderItems.FirstOrDefault(x => x.CustomerId == newOrderItem.CustomerId);
                if (orderItem != null) OrderItems.Remove(orderItem);
                OrderItems.Insert(0, newOrderItem);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void HubOn()
        {
            var shop = OrderItems.FirstOrDefault();
            if (shop == null)
            {
                return;
            }
            HubConnection.On<string>($"NewCustomerShop{shop.ShopId}", (item) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    try
                    {
                        var newCustomer = JsonConvert.DeserializeObject<Customer>(item);
                        if (!OrderItems.Any(c => c.OrderNo == newCustomer.OrderNo))
                        {
                            App.Database.SaveCustomerAsync(newCustomer).GetAwaiter();
                            var customer = Task.Run(async () => await App.Database.GetCustomerByOrderNoAsync(newCustomer.OrderNo)).Result;
                            var newItem = new OrderItem
                            {
                                CreatedOn = customer.CreatedOn,
                                CustomerId = customer.Id,
                                MessageStatus = customer.MessageStatus,
                                MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                                Mobile = customer.Mobile,
                                Name = customer.Name,
                                OrderNo = customer.OrderNo,
                            };

                            OrderItems.Insert(0, newItem);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            });
            HubConnection.StartAsync();
        }

        //public async Task ExecuteLoadItemsCommand()
        //{
        //    //if (IsBusy)
        //    //    return;
        //    //IsBusy = true;
        //    try
        //    {
        //        if (!_connected)
        //            await _hubConnection.StartAsync();

        //        _connected = true;
        //        await App.Current.MainPage.DisplayAlert("", "Connected", "Cancel");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        //_connected = false;
        //    }
        //}
    }
}
