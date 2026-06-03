using System.Text.Json.Serialization;

namespace RagAPI.Dtos
{
    public class MedicalSummaryDto
    {
        [JsonPropertyName("main_diagnoses")]
        public List<string> MainDiagnoses { get; set; } = new();

        [JsonPropertyName("recent_symptoms")]
        public List<string> RecentSymptoms { get; set; } = new();

        [JsonPropertyName("treatments")]
        public List<string> Treatments { get; set; } = new();

        [JsonPropertyName("clinical_summary")]
        public string ClinicalSummary { get; set; } = string.Empty;
    }
}