namespace HelpTechAppWeb.Models
{
    public class CriminalRecord
    {
        public string TechnicalId { get; set; }
        public string FileUrl { get; set; }

        public CriminalRecord()
        {
            this.TechnicalId = string.Empty;
            this.FileUrl = string.Empty;
        }
        public CriminalRecord
            (string technicalId, string fileUrl)
        {
            this.TechnicalId = technicalId;
            this.FileUrl = fileUrl;
        }
    }
}