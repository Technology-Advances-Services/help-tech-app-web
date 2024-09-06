namespace HelpTechAppWeb.Models
{
    public class Chat
    {
        public int ChatRoomId { get; set; }
        public int? TechnicalId { get; set; }
        public int? ConsumerId { get; set; }
        public string PersonId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }

        public Chat()
        {
            this.ChatRoomId = 0;
            this.TechnicalId = 0;
            this.ConsumerId = 0;
            this.PersonId = string.Empty;
            this.Sender = string.Empty;
            this.Message = string.Empty;
        }
        public Chat
            (int chatRoomId, string personId,
            DateTime shippingDate, string sender,
            string message)
        {
            this.ChatRoomId = chatRoomId;
            this.PersonId = personId;
            this.ShippingDate = shippingDate;
            this.Sender = sender;
            this.Message = message;
        }
    }
}