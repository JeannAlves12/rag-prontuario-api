using System.Net;
using System.Text.Json;
using RagAPI.Dtos;

namespace RagAPI.Services
{
    public class HospitalService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public HospitalService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["SiahApiBaseUrl"]
                ?? throw new InvalidOperationException("A chave 'SiahApiBaseUrl' não foi encontrada no appsettings.json.");
        }

        // Busca os dados pessoais do paciente via /profile?cpf=
        public async Task<PatientDto?> GetPatientAsync(string cpf)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/profile?cpf={cpf}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PatientDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Busca as consultas formatadas do paciente via /api/Consultations/patient/{cpf}
        public async Task<List<ConsultationDto>> GetPatientConsultations(string cpf)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}/api/Consultations/patient/{cpf}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new List<ConsultationDto>();
            }

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ConsultationDto>>(json,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ConsultationDto>();
        }
    }
}