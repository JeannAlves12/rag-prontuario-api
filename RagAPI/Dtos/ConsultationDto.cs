namespace RagAPI.Dtos
{
    public class ConsultationDto
    {
        public string Id { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        public string FinalDiagnosis { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Observations { get; set; } = string.Empty;

        // Mudamos de List<string> para a nossa nova classe abaixo
        public List<MedicationDto>? Medications { get; set; }
    }

    // Lemos apenas o Nome que vem lá do banco de dados
    public class MedicationDto
    {
        public string Name { get; set; } = string.Empty;
    }
}