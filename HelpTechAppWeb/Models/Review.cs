namespace HelpTechAppWeb.Models
{
    public class Review
    {
        public int TechnicalId { get; set; }
        public int ConsumerId { get; set; }
        public int Score { get; set; }
        public string Opinion { get; set; }
        public string ReviewState { get; set; }

        public Review()
        {
            this.TechnicalId = 0;
            this.ConsumerId = 0;
            this.Score = 0;
            this.Opinion = string.Empty;
            this.ReviewState = string.Empty;
        }
        public Review
            (int technicalId, int consumerId,
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