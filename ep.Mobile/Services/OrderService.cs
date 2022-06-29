using ep.Mobile.Interfaces.IServices;
using ep.Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Services
{
    public class OrderService : IOrderService
    {
        public Task<IEnumerable<OrderItem>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
