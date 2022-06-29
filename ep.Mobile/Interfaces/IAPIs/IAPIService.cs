using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IAPIs
{
    public interface IAPIService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(string endPoint);
        Task<T> GetByIdAsync<T>(string endPoint, int id);
        Task<string> PostAsync<T>(T entity, string endPoint);
        Task<int> PutAsync<T>(T entity, string endPoint);
    }
}