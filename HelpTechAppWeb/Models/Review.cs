namespace HelpTechAppWeb.Models
{
    public class Review
    {
        public string TechnicalId { get; set; }
        public string ConsumerId { get; set; }
        public int Score { get; set; }
        public string Opinion { get; set; }
        public string ReviewState { get; set; }

        public Review()
        {
            this.TechnicalId = string.Empty;
            this.ConsumerId = string.Empty;
            this.Score = 0;
            this.Opinion = string.Empty;
            this.ReviewState = string.Empty;
        }
        public Review
            (string technicalId, string consumerId,
            int score, string opinion,
            string reviewState)
        {
            this.TechnicalId = technicalId;
            this.ConsumerId = consumerId;
            this.Score = score;
            this.Opinion = opinion;
            this.ReviewState = reviewState;
        }
    }
}