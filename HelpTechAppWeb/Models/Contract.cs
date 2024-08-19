namespace HelpTechAppWeb.Models
{
    public class Contract
    {
        public int Id { get; private set; }
        public int MembershipId { get; private set; }
        public int? TechnicalId { get; private set; }
        public int? ConsumerId { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime FinalDate { get; private set; }
        public string ContractState { get; private set; }

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