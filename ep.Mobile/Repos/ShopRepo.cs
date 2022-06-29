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
    public class ShopRepo : IShopRepo
    {
        private SQLiteAsyncConnection _connection;

        public ShopRepo()
        {
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        private async Task InitTable()
        {
            try
            {
                await _connection.CreateTableAsync<Shop>();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<Shop>> GetShopsAsync()
        {
            try
            {
                var shops = await _connection.Table<Shop>().ToListAsync();
                return shops;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Shop> GetShopAsync()
        {
            try
            {                
                return await _connection.Table<Shop>().FirstOrDefaultAsync(s => s.Id == 1);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> InsertShopAsync(Shop shop)
        {
            await InitTable();
            return await _connection.InsertAsync(shop);
        }

        public async Task<int> UpdateShopAsync(Shop shop)
        {
            try
            {
                //await InitTable();
                return await _connection.UpdateAsync(shop);

            }
            catch (Exception ex)
            { 
                throw;
            }
        }
    }
}
