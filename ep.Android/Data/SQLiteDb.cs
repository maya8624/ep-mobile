using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using ep.Mobile.Interfaces.IRepos;
using Xamarin.Essentials;
using ep.Android.Droid.Data;

[assembly: Dependency(typeof(SQLiteDb))]
namespace ep.Android.Droid.Data
{
	public class SQLiteDb : ISQLiteDb
	{
		public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
             //FileSystem.AppDataDirectory
            var path = Path.Combine(documentsPath, "EPDb2.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}
