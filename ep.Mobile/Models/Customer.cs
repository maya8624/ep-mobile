﻿using ep.Mobile.Enums;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ep.Mobile.Models
{
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        
        public bool Inactive { get; set; } = false;

        public MessageStatus MessageStatus { get; set; } = MessageStatus.Prep;

        public string Mobile { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string OrderNo { get; set; } = string.Empty;

        public OrderStatus OrderStatus { get; set; } = OrderStatus.Active;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        
        public DateTime? UpdatedOn { get; set; }
    }
}
