using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IRepos
{
    public interface IShopRepo
    {
        Task<int> InsertShopAsync(Shop shop);
        Task<Shop> GetShopAsync();
        Task<List<Shop>>GetShopsAsync();
        Task<int> UpdateShopAsync(Shop shop);
    }
}
