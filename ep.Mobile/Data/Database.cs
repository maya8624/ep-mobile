using ep.Mobile.Enums;
using ep.Mobile.Models;
using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ep.Mobile.Data
{
    public class Database
    {
        private readonly SQLiteAsyncConnection _database;

        public Database(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Customer>().Wait();
            _database.CreateTableAsync<Message>().Wait();
            _database.CreateTableAsync<Shop>().Wait();
        }

        public async Task DeleteAllCustomersAsync()
        {
            await _database.DeleteAllAsync<Customer>();
        }
               
        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _database.Table<Customer>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Customer> GetCustomerByOrderNoAsync(string orderNo)
        {
            return await _database
                ?.Table<Customer>()
                ?.FirstOrDefaultAsync(x => x.OrderNo == orderNo);
        }
         
        public async Task<IEnumerable<Customer>> GetCustomersByMessageStatus(MessageStatus status)
        {
            var results = _database
                ?.Table<Customer>()
                ?.Where(x => x.Inactive == false);
            if (status != MessageStatus.Other)
            {
                results = results?.Where(x => x.MessageStatus == status);
            }
            else
            {
                results = results?.Where(x => x.MessageStatus != MessageStatus.Completed);
            }
            return await results
                ?.OrderByDescending(x => x.OrderNo)
                ?.ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersByOrderStatusAsync(OrderStatus status)
        {
            return await _database
                ?.Table<Customer>()
                ?.Where(x => x.OrderStatus == status)
                ?.Where(x => x.Inactive == false)
                ?.OrderByDescending(x => x.OrderNo)
                ?.ToListAsync();
        }

        public async Task<int> SaveCustomerAsync(Customer customer)
        {
            return await _database.InsertAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _database
                ?.Table<Customer>()
                ?.Where(x => x.Inactive == false)
                ?.ToListAsync();
        }

        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            return await _database.UpdateAsync(customer);
        }

        public async Task<int> SaveMessageAsync(Message message)
        {
            return await _database.InsertAsync(message);
        }

        public async Task<List<Shop>> GetShopsAsync()
        {
            return await _database.Table<Shop>().ToListAsync();
        }

        public async Task<Shop> GetShopAsync()
        {
            //TODO: always one shop registered, delete all unused shops
            return await _database.Table<Shop>().FirstOrDefaultAsync();
        }

        public async Task<int> SaveShopAsync(Shop shop)
        {
            return await _database.InsertAsync(shop);
        }

        public async Task<int> UpdateShopAsync(Shop shop)
        {
            return await _database.UpdateAsync(shop);
        }

        public async Task<int> DeleteShopAsync(Shop shop)
        {
            return await _database.DeleteAsync(shop);
        }    

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _database.Table<Message>().ToListAsync();
        }

        //public Task<List<Shop>> QuerySubscribedAsync()
        //{
        //    return _database.QueryAsync<Shop>("SELECT * FROM Person WHERE Subscribed = true");
        //}

        //public Task<List<Shop>> LinqNotSubscribedAsync()
        //{
        //    return _database.Table<Shop>().Where(p => p.Subscribed == false).ToListAsync();
        //}
    }
}
