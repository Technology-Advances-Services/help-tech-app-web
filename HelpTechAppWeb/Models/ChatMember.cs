namespace HelpTechAppWeb.Models
{
    public class ChatMember
    {
        public int ChatRoomId { get; private set; }
        public int? TechnicalId { get; private set; }
        public int? ConsumerId { get; private set; }

        public ChatMember()
        {
            this.ChatRoomId = 0;
            this.TechnicalId = null;
            this.ConsumerId = null;
        }
        public ChatMember
            (int chatRoomId, int? technicalId,
            int? consumerId)
        {
            this.ChatRoomId = chatRoomId;
            this.TechnicalId = technicalId;
            this.ConsumerId = consumerId;
        }
    }
}