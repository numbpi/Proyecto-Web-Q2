using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MediatorController : ControllerBase
{
    private readonly MediatorService _mediatorService;

    public MediatorController(MediatorService mediatorService)
    {
        _mediatorService = mediatorService;
    }

    // Permite registrar un nuevo mediador
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMediatorDto dto)
    {
        var mediator = await _mediatorService.CreateAsync(dto);
        return Ok(mediator);
    }

    // Obtiene todos los mediadores registrados
    // Este endpoint será utilizado por el frontend
    // para mostrarlos en la vista /admin/mediators
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var mediators = await _mediatorService.GetAllAsync();
        return Ok(mediators);
    }

    // Obtiene un mediador específico por su Id
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var mediator = await _mediatorService.GetByIdAsync(id);
            return Ok(mediator);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Actualiza la información de un mediador
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateMediatorDto dto)
    {
        try
        {
            var mediator = await _mediatorService.UpdateAsync(id, dto);
            return Ok(mediator);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Desactiva un mediador
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(string id)
    {
        try
        {
            await _mediatorService.DeactivateAsync(id);

            return Ok(new
            {
                message = "Mediador desactivado correctamente."
            });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}