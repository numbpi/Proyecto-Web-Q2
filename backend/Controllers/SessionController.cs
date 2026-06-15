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
    private readonly SessionService _sessionService;

    public SessionController(SessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> Create([FromBody] CreateSessionDto dto)
    {
        try
        {
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

    [HttpGet("my")]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> GetMySessions()
    {
        try
        {
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

    [HttpPut("{sessionId}/status")]
    [Authorize(Roles = "mediator")]
    public async Task<IActionResult> UpdateStatus(
        string sessionId,
        [FromBody] UpdateSessionStatusDto dto
    )
    {
        try
        {
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