﻿namespace HelpTechAppWeb.Models
{
    public class HttpClientSettings
    {
        public string BaseAddress { get; set; }

        public HttpClientSettings()
        {
            this.BaseAddress = string.Empty;
        }
        public HttpClientSettings
            (string baseAddress)
        {
            this.BaseAddress = baseAddress;
        }
    }
}