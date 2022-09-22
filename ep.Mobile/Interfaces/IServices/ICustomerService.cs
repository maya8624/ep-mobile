using ep.Mobile.Enums;
using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ep.Mobile.Interfaces.IServices
{
    public interface ICustomerService
    {
        //TODO: Move SendSMSAsync to MessageService
        Task<bool> AnyCustomers(DateTime dateTime);
        Task DeleteAllRecordsAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<OrderItem> GetCustomerByOrderNoAsync(string orderNo);
        Task<IEnumerable<OrderItem>> GetOrderItemsAsync();
        Task<IEnumerable<OrderItem>> GetOrderItemsByMessageStatus(MessageStatus status);
        Task<int> GetLatestOrderNumberAsync();
        Task<Summary> GetOrderSummaryAsync();
        Task SaveCustomer(Customer customer);
        Task<OrderItem> SendSmsAsync(OrderItem orderItem, DevicePlatform platform);
        Task UpdateCustomerAsync(int id);
    }
}
