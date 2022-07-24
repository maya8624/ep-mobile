using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Models
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public string Email { get; set; }
        
        public DateTime CreatedOn { get; set; }
        
        // Should be hashed
        public string Password { get; set; }
        
        public DateTime UpdatedOn { get; set; }
    }
}
