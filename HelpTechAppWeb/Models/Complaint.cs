namespace HelpTechAppWeb.Models
{
    public class Complaint
    {
        public int TypeComplaintId { get; private set; }
        public int JobId { get; private set; }
        public string Sender { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public string Description { get; private set; }
        public string ComplaintState { get; private set; }

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