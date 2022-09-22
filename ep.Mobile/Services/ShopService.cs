using ep.Mobile.Crypto;
using ep.Mobile.Dtos;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Models;
using ep.Mobile.Reference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ep.Mobile.Services
{
    public class ShopService : IShopService
    {
        private readonly IAPIService _apiService;

        public ShopService()
        {
            _apiService = DependencyService.Get<IAPIService>();
        }

        public (string, string) Encrytp(string key, string text)
        {
            var encrypt = CryptoService.Encrypt(key, text);
            return encrypt;
        }
        
        public async Task CreateShopAsync(Shop shop)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    //TODO: don't need get shopId replaced with GUID, change api
                    //var request = await _apiService.PostAsync(shop, Constant.CreateShopEndpoint);
                    //if (!string.IsNullOrEmpty(request))
                    //{
                    //    //TODO: custom exception
                    //    throw new Exception("No data returned from the server.");
                    //}
                    
                    //var response = JsonConvert.DeserializeObject<ShopResponse>(request);
                    //await SecureStorage.SetAsync(Constant.SymKey, response.Key);
                }
                await App.Database.SaveShopAsync(shop);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Shop> GetShopAsync()
        {
            return await App.Database.GetShopAsync();
        }

        public async Task<List<Shop>> GetShopsAsync()
        {
            return await App.Database.GetShopsAsync();
        }

        //TODO: test this function
        public async Task UpdateShopAsync(Shop shop)
        {
            try
            {
                if (Connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    //TODO: Use "Put"
                    //var request = await _apiService.PostAsync(shop, Constant.UpdateShopEndpoint);
                    //if (!string.IsNullOrEmpty(request))
                    //{
                    //    //TODO: custom exception
                    //    throw new Exception("No data returned from the server.");
                    //}
                }
                await App.Database.UpdateShopAsync(shop);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
