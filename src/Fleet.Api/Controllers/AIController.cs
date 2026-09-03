using Fleet.Application.DTOs;
using Fleet.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fleet.Api.Controllers;

/// <summary>
/// Expone operaciones relacionadas con el agente de IA.
/// </summary>
[ApiController]
[Route("api/ai")]
public sealed class AIController : ControllerBase
{
    private readonly IOperationalAgent _operationalAgent;

    public AIController(IOperationalAgent operationalAgent)
    {
        _operationalAgent = operationalAgent;
    }

    /// <summary>
    /// Procesa una consulta operativa mediante el agente de IA.
    /// </summary>
    [HttpPost("chat")]
    public async Task<ActionResult<AIChatResponse>> Chat(
        [FromBody] AIChatRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
        {
            return BadRequest(
                "El mensaje de consulta es obligatorio.");
        }

        var answer = await _operationalAgent.AskAsync(
            request.Message,
            cancellationToken);

        return Ok(new AIChatResponse(answer));
    }
}