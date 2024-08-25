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

        public async Task<dynamic?> PostAsync<T>
            (string resource, T content)
        {
            var stringContent = new StringContent
                (JsonConvert.SerializeObject(content),
                Encoding.UTF8, "application/json");

            var httpResponseMessage = await _httpClient
                .PostAsync(resource, stringContent);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<dynamic>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<bool> PostAsync<T>
            (string resource, string token,
            T content)
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

            var result = JsonConvert.DeserializeObject<bool>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<IEnumerable<T>> GetAsync<T>
            (string resource)
        {
            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return [];

            var result = JsonConvert.DeserializeObject<IEnumerable<T>>
                (await httpResponseMessage.Content
                .ReadAsStringAsync()) ?? [];

            return result;
        }

        public async Task<IEnumerable<T>> GetAsync<T>
            (string resource, string token)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue
                ("Bearer", token);

            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return [];

            var result = JsonConvert.DeserializeObject<IEnumerable<T>>
                (await httpResponseMessage.Content
                .ReadAsStringAsync()) ?? [];

            return result;
        }

        public async Task<T?> GetSingleAsync<T>
            (string resource)
        {
            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<T?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }

        public async Task<T?> GetSingleAsync<T>
            (string resource, string token)
        {
            _httpClient.DefaultRequestHeaders
                .Authorization = new AuthenticationHeaderValue
                ("Bearer", token);

            var httpResponseMessage = await _httpClient
                .GetAsync(resource);

            if (!httpResponseMessage.IsSuccessStatusCode)
                return default;

            var result = JsonConvert.DeserializeObject<T?>
                (await httpResponseMessage.Content.ReadAsStringAsync());

            return result;
        }
    }
}