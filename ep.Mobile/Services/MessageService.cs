using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Interfaces;
using ep.Mobile.Models;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Interfaces.IAPIs;

namespace ep.Mobile.Services
{
    public class MessageService
    {
        private readonly IAPIService _apiService;

        private const string _baseUrl = "https://webapiepager20211220105219.azurewebsites.net/api/message";
        private HttpClient _client;

        public MessageService()
        {
            _apiService = DependencyService.Get<IAPIService>();
        }

        public async Task PutMessage(Message message)
        {
            _client = new HttpClient();
            var content = JsonConvert.SerializeObject(message);
            await _client.PutAsync($"{_baseUrl}/update-message", new StringContent(content));
        }
    }
}
