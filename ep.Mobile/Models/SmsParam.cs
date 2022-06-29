using ep.Mobile.Enums;

namespace ep.Mobile.Models
{
    public class SmsParam
    {
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public MessageStatus MessageStatus { get; set; }
    }
}
