using System;
using System.IO;
using SQLite;
using ep.iOS.Data;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IRepos;
using Xamarin.Essentials;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ep.iOS.Data
{
    public class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); 
            var path = Path.Combine(documentsPath, "EPDb2.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}

