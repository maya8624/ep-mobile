using ep.Mobile.Enums;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IAPIService _apiService;
        private readonly ISmsService _smsService;

        public CustomerService()
        {
            _apiService = DependencyService.Get<IAPIService>();
            _smsService = DependencyService.Get<ISmsService>();
        }

        public async Task DeleteAllRecordsAsync(DateTime dateTime)
        {
            try
            {
                var anyCustomer = await App.Database.AnyCustomer(dateTime);
                if (anyCustomer is false)
                {
                    await App.Database.DeleteAllRecordsAsync<Customer>();
                    await App.Database.DeleteAllRecordsAsync<Message>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                return await App.Database.GetCustomerByIdAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderItem> GetCustomerByOrderNoAsync(string orderNo)
        {
            try
            {
                var customer = await App.Database.GetCustomerByOrderNoAsync(orderNo);
                var orderItem = new OrderItem
                {
                    CreatedOn = customer.CreatedOn,
                    CustomerId = customer.Id,
                    MessageStatus = customer.MessageStatus,
                    MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                    Mobile = customer.Mobile,
                    Name = customer.Name,
                    OrderNo = customer.OrderNo,
                    //ShowCloseButton = x.MessageStatus == MessageStatus.Sent || x.MessageStatus == MessageStatus.Resent,
                    //ShowSMSButton = x.MessageStatus != MessageStatus.Completed
                };
                return orderItem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int> GetLatestOrderNumberAsync()
        {
            try
            {
                var customer = await App.Database.GetLatestCustomerAsync();
                return customer == null ? 0 : Convert.ToInt16(customer.OrderNo);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsAsync()
        {
            try
            {
                var customers = await App.Database.GetCustomersByOrderStatusAsync(OrderStatus.Active);
                return customers.Select(x => new OrderItem
                {
                    CreatedOn = x.CreatedOn,
                    CustomerId = x.Id,
                    MessageStatus = x.MessageStatus,
                    MessageCreatedOn = x.UpdatedOn ?? x.CreatedOn,
                    Mobile = x.Mobile,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    ShowCloseButton = x.MessageStatus == MessageStatus.Sent || x.MessageStatus == MessageStatus.Resent,
                    ShowSMSButton = x.MessageStatus != MessageStatus.Completed
                });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByMessageStatus(MessageStatus status)
        {
            try
            {
                var customers = await App.Database.GetCustomersByMessageStatus(status);
                return customers.Select(x => new OrderItem
                {
                    CreatedOn = x.CreatedOn,
                    CustomerId = x.Id,
                    MessageStatus = x.MessageStatus,
                    MessageCreatedOn = x.UpdatedOn ?? x.CreatedOn,
                    Mobile = x.Mobile,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    ShowCloseButton = x.MessageStatus == MessageStatus.Sent || x.MessageStatus == MessageStatus.Resent,
                    ShowSMSButton = x.MessageStatus != MessageStatus.Completed
                });
            }
            catch (Exception)
            {
                throw;
            }
        }       

        public async Task<Summary> GetOrderSummaryAsync()
        {
            try
            {
                var shop = await App.Database.GetShopAsync();
                var customers = await App.Database.GetCustomersAsync();
                var summary = new Summary
                {
                    Completed = customers.Count(x => x.MessageStatus == MessageStatus.Completed),
                    Prep = customers.Count(x => x.MessageStatus == MessageStatus.Prep),
                    Resent = customers.Count(x => x.MessageStatus == MessageStatus.Resent),
                    Sent = customers.Count(x => x.MessageStatus == MessageStatus.Sent),
                    BusinessName = shop.BusinessName,
                    Total = customers.Count()
                };
                return summary;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SenSmsOniOSAsync(string message, string mobile)
        {
            try
            {
                var sms = new SmsMessage(message, mobile);
                await Sms.ComposeAsync(sms);
            }
            catch (FeatureNotSupportedException)
            {
                throw new FeatureNotSupportedException("Sms is not supported on this device.");
            }
            catch (Exception)
            {
                throw new Exception("Other error has occurred.");
            }
        }

        public async Task<OrderItem> SendSmsAsync(OrderItem orderItem, DevicePlatform platform)
        {
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    var updatedItem = new OrderItem();
                    var message = await CreateMessage(orderItem);
                    if (platform == DevicePlatform.iOS)
                    {
                        await SenSmsOniOSAsync(orderItem.Mobile, message.Text);
                    }
                    else if (platform == DevicePlatform.Android)
                    {
                        await SendMessageAsync(orderItem.Mobile, message.Text);
                    }
                    else 
                    {
                        throw new Exception("Message is not sent on this device.");
                    }
                      
                    //TODO: put it a separate method
                    await App.Database.SaveMessageAsync(message);
                    var customer = await App.Database.GetCustomerByIdAsync(orderItem.CustomerId);
                    customer.UpdatedOn = message.CreatedOn;
                    customer.MessageStatus = message.Status;
                    customer.OrderStatus = message.Status == MessageStatus.Completed ? OrderStatus.Closed : OrderStatus.Active;
                    await App.Database.UpdateCustomerAsync(customer);

                    orderItem.Icon = message.Icon;
                    orderItem.MessageCreatedOn = message.CreatedOn;
                    orderItem.MessageStatus = message.Status;
                    //orderItem.ShowCloseButton = true;
                    orderItem.Text = message.Text;
                    scope.Complete();
                }
                return orderItem;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveCustomer(Customer customer)
        {
            try
            {
                await App.Database.SaveCustomerAsync(customer);
                //if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                //{
                //    await _apiService.PostAsync(customer, Constant.CreateCustomerEndpoint);
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateCustomerAsync(int id)
        {
            try
            {
                var customer = await App.Database.GetCustomerByIdAsync(id);
                customer.Inactive = true;
                await App.Database.UpdateCustomerAsync(customer);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task SendMessageAsync(string mobile, string text)
        {
            try
            {
                await _smsService.SendMessageAsync(mobile, text);
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine("Sms is not supported on this device", ex.Message);
                throw new FeatureNotSupportedException();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occurred", ex.Message);
                throw;
            }
        }

        private async Task<Message> CreateMessage(OrderItem orderItem)
        {
            var shop = await App.Database.GetShopAsync();
            var message = new Message
            {
                CustomerId = orderItem.CustomerId,
                CreatedOn = DateTime.Now,
                OrderNo = orderItem.OrderNo,
                Text = $"{shop.BusinessName}: your order [{orderItem.OrderNo}] is ready to pick up!!"
            };

            switch (orderItem.MessageStatus)
            {
                case MessageStatus.Prep:
                    message.Icon = "sent";
                    message.Status = MessageStatus.Sent;
                    break;
                case MessageStatus.Sent:
                case MessageStatus.Resent:
                    message.Icon = "resent";
                    message.Status = MessageStatus.Resent;
                    break;
                case MessageStatus.Completed:
                    message.Icon = "complete";
                    message.Status = MessageStatus.Completed;
                    message.Text = $"Order: {orderItem.OrderNo} has been picked up.";
                    break;
                default:
                    throw new ArgumentException("Invalid status for command", nameof(orderItem.MessageStatus));
            }
            return message;
        }
    }
}
