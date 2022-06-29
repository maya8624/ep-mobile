using ep.Mobile.Enums;
using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ep.Mobile.Repos
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly SQLiteAsyncConnection _connection;
  
        public CustomerRepo()
        {
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        private async Task InitTable()
        {
            await _connection.CreateTableAsync<Customer>();
        }
       
        public async Task DeleteAsync(Customer customer)
        {
            await InitTable();
            await _connection.DeleteAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            await InitTable();
            return await _connection.Table<Customer>()
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            await InitTable();
            return await _connection.Table<Customer>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetCustomersByOrderStatusAsync(OrderStatus status)
        {
            await InitTable();
            return await _connection.Table<Customer>()
                .Where(c => c.OrderStatus == status)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByMessageStatusAsync(MessageStatus status)
        {
            await InitTable();
            return await _connection.Table<Customer>()
                .Where(c => c.LatestedMessageStatus == status)
                .OrderByDescending(c => c.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetByKeywordAsync(string keyword)
        {
            await InitTable();
            return await _connection.Table<Customer>().Where(m => m.OrderNo.Contains(keyword)).ToListAsync();
        }

        public async Task<int> InsertAsync(Customer customer)
        {
            await InitTable();
            return await _connection.InsertAsync(customer);
        }

        public async Task UpdateAsync(Customer customer)
        {
            await InitTable();
            await _connection.UpdateAsync(customer);
        }

        public async Task DeleteAllAsync()
        {            
            await _connection.QueryAsync<Customer>("Delete FROM Customer");
        }
    }
}
