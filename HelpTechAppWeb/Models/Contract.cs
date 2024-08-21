namespace HelpTechAppWeb.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public int? TechnicalId { get; set; }
        public int? ConsumerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string ContractState { get; set; }

        public Contract()
        {
            this.Id = 0;
            this.MembershipId = 0;
            this.TechnicalId = 0;
            this.ConsumerId = 0;
            this.StartDate = DateTime.MinValue;
            this.FinalDate = DateTime.MinValue;
            this.ContractState = string.Empty;
        }
        public Contract
            (int id, int membershipId,
            int? technicalId, int? consumerId,
            DateTime startDate, DateTime finalDate,
            string contractState)
        {
            this.Id = id;
            this.MembershipId = membershipId;
            this.TechnicalId = technicalId;
            this.ConsumerId = consumerId;
            this.StartDate = startDate;
            this.FinalDate = finalDate;
            this.ContractState = contractState;
        }
    }
}