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
    private readonly AgreementService _agreementService;

    public AgreementController(AgreementService agreementService)
    {
        _agreementService = agreementService;
    }

    [HttpPost]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> Create([FromBody] CreateAgreementDto dto)
    {
        try
        {
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

    [HttpPut("{agreementId}/confirm")]
    public async Task<IActionResult> Confirm(string agreementId, [FromBody] ConfirmAgreementDto dto)
    {
        try
        {
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

    [HttpPut("{agreementId}/compliance")]
    public async Task<IActionResult> ReportCompliance(
        string agreementId,
        [FromBody] ComplianceReportDto dto
    )
    {
        try
        {
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
