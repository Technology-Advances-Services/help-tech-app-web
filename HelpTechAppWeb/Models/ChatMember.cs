namespace HelpTechAppWeb.Models
{
    public class ChatMember
    {
        public int ChatRoomId { get; set; }
        public string? TechnicalId { get; set; }
        public string? ConsumerId { get; set; }

        public ChatMember()
        {
            this.ChatRoomId = 0;
            this.TechnicalId = null;
            this.ConsumerId = null;
        }
        public ChatMember
            (int chatRoomId, string? technicalId,
            string? consumerId)
        {
            this.ChatRoomId = chatRoomId;
            this.TechnicalId = technicalId;
            this.ConsumerId = consumerId;
        }
    }
}