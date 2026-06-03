using System.Text.Json.Serialization;

namespace RagAPI.Dtos
{
    public class PatientDto
    {
        public string Id { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Cpf { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [JsonPropertyName("dataNascimento")]
        public string DataNascimento { get; set; } = string.Empty;

        public string Genero { get; set; } = string.Empty;

        [JsonPropertyName("tipoSanguineo")]
        public string TipoSanguineo { get; set; } = string.Empty;

        [JsonPropertyName("hospitalVinculado")]
        public string HospitalVinculado { get; set; } = string.Empty;

        [JsonPropertyName("planoSaude")]
        public PlanoSaudeDto? PlanoSaude { get; set; }
    }

    public class PlanoSaudeDto
    {
        [JsonPropertyName("possui")]
        public bool Possui { get; set; }

        [JsonPropertyName("nome")]
        public string Nome { get; set; } = string.Empty;
    }
}
