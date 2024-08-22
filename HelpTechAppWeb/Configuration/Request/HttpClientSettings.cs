namespace HelpTechAppWeb.Configuration.Request
{
    public class HttpClientSettings
    {
        public string HelpTechService { get; set; }

        public HttpClientSettings()
        {
            HelpTechService = string.Empty;
        }
        public HttpClientSettings
            (string baseAddress)
        {
            HelpTechService = baseAddress;
        }
    }
}