namespace HelpTechAppWeb.Models
{
    public class Contract
    {
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public string? TechnicalId { get; set; }
        public string? ConsumerId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Policies { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinalDate { get; set; }
        public string ContractState { get; set; }

        public Contract()
        {
            this.Id = 0;
            this.MembershipId = 0;
            this.TechnicalId = string.Empty;
            this.ConsumerId = string.Empty;
            this.Name = string.Empty;
            this.Price = 0;
            this.Policies = string.Empty;
            this.StartDate = DateTime.MinValue;
            this.FinalDate = DateTime.MinValue;
            this.ContractState = string.Empty;
        }
        public Contract
            (int id, int membershipId,
            string? technicalId, string? consumerId,
            string name, decimal price, string policies,
            DateTime startDate, DateTime finalDate,
            string contractState)
        {
            this.Id = id;
            this.MembershipId = membershipId;
            this.TechnicalId = technicalId;
            this.ConsumerId = consumerId;
            this.Name= name;
            this.Price = price;
            this.Policies = policies;
            this.StartDate = startDate;
            this.FinalDate = finalDate;
            this.ContractState = contractState;
        }
    }
}