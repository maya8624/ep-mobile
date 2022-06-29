using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Models
{
    public class QRScan
    {
        public int Id { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public string Mobile { get; set; }
        public string Name { get; set; }
        public string OrderNo { get; set; }
        public int ShopId { get; set; }
    }
}