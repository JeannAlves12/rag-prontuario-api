using Microsoft.AspNetCore.Mvc;
using RagAPI.Services;

namespace RagAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RagController : ControllerBase
    {
        private readonly SummaryService _summaryService;

        public RagController(SummaryService summaryService)
        {
            _summaryService = summaryService;
        }

        [HttpGet("summary/{cpf}")]
        public async Task<IActionResult> GetPatientSummary(string cpf)
        {
            try
            {
                // O Controller agora só delega a tarefa para o serviço especializado
                var summary = await _summaryService.GeneratePatientSummaryAsync(cpf);

                if (summary == null)
                    return NotFound(new { message = "Paciente não possui consultas." });

                return Ok(summary);
            }
            catch (Exception ex)
            {
                // Tratamento de erro elegante caso o LLM falhe ou a API do hospital caia
                return StatusCode(500, new
                {
                    error = "Erro interno ao processar o resumo médico.",
                    details = ex.Message
                });
            }
        }
    }
}