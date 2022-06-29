using SQLite;

namespace ep.Mobile.Interfaces.IRepos
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
        //SQLiteAsyncConnection Connection { get; }
    }
}

