using ep.Mobile.Helpers;
using ep.Mobile.Interfaces.IRepos;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ep.Mobile.Repos
{
    public class RepoBase
    {
        private readonly SQLiteAsyncConnection _dbConnection;
        public SQLiteAsyncConnection DBConnection => _dbConnection;

        public RepoBase()
        {
            _dbConnection = DependencyService.Get<ISQLiteDb>().GetConnection();
            if (_dbConnection == null)
            {
                _dbConnection = new SQLiteHelper().Connection;
            }
        }

    }
}
