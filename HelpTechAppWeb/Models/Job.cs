namespace HelpTechAppWeb.Models
{
    public class Job
    {
        public int Id { get; set; }
        public int AgendaId { get; set; }
        public string? PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Phone { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? AnswerDate { get; set; }
        public DateTime? WorkDate { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public decimal? Time { get; set; }
        public decimal? LaborBudget { get; set; }
        public decimal? MaterialBudget { get; set; }
        public decimal? AmountFinal { get; set; }
        public string JobState { get; set; }

        public Job()
        {
            this.Id = 0;
            this.AgendaId = 0;
            this.PersonId = string.Empty;
            this.Address = string.Empty;
            this.Description = string.Empty;
            this.AmountFinal = 0;
            this.JobState = string.Empty;
        }
        public Job
            (int id, int agendaId, string personId,
            DateTime? answerDate, DateTime? workDate,
            string address, string description,
            decimal? time, decimal? laborBudget,
            decimal? materialBudget, string jobState)
        {
            this.Id = id;
            this.AgendaId = agendaId;
            this.PersonId = personId;
            this.AnswerDate = answerDate;
            this.WorkDate = workDate;
            this.Address = address;
            this.Description = description;
            this.Time = time;
            this.LaborBudget = laborBudget;
            this.MaterialBudget = materialBudget;
            this.AmountFinal = laborBudget + materialBudget;
            this.JobState = jobState;
        }
    }
}