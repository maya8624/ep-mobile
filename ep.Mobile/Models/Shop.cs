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

        [MaxLength(100)]
        public string Email { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
        
        [MaxLength(100)]
        public string Owner { get; set; }

        public Guid ShopId { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        public string Phone { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public DateTime? UpdatedOn { get; set; }
    }
}
