namespace HelpTechAppWeb.Models
{
    public class Job
    {
        public int Id { get; private set; }
        public int AgendaId { get; private set; }
        public int ConsumerId { get; private set; }
        public DateTime? AnswerDate { get; private set; }
        public DateTime? WorkDate { get; private set; }
        public string Address { get; private set; }
        public string Description { get; private set; }
        public decimal? Time { get; private set; }
        public decimal? LaborBudget { get; private set; }
        public decimal? MaterialBudget { get; private set; }
        public string JobState { get; private set; }

        public Job()
        {
            this.Id = 0;
            this.AgendaId = 0;
            this.ConsumerId = 0;
            this.Address = string.Empty;
            this.Description = string.Empty;
            this.JobState = string.Empty;
        }
        public Job
            (int id, int agendaId, int consumerId,
            DateTime? answerDate, DateTime? workDate,
            string address, string description,
            decimal? time, decimal? laborBudget,
            decimal? materialBudget, string jobState)
        {
            this.Id = id;
            this.AgendaId = agendaId;
            this.ConsumerId = consumerId;
            this.AnswerDate = answerDate;
            this.WorkDate = workDate;
            this.Address = address;
            this.Description = description;
            this.Time = time;
            this.LaborBudget = laborBudget;
            this.MaterialBudget = materialBudget;
            this.JobState = jobState;
        }
    }
}