using System.Text;
using System.Text.Json;

namespace RagAPI.Services
{
    public class AiService
    {
        private readonly HttpClient _httpClient;

        public AiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // 5 minutos de timeout é excelente para rodar LLMs localmente, boa sacada!
            _httpClient.Timeout = TimeSpan.FromMinutes(5);
        }

        public async Task<string> AskModel(string prompt)
        {
            var request = new
            {
                model = "phi3",
                prompt = prompt,
                stream = false,
                format = "json", // 🔥 A mágica acontece aqui: força o modelo a retornar JSON nativo
                options = new
                {
                    temperature = 0.2
                    // Removemos o num_predict para o modelo ter fôlego para gerar todo o JSON até o final
                }
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", content);

            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var result = doc.RootElement.GetProperty("response").GetString();

            return result ?? "";
        }
    }
}