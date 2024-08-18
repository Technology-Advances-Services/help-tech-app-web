﻿namespace HelpTechAppWeb.Models
{
    public class CriminalRecord
    {
        public int TechnicalId { get; private set; }
        public string FileUrl { get; private set; }

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