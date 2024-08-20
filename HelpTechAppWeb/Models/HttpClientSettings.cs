namespace HelpTechAppWeb.Models
{
    public class HttpClientSettings
    {
        public string HelpTechService { get; set; }

        public HttpClientSettings()
        {
            this.HelpTechService = string.Empty;
        }
        public HttpClientSettings
            (string baseAddress)
        {
            this.HelpTechService = baseAddress;
        }
    }
}