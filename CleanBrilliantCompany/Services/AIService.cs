using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
                throw new Exception("OPENAI_API_KEY not found in environment variables.");

            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        public async Task<string> GenerateAnalysis(string inputData)
    {
        var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }

        // public async Task<string> GenerateAnalysis(string inputData)
        // {
        //     var requestBody = new
        //     {
        //         model = "gpt-3.5-turbo",
        //         messages = new[]
        //         {
        //             new { role = "system", content = "You are a report writer who summarizes business data into a professional report." },
        //             new { role = "user", content = inputData }
        //         }
        //     };

        //     var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
        //     var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
        //     response.EnsureSuccessStatusCode();

        //     var responseString = await response.Content.ReadAsStringAsync();
        //     dynamic result = JsonConvert.DeserializeObject(responseString);

        //     return result.choices[0].message.content;
        // }
    }
}
