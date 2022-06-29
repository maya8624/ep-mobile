using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Models;

namespace ep.Mobile.Interfaces.IServices
{
    public interface IShopService
    {
        Task CreateShopAsync(Shop shop);
        (string eText, string iv) Encrytp(string key, string text);
        Task<Shop> GetShopAsync();
        Task<List<Shop>> GetShopsAsync();
        Task UpdateShopAsync(Shop shop);
    }
}
