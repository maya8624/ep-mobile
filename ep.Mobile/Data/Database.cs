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
            _database.CreateTableAsync<Customer>();
            _database.CreateTableAsync<Message>();
            _database.CreateTableAsync<Shop>();
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            return await _database.Table<Customer>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Customer> GetCustomerByOrderNoAsync(string orderNo)
        {
            return await _database.Table<Customer>().FirstOrDefaultAsync(c => c.OrderNo == orderNo);
        }
              
        public async Task<IEnumerable<Customer>> GetCustomersByOrderStatusAsync(OrderStatus status)
        {
            return await _database.Table<Customer>()
                .Where(c => c.OrderStatus == status)
                .OrderByDescending(c => c.OrderNo)
                .ToListAsync();
        }

        public async Task<int> SaveCustomerAsync(Customer customer)
        {
            return await _database.InsertAsync(customer);
        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _database.Table<Customer>().ToListAsync();
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
