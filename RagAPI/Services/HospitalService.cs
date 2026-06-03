using System.Net;
using System.Text.Json;
using RagAPI.Dtos;

namespace RagAPI.Services
{
    public class HospitalService
    {
        private readonly HttpClient _httpClient;

        public HospitalService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // NOVO: Método para buscar os dados pessoais do paciente
        public async Task<PatientDto?> GetPatientAsync(string cpf)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7154/Patients/cpf/{cpf}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PatientDto>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // MANTIDO: O método que busca as consultas e já estava blindado
        public async Task<List<ConsultationDto>> GetPatientConsultations(string cpf)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7154/api/Consultations/patient/{cpf}");

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