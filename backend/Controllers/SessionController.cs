using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SessionController : ControllerBase
{
    // Servicio que tiene la logica de las sesiones de mediacion
    private readonly SessionService _sessionService;

    public SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    // POST api/Session - Crea una sesion de mediacion (solo mediador)
    [HttpPost]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> Create([FromBody] CreateSessionDto dto)
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var session = await _sessionService.CreateAsync(dto, userId);

            return Ok(session);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/Session/case/{caseId} - Trae las sesiones de un caso especifico
    [HttpGet("case/{caseId}")]
    public async Task<IActionResult> GetByCaseId(string caseId)
    {
        try
        {
            var sessions = await _sessionService.GetByCaseIdAsync(caseId);

            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // GET api/Session/my - Trae las sesiones del mediador que esta logueado
    [HttpGet("my")]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> GetMySessions()
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var sessions = await _sessionService.GetByMediatorAsync(userId);

            return Ok(sessions);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // PUT api/Session/{sessionId}/status - Actualiza el estado de una sesion (solo mediador)
    [HttpPut("{sessionId}/status")]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> UpdateStatus(
        string sessionId,
        [FromBody] UpdateSessionStatusDto dto
    )
    {
        try
        {
            // Saca el ID del usuario desde el token JWT
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var session = await _sessionService.UpdateStatusAsync(
                sessionId,
                dto.Status,
                dto.SessionNotes,
                userId
            );

            return Ok(session);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public class UpdateSessionStatusDto
{
    public string Status { get; set; } = string.Empty;
    public string SessionNotes { get; set; } = string.Empty;
}