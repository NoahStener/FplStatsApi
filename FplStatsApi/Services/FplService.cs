using System.Text.Json;

namespace FplStatsApi.Services
{
    public class FplService
    {
        private readonly HttpClient _httpClient;

        public FplService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<JsonDocument> GetBootstrapDataAsync()
        {
            var response = await _httpClient.GetAsync("https://fantasy.premierleague.com/api/bootstrap-static/");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            var json = await JsonDocument.ParseAsync(stream);
            return json;
        }
    }
}
