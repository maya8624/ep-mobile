using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IRepos
{
    public interface IMessageRepo
    {
        Task<int> InsertAsync(Message message);
        Task<Message> GetByIdAsync(int messageId);
        Task<Message> GetLastMessageByCustomerId(int customerId);
        Task<List<Message>> GetMessagesAsync();
    }
}
