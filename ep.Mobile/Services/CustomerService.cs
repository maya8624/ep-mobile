using ep.Mobile.Enums;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IAPIService _apiService;
        private readonly ICustomerRepo _customerRepo;
        private readonly ISmsService _smsService;
        private readonly string _createCustomerEndPoint = "customer/create";

        public CustomerService()
        {
            _apiService = DependencyService.Get<IAPIService>();
            _customerRepo = DependencyService.Get<ICustomerRepo>();
            _smsService = DependencyService.Get<ISmsService>();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            try
            {
                //return await _customerRepo.GetByIdAsync(id);
                return await App.Database.GetCustomerByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Customer> GetCustomerByOrderNoAsync(string orderNo)
        {
            try
            {
                //return await _customerRepo.GetByIdAsync(id);
                return await App.Database.GetCustomerByOrderNoAsync(orderNo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetCustomersAsync()
        {
            try
            {  
                var customers = await App.Database.GetCustomersByOrderStatusAsync(OrderStatus.Active);
                var orders = new List<OrderItem>();
                foreach (var customer in customers)
                {
                    orders.Add(new OrderItem
                    {
                        CreatedOn = customer.CreatedOn,
                        CustomerId = customer.Id,
                        MessageStatus = customer.LatestedMessageStatus,
                        MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                        Mobile = customer.Mobile,
                        Name = customer.Name,
                        OrderNo = customer.OrderNo,
                        ShopId = customer.ShopId,
                        ShowCloseButton = customer.LatestedMessageStatus == MessageStatus.Sent || customer.LatestedMessageStatus == MessageStatus.Resent,
                        ShowSMSButton = customer.LatestedMessageStatus != MessageStatus.Completed,
                        SMSParam = new SmsParam
                        {
                            CustomerId = customer.Id,
                            ShopId = customer.ShopId,
                            MessageStatus = customer.LatestedMessageStatus
                        }
                    });
                }
                return orders.OrderByDescending(o => o.CustomerId);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
                
        public async Task<IEnumerable<OrderItem>> GetCustomerOrdersByMessageStatus(MessageStatus status)
        {
            try
            {
                var orders = new List<OrderItem>();
                var customers = status == MessageStatus.Other ?
                    await _customerRepo.GetAllAsync() :
                    await _customerRepo.GetCustomersByMessageStatusAsync(status);
                foreach (var customer in customers)
                {
                    orders.Add(new OrderItem
                    {
                        CustomerId = customer.Id,
                        CreatedOn = customer.CreatedOn,
                        Mobile = customer.Mobile,
                        Name = customer.Name,
                        MessageStatus = customer.LatestedMessageStatus,
                        MessageCreatedOn = customer.UpdatedOn ?? customer.CreatedOn,
                        OrderNo = customer.OrderNo,
                        SMSParam = new SmsParam 
                        { 
                            CustomerId = customer.Id, 
                            MessageStatus = customer.LatestedMessageStatus, 
                            ShopId = customer.ShopId 
                        }
                    });
                }
                return orders;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<OrderItem> SendSMSAsync(SmsParam param)
        {
            try
            {
                var customer = await GetCustomerByIdAsync(param.CustomerId);
                var message = SetMessage(customer, param);               
                await SendMessageAsync(customer.Mobile, message.Text);

                var orderItem = new OrderItem
                {
                    CustomerId = customer.Id,
                    CreatedOn = customer.CreatedOn,
                    Icon = message.Icon,
                    MessageCreatedOn = message.CreatedOn,
                    Mobile = customer.Mobile,
                    Name = customer.Name,
                    OrderNo = customer.OrderNo,
                    MessageStatus = message.Status,
                    ShopId = message.ShopId,
                    ShowCloseButton = true,
                    Text = message.Text,
                    SMSParam = new SmsParam { CustomerId = customer.Id, MessageStatus = message.Status, ShopId = customer.ShopId}
                };
                                
                //await _messageRepo.InsertAsync(message);
                await App.Database.SaveMessageAsync(message);
                customer.UpdatedOn = message.CreatedOn;
                customer.LatestedMessageStatus = message.Status;
                //await _customerRepo.UpdateAsync(customer);
                await App.Database.UpdateCustomerAsync(customer);
                
                //await _apiService.PostAsync(message, _messageCreateEndPoint);
                return orderItem;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task SaveCustomer(Customer customer)
        {
            try
            {
                await App.Database.SaveCustomerAsync(customer);
                //await _apiService.PostAsync(customer, _createCustomerEndPoint);
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
                throw new Exception();
            }
        }

        private Message SetMessage(Customer customer, SmsParam param)
        {
            var message = new Message
            {
                CustomerId = customer.Id,
                CreatedOn = DateTimeOffset.UtcNow,
                ShopId = customer.ShopId,
                OrderNo = customer.OrderNo,
                Text = $"Your Order: {customer.OrderNo} is ready to pick up"
            };

            switch (param.MessageStatus)
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
                    message.Text = $"Order: {customer.OrderNo} has been picked up.";
                    break;
                default:
                    throw new ArgumentException("Invalid status for command", nameof(param.MessageStatus));
            }
            return message;
        }

        public async Task DeleteAsync(Customer customer)
        {
            await _customerRepo.DeleteAsync(customer);
        }
    }
}
