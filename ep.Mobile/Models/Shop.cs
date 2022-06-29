using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Models
{
    public class Shop
    {
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }

        [MaxLength(50)]
        public string ABN { get; set; }

        [MaxLength(200)]
        public string Address { get; set; }

        public string Email { get; set; }

        [MaxLength(100)]
        public string Owner { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Password { get; set; }

        public int ShopId { get; set; }

        [MaxLength(50)]
        public string Telephone { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
