using ep.Mobile.Interfaces;
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
    public class MessageRepo : IMessageRepo
    {
        private readonly SQLiteAsyncConnection _connection;

        public MessageRepo()
        {
            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
        }

        public async Task<int> InsertAsync(Message message)
        {
            await _connection.CreateTableAsync<Message>();
            return await _connection.InsertAsync(message);
        }

        public async Task<Message> GetByIdAsync(int id)
        {
            return await _connection.Table<Message>().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Message> GetLastMessageByCustomerId(int customerId)
        {
            var message = await _connection
                .Table<Message>()
                .Where(m => m.CustomerId == customerId)
                .OrderByDescending(m => m.Id)
                .FirstOrDefaultAsync();
            return message;
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            return await _connection.Table<Message>().ToListAsync();
        }
    }
}
