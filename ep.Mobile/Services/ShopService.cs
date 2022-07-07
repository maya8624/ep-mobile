using System.Threading.Tasks;
using ep.Mobile.Models;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Interfaces.IAPIs;
using System;
using System.Collections.Generic;
using ep.Mobile.Dtos;
using Newtonsoft.Json;
using ep.Mobile.Crypto;
using Xamarin.Essentials;

namespace ep.Mobile.Services
{
    public class ShopService : IShopService
    {
        private readonly IAPIService _apiService;
        private readonly IShopRepo _shopRepo;
        private const string _createEndPoint = "api/shop/create";
        private const string _updateEndPoint = "api/shop/update";

        public ShopService()
        {
            _shopRepo = DependencyService.Get<IShopRepo>();
            _apiService = DependencyService.Get<IAPIService>();
        }

        public (string, string) Encrytp(string key, string text)
        {
            var encrypt = CryptoService.Encrypt(key, text);
            return encrypt;
        }

        public async Task<Shop> GetShopAsync()
        {
            return await App.Database.GetShopAsync();
        }

        public async Task<List<Shop>> GetShopsAsync()
        {
            return await _shopRepo.GetShopsAsync();
        }

        public async Task CreateShopAsync(Shop shop)
        {
            try
            {
                var response = await _apiService.PostAsync(shop, _createEndPoint);
                if (!string.IsNullOrEmpty(response))
                {
                    //TODO: show error                
                }

                var shopResponse = JsonConvert.DeserializeObject<ShopResponse>(response);
                await SecureStorage.SetAsync("symKey", shopResponse.Key);
                shop.ShopId = shopResponse.ShopId;
                await App.Database.SaveShopAsync(shop);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task UpdateShopAsync(Shop shop)
        {
            try
            {
                var response = await _apiService.PostAsync(shop, _createEndPoint);
                if (!string.IsNullOrEmpty(response))
                {
                    //TODO: show error                
                }

                var shopResponse = JsonConvert.DeserializeObject<ShopResponse>(response);
                await SecureStorage.SetAsync("symKey", shopResponse.Key);
                shop.ShopId = shopResponse.ShopId;
                await App.Database.UpdateShopAsync(shop);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //public async Task UpdateShopAsync(Shop shop)
        //{ 
        //    await _shopRepo.UpdateShopAsync(shop);
        //    //await _apiService.PostAsync(shop, _createEndPoint);
        //    // TODO: update shop data on server
        //    //await _apiService.PutAsync(shop, _updateEndPoint);
        //}
    }
}
