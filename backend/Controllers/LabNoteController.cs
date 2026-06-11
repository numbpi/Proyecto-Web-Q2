using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LabNoteController : ControllerBase
{
    private readonly LabNoteService _labNoteService;

    public LabNoteController(LabNoteService labNoteService)
    {
        _labNoteService = labNoteService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateLabNoteDto dto)
    {
        try
        {
            var userId = GetUserIdFromToken();

            var note = await _labNoteService.CreateAsync(dto, userId);

            return Ok(note);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetMyNotes()
    {
        var userId = GetUserIdFromToken();

        var notes = await _labNoteService.GetByUserAsync(userId);

        return Ok(notes);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var userId = GetUserIdFromToken();

            await _labNoteService.DeleteAsync(id, userId);

            return Ok(new { message = "Nota eliminada correctamente" });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(403, new { message = ex.Message });
        }
    }

    private string GetUserIdFromToken()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? User.FindFirstValue("sub")
            ?? throw new UnauthorizedAccessException("No se pudo optener el UserId del token.");
    }
}
