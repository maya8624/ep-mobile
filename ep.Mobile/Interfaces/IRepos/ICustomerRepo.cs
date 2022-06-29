using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Enums;

namespace ep.Mobile.Interfaces.IRepos
{
    public interface ICustomerRepo
    {
        Task DeleteAsync(Customer customer);
        Task DeleteAllAsync();
        Task<int> InsertAsync(Customer customer);
        Task<IEnumerable<Customer>> GetCustomersByOrderStatusAsync(OrderStatus status);
        Task<IEnumerable<Customer>> GetCustomersByMessageStatusAsync(MessageStatus status);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetByKeywordAsync(string keyword);
        Task UpdateAsync(Customer customer);
    }
}
