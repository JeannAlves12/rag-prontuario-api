namespace RagAPI.Dtos
{
    public class PatientSummaryResponseDto
    {
        public PatientDto Patient { get; set; } = null!;
        public MedicalSummaryDto Summary { get; set; } = null!;
    }
}
