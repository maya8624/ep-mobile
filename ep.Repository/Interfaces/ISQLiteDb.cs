using SQLite;

namespace ep.Repository.Interfaces
{
    public interface ISQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}

