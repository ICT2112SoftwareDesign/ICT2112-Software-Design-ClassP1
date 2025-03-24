using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace CleanBrilliantCompany.Services
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;

        public AIService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public string GenerateAnalysis(string data)
        {
            // API logic
            return $"AI Summary for: {data}";
        }
    }
}
