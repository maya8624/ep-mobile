using ep.Mobile.Enums;
using SQLite;
using System;

namespace ep.Mobile.Models
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(30)]
        public string Icon { get; set; }

        public string OrderNo { get; set; }

        public MessageStatus Status { get; set; }

        public int ShopId { get; set; }

        [MaxLength(200)]
        public string Text { get; set; }

        public int CustomerId { get; set; }

        public DateTime CreatedOn { get; set; }        
    }
}

