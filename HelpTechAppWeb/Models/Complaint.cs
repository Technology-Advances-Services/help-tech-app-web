namespace HelpTechAppWeb.Models
{
    public class Complaint
    {
        public int TypeComplaintId { get; set; }
        public int JobId { get; set; }
        public string Sender { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Description { get; set; }
        public string ComplaintState { get; set; }

        public Complaint()
        {
            this.TypeComplaintId = 0;
            this.JobId = 0;
            this.Sender = string.Empty;
            this.RegistrationDate = DateTime.MinValue;
            this.Description = string.Empty;
            this.ComplaintState = string.Empty;
        }
        public Complaint
            (int typeComplaintId, int jobId,
            string sender, DateTime registrationDate,
            string description, string complaintState)
        {
            this.TypeComplaintId = typeComplaintId;
            this.JobId = jobId;
            this.Sender = sender;
            this.RegistrationDate = registrationDate;
            this.Description = description;
            this.ComplaintState = complaintState;
        }
    }
}