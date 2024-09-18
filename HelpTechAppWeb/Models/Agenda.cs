namespace HelpTechAppWeb.Models
{
    public class Agenda
    {
        public int Id { get; set; }
        public string TechnicalId {  get; set; }
        public DateTime RegistrationDate { get; set; }

        public Agenda()
        {
            this.Id = 0;
            this.TechnicalId = string.Empty;
            this.RegistrationDate = DateTime.Now;
        }
        public Agenda
            (int id, string technicalId,
            DateTime registrationDate)
        {
            this.Id = id;
            this.TechnicalId = technicalId;
            this.RegistrationDate = registrationDate;
        }
    }
}