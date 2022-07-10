using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Models
{
    public class Summary
    {
        public int Completed { get; set; }
        public int Prep { get; set; }
        public int Resent { get; set; }
        public int Sent { get; set; }
        public string ShopName { get; set; }
        public int Total { get; set; }
    }
}
