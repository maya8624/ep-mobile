using ep.Mobile.Enums;
using ep.Mobile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IServices
{
    public interface ICustomerService
    {
        Task DeleteAsync(Customer customer);
        //TODO: Move SendSMSAsync to MessageService
        Task<OrderItem> SendSMSAsync(SmsParam param);
        Task<IEnumerable<OrderItem>> GetCustomersAsync();
        Task<Customer> GetCustomerByIdAsync(int id);
        Task<Customer> GetCustomerByOrderNoAsync(string orderNo);
        Task<IEnumerable<OrderItem>> GetCustomerOrdersByMessageStatus(MessageStatus status);
        Task SaveCustomer(Customer customer);
    }
}
