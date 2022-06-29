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
using ep.Mobile.Interfaces.IRepos;

namespace ep.Mobile.Services
{
    public class MessageService : IMessageService
    {
        private readonly IAPIService _apiService;
        private readonly IMessageRepo _messageRepo;

        private const string _baseUrl = "https://webapiepager20211220105219.azurewebsites.net/api/message";
        private HttpClient _client;

        public MessageService()
        {
            _apiService = DependencyService.Get<IAPIService>();
            _messageRepo = DependencyService.Get<IMessageRepo>();
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _messageRepo.GetMessagesAsync();
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _messageRepo.GetByIdAsync(id);
        }

        public async Task<int> InsertAsync(Message message)
        {
            return await _messageRepo.InsertAsync(message);
        }

        public async Task PutMessage(Message message)
        {
            _client = new HttpClient();
            var content = JsonConvert.SerializeObject(message);
            await _client.PutAsync($"{_baseUrl}/update-message", new StringContent(content));
        }
    }
}
