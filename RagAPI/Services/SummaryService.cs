using System.Text;
using System.Text.Json;
using RagAPI.Dtos;

namespace RagAPI.Services
{
    public class SummaryService
    {
        private readonly AiService _aiService;
        private readonly HospitalService _hospitalService;

        public SummaryService(AiService aiService, HospitalService hospitalService)
        {
            _aiService = aiService;
            _hospitalService = hospitalService;
        }


        // ALTERADO: Agora retorna o Super DTO
        public async Task<PatientSummaryResponseDto?> GeneratePatientSummaryAsync(string cpf)
        {
            // 1. Busca os dados pessoais do paciente
            var patient = await _hospitalService.GetPatientAsync(cpf);

            if (patient == null)
                return null; // Se o paciente não existe no hospital, paramos por aqui

            // 2. Busca as consultas
            var consultations = await _hospitalService.GetPatientConsultations(cpf);

            // Se o paciente existe mas não tem consultas, retornamos o paciente com um resumo padrão
            if (consultations == null || !consultations.Any())
            {
                return new PatientSummaryResponseDto
                {
                    Patient = patient,
                    Summary = new MedicalSummaryDto { ClinicalSummary = "Paciente sem histórico de consultas registradas." }
                };
            }

            var historyBuilder = new StringBuilder();
            int i = 1;

            foreach (var c in consultations)
            {
                historyBuilder.AppendLine($"Consulta {i}:");
                historyBuilder.AppendLine($"Data: {c.Date:yyyy-MM-dd}");
                historyBuilder.AppendLine($"Motivo/Anamnese: {c.Reason}");
                historyBuilder.AppendLine($"Diagnóstico: {c.FinalDiagnosis}");
                historyBuilder.AppendLine($"Observações: {c.Observations}");

                // Mágica para extrair o array de remédios para texto:
                var remedios = (c.Medications != null && c.Medications.Any())
                                ? string.Join(", ", c.Medications.Select(m => m.Name))
                                : "Nenhum no sistema";
                historyBuilder.AppendLine($"Medicamentos no Sistema: {remedios}");

                historyBuilder.AppendLine("-----");
                i++;
            }

            var prompt = $$"""
                Você é um assistente médico especializado em análise de histórico clínico.

                Sua tarefa é analisar o histórico de consultas de um paciente e extrair informações relevantes de forma estruturada.
                
                Responda APENAS em JSON válido, sem explicações adicionais.

                Formato obrigatório:
                {
                    "main_diagnoses": [],
                    "recent_symptoms": [],
                    "treatments": [],
                    "clinical_summary": ""
                }

                Regras:
                - Para o campo "main_diagnoses", utilize OBRIGATORIAMENTE o que estiver descrito no campo "Diagnóstico" do sistema. Não coloque doenças antigas do histórico na lista de diagnósticos principais atuais.
                - Extraia sintomas a partir dos motivos das consultas.
                - Extraia tratamentos (medicamentos, repouso ou condutas).
                - REGRA DE CONFLITO: Se o campo "Medicamentos no Sistema" listar algum remédio, ele OBRIGATORIAMENTE deve entrar na lista de "treatments", mesmo que o texto da anamnese diga o contrário (os dados do sistema são prioridade máxima).
                - NÃO invente informações e ignore medicamentos que o paciente apenas "pediu" ou "tem alergia".
                - Se não houver dados reais para algum campo, retorne lista vazia.
                - O campo "clinical_summary" deve ser um resumo curto (máx 3 frases).
                
                Agora analise o histórico abaixo:

                {{historyBuilder}}
                REGRAS CRÍTICAS DE FORMATAÇÃO JSON:
                1. Os campos "main_diagnoses", "recent_symptoms" e "treatments" DEVEM ser um array plano de strings (Exemplo correto: ["item 1", "item 2"]).
                2. É ESTRITAMENTE PROIBIDO usar arrays aninhados ou listas duplas (Exemplo errado: [["item 1"]]). Junte tudo em uma única lista simples.
                """;

            var summaryResponse = await _aiService.AskModel(prompt);

            try
            {
                var cleaned = summaryResponse
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                var aiSummary = JsonSerializer.Deserialize<MedicalSummaryDto>(cleaned);

                // 3. O GRANDE FINAL: Monta o pacote completo juntando o paciente e a IA!
                return new PatientSummaryResponseDto
                {
                    Patient = patient,
                    Summary = aiSummary ?? new MedicalSummaryDto()
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro crítico ao desserializar. Resposta crua do LLM: {summaryResponse}");
                throw new Exception("Falha ao interpretar a resposta da IA.", ex);
            }
        }
    }
}