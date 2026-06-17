using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AgreementController : ControllerBase
{
    // Servicio que tiene la logica de los acuerdos
    private readonly AgreementService _agreementService;

    public AgreementController(AgreementService agreementService)
    {
        _agreementService = agreementService;
    }

    // POST api/Agreement - Crea un acuerdo (solo mediador)
    [HttpPost]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> Create([FromBody] CreateAgreementDto dto)
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var agreement = await _agreementService.CreateAsync(dto, userId);

            return Ok(agreement);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/Agreement/case/{caseId} - Trae el acuerdo de un caso especifico
    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(string caseId)
    {
        try
        {
            var agreement = await _agreementService.GetByCaseIdAsync(caseId);

            if (agreement is null)
                return NotFound(new { message = "No se encontró un acuerdo para este caso" });

            return Ok(agreement);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/Agreement/{agreementId}/confirm - Confirma o rechaza un acuerdo
    [HttpPut("{agreementId}/confirm")]
    public async Task<IActionResult> Confirm(string agreementId, [FromBody] ConfirmAgreementDto dto)
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            dto.AgreementId = agreementId;

            var agreement = await _agreementService.ConfirmAsync(dto, userId);

            return Ok(agreement);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/Agreement/{agreementId}/compliance - Reporta si se cumplio o no un punto del acuerdo
    [HttpPut("{agreementId}/compliance")]
    public async Task<IActionResult> ReportCompliance(
        string agreementId,
        [FromBody] ComplianceReportDto dto
    )
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var agreement = await _agreementService.ReportComplianceAsync(
                agreementId,
                dto.PointIndex,
                dto.ComplianceStatus,
                userId
            );

            return Ok(agreement);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
