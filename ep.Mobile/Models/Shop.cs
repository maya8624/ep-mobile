using SQLite;
using System;

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

        //public string Email { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Owner { get; set; }

        public int ShopId { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset? UpdatedOn { get; set; }
    }
}
