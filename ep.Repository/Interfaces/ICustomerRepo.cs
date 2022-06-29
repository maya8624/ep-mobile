using ep.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Repository.Interfaces
{
    public interface ICustomerRepo
    {
        Task DeleteAsync(Customer customer);
        Task<int> InsertAsync(Customer customer);
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetByKeywordAsync(string keyword);
        Task<IEnumerable<Customer>> GetAllAsync();
        IEnumerable<Customer> GetTodaysCustomers();
        Task UpdateAsync(Customer customer);
    }
}
