namespace HelpTechAppWeb.Models
{
    public class Chat
    {
        public int ChatRoomId { get; set; }
        public int PersonId { get; set; }
        public DateTime ShippingDate { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }

        public Chat()
        {
            this.ChatRoomId = 0;
            this.PersonId = 0;
            this.Sender = string.Empty;
            this.Message = string.Empty;
        }
        public Chat
            (int chatRoomId, int personId,
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