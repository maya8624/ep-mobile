using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Interfaces.IServices
{
    public interface ISmsService
    {
        Task SendMessageAsync(string mobile, string text);
    }
}
