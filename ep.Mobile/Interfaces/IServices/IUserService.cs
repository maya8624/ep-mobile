using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ep.Mobile.Models;

namespace ep.Mobile.Interfaces.IServices
{
    public interface IUserService
    {
        Task<int> Insert(User user);
        Task<User> GetById(int id);
        Task Update();
    }
}
