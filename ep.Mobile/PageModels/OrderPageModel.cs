using ep.Mobile.Enums;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using ep.Mobile.PageModels.Base;
using ep.Mobile.Reference;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.PageModels
{
    public class OrderPageModel : PageModelBase, IQueryAttributable
    {
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

        private string _shopName;
        public string ShopName
        {
            get => _shopName;
            set => SetProperty(ref _shopName, value);
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

        private readonly ICustomerService _customerService;
        private readonly HubConnection _hubConnection;
        private bool _connected;

        public AsyncCommand<SmsParam> CloseComand { get; set; }
        public AsyncCommand<SmsParam> SMSCommand { get; private set; }
        public AsyncCommand<MessageStatus> SummaryCommand { get; set; }
        public ObservableCollection<OrderItem> OrderItems { get; set; }

        public ICommand LoadCommand { get; protected set; }

        public OrderPageModel()
        {
            _currentDate = DateTime.Now.ToString("MMM dd, yyy");
            _customerService = DependencyService.Get<ICustomerService>();
            CloseComand = new AsyncCommand<SmsParam>(CloseOrderItem);
            SummaryCommand = new AsyncCommand<MessageStatus>(GetOrdersByMessageStatus);
            SMSCommand = new AsyncCommand<SmsParam>(SendMessageAsync);
            OrderItems = new ObservableCollection<OrderItem>(
                 Task.Run(async () => await _customerService.GetCustomersAsync()).Result
            );
            Task.Run(async () => await GetSummary());

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"{Constant.ApiBaseUrl}/hub/customer")
                .ConfigureLogging(logging => 
                { 
                    logging.AddFilter("SignalR", LogLevel.Debug); 
                })
                .Build();

            HubOn();
        }

        private void HubOn()
        {
            var shop = OrderItems.FirstOrDefault();
            if (shop == null)
            {
                return;
            }
            _hubConnection.On<string>($"NewCustomerShop{shop.ShopId}", (item) =>
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
                                MessageStatus = customer.LatestedMessageStatus,
                                MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                                Mobile = customer.Mobile,
                                Name = customer.Name,
                                OrderNo = customer.OrderNo,
                                //ShowCloseButton = false,
                                //ShowSMSButton = true
                                SMSParam = new SmsParam
                                {
                                    CustomerId = customer.Id,
                                    ShopId = customer.ShopId,
                                    MessageStatus = customer.LatestedMessageStatus
                                }
                            };

                            OrderItems.Insert(0, newItem);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            });
            _hubConnection.StartAsync();
        }

        public async Task ExecuteLoadItemsCommand()
        {
            //if (IsBusy)
            //    return;
            //IsBusy = true;
            try
            {
                if (!_connected)
                    await _hubConnection.StartAsync();

                _connected = true;
                await App.Current.MainPage.DisplayAlert("", "Connected", "Cancel");
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //_connected = false;
            }
        }
                
        private async Task GetSummary()
        {
            try
            {
                var shop = await App.Database.GetShopAsync();
                _shopName = shop.Name;

                var messages = await App.Database.GetMessagesAsync();
                _completed = messages.Count(m => m.Status == MessageStatus.Completed);
                _prep = OrderItems.Count(o => o.MessageStatus == MessageStatus.Prep);
                _sent = messages.Count(m => m.Status == MessageStatus.Sent);
                _resent = messages.Count(m => m.Status == MessageStatus.Resent);
                _total = _completed + _prep + _sent + _resent;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
                throw;
            }
        }      

        private async Task SendMessageAsync(SmsParam param)
        {
            try
            {
                var currentItem = OrderItems.FirstOrDefault(o => o.CustomerId == param.CustomerId);
                var updatedItem = await _customerService.SendSMSAsync(param);
                OrderItems.Remove(currentItem);
                OrderItems.Add(updatedItem);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
                throw;
            }
        }

        private async Task GetOrdersByMessageStatus(MessageStatus status)
        {
            try
            {
                OrderItems.Clear();
                var items = status == MessageStatus.Other ?
                    OrderItems :
                    OrderItems.Where(o => o.MessageStatus == status);
                //var items = await _customerService.GetCustomerOrdersByMessageStatus(status);
                foreach (var item in items)
                {
                    OrderItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
                throw;
            }
        }

        private async Task CloseOrderItem(SmsParam param)
        {
            try
            {
                param.MessageStatus = MessageStatus.Completed;
                await SendMessageAsync(param);
                var item = OrderItems.FirstOrDefault(o => o.CustomerId == param.CustomerId);
                OrderItems.Remove(item);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message, "Close");
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
      
        public async Task OnAppearing()
        {
            await _hubConnection.StartAsync();
        }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if (!query.Any())
            {
                return;
            }
            string orderNo = HttpUtility.UrlDecode(query["orderNo"]);            
            Task.Run(async () => await LoadCustomer(orderNo));
        }

        private async Task LoadCustomer(string orderNo)
        {
            try
            {
                var customer = await _customerService.GetCustomerByOrderNoAsync(orderNo);
                var newItem = new OrderItem
                {
                    CreatedOn = customer.CreatedOn,
                    CustomerId = customer.Id,
                    MessageStatus = customer.LatestedMessageStatus,
                    MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                    Mobile = customer.Mobile,
                    Name = customer.Name,
                    OrderNo = customer.OrderNo,
                    SMSParam = new SmsParam
                    {
                        CustomerId = customer.Id,
                        ShopId = customer.ShopId,
                        MessageStatus = customer.LatestedMessageStatus
                    }
                };
                OrderItems.Insert(0, newItem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
