using ep.Mobile.Interfaces.IRepos;
using ep.Mobile.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ep.Mobile.Helpers
{
    public class SQLiteHelper
    {
        private readonly SQLiteAsyncConnection _connection;
        public SQLiteAsyncConnection Connection => _connection;
        
        public SQLiteHelper()
        {
            var path = Path.Combine(FileSystem.AppDataDirectory, "EPDb.db");
            _connection = new SQLiteAsyncConnection(path);
            _connection.CreateTableAsync<Shop>();
            _connection.CreateTableAsync<Customer>();
            _connection.CreateTableAsync<Message>();
        }
	}
}
