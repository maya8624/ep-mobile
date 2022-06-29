using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IServices
{
    public interface IMessageService
    {
        Task<int> InsertAsync(Message message);
        Task<Message> GetByIdAsync(int id);
        Task<List<Message>> GetMessagesAsync();
    }
}
