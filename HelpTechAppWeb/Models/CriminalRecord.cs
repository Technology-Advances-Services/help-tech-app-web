namespace HelpTechAppWeb.Models
{
    public class CriminalRecord
    {
        public int TechnicalId { get; set; }
        public string FileUrl { get; set; }

        public CriminalRecord()
        {
            this.TechnicalId = 0;
            this.FileUrl = string.Empty;
        }
        public CriminalRecord
            (int technicalId, string fileUrl)
        {
            this.TechnicalId = technicalId;
            this.FileUrl = fileUrl;
        }
    }
}