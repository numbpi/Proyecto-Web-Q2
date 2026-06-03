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

    /*
      TODO: Los enpoints pendientes que me faltan son:
        1. Para asignar el mediador
        2. Para actualizar  el Caso -> porque ocupo lo del MediadorId

      Porque ocupo lo de mediador
    */
}
