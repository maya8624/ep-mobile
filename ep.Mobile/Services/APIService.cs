using ep.Mobile.Interfaces.IAPIs;
using ep.Mobile.Models;
using ep.Mobile.Reference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace ep.Mobile.Services
{
    public class APIService : IAPIService
    {
        private readonly HttpClient _client;

        public APIService()
        {
            _client = new HttpClient();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(string endPoint)
        {
            var apiUrl = $"{Constant.ApiBaseUrl}/{endPoint}";
            var content = await _client.GetStringAsync(apiUrl);
            return JsonConvert.DeserializeObject<IEnumerable<T>>(content);
        }

        public async Task<T> GetByIdAsync<T>(string endPoint, int id)
        {
            try
            {
                var uri = $"{Constant.ApiBaseUrl}/{endPoint}/{id}";
                var content = await _client.GetStringAsync(uri);
                var response = JsonConvert.DeserializeObject<T>(content);
                return response;
            }
            catch (Exception ex)
            {                
                throw;
            }
        }

        public async Task<string> PostAsync<T>(T entity, string endPoint)
        {
            try
            {
                var apiUrl = $"{Constant.ApiBaseUrl}/{endPoint}";
                var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync(apiUrl, content);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> PutAsync<T>(T entity, string endPoint)
        {
            try
            {
                var apiUrl = $"{Constant.ApiBaseUrl}/{endPoint}";
                var content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json");
                var result = await _client.PutAsync(apiUrl, content);
                return Convert.ToInt16(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
