using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CaseController : ControllerBase
{
    private readonly CaseService _caseService;

    public CaseController(CaseService caseService)
    {
        _caseService = caseService;
    }

    [HttpPost]
    [Authorize(Roles = "user")]
    public async Task<IActionResult> Create([FromBody] CreateCaseDto dto)
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cases = await _caseService.CreateAsync(dto, userId);

            return Ok(cases);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
                return Unauthorized();

            var cases = await _caseService.GetCasesAsync(userId, role);

            return Ok(cases);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    [Route("{caseId}")]
    public async Task<IActionResult> GetById(string caseId)
    {
        try
        {
            var caso = await _caseService.GetByIdAsync(caseId);

            if (caso is null)
                return NotFound(new { message = "No fue encontrado el caso" });

            return Ok(caso);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Roles = "admin")]
    [Route("{caseId}/assign")]
    public async Task<IActionResult> AsignMediator(string caseId, [FromBody] AssignMediatorDto dto)
    {
        try
        {
            var caso = await _caseService.AssignMediatorAsync(caseId, dto.MediatorId);
            return Ok(caso);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut]
    [Authorize(Roles = "mediator")]
    [Route("{caseId}/status")]
    public async Task<IActionResult> UpdateStatusAsync(
        string caseId,
        [FromBody] UpdateStatusDto dto
    )
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var caso = await _caseService.UpdateStatusAsync(caseId, dto.Status, userId!);

            return Ok(caso);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
