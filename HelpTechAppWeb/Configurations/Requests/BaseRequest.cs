using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using HelpTechAppWeb.Configurations.Interfaces;

namespace HelpTechAppWeb.Configurations.Requests
{
    internal class BaseRequest
        (IHttpClientFactory httpClientFactory) :
        IBaseRequest
    {
        private readonly HttpClient _httpClient = httpClientFactory
            .CreateClient("HelpTechService");

        public async Task<dynamic?> GetAsync
            (string resource)
        {
            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<dynamic?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<dynamic?> GetAsync
            (string resource, string token)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue
                ("Bearer", token);

            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<dynamic?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<dynamic?> PostAsync
            (string resource, object content)
        {
            var stringContent = new StringContent
                (JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync(resource, stringContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<dynamic?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<dynamic?> PostAsync
            (string resource, string token,
            object content)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue
                ("Bearer", token);

            var stringContent = new StringContent
                (JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync(resource, stringContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<dynamic?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }
    }
}