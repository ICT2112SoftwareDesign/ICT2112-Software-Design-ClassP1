using CleanBrilliantCompany.Services;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CleanBrilliantCompany.ReportManagement.Services
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
            _httpClient.Timeout = TimeSpan.FromSeconds(120);
        }

        //     public async Task<string> GenerateAnalysis(string inputData)
        // {
        //     var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
        //     response.EnsureSuccessStatusCode();

        //     return await response.Content.ReadAsStringAsync();
        // }

        public async Task<string> GenerateAnalysis(string inputData)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "system", content =
                        "You are a professional analyst. Given raw dashboard reports, write a concise business summary. " +
                        "Do not echo the data. Analyze key trends, identify issues, and summarize insights. " +
                        "Do not use markdown or HTML. Write in clear professional language using plain text only. Do not use asterisks (*)" },
                    new { role = "user", content = inputData }
                }
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject(responseString);
            string aiContent = result.choices[0].message.content;

            return result.choices[0].message.content;
        }
    }
}
