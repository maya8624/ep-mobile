using ep.Mobile.Enums;
using System;

namespace ep.Mobile.Models
{
    public class OrderItem
    {
        public DateTimeOffset CreatedOn { get; set; }

        public int CustomerId { get; set; }

        public string Icon { get; set; }
        
        public DateTimeOffset MessageCreatedOn { get; set; }
        
        public string Mobile { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string OrderNo { get; set; } = string.Empty;

        public int ShopId { get; set; }

        public MessageStatus MessageStatus { get; set; }

        public bool ShowCloseButton { get; set; }

        public bool ShowSMSButton { get; set; } = true;

        public string Text { get; set; }

        public SmsParam SMSParam { get; set; }
    }
}
