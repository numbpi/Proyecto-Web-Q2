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
    // Servicio que tiene la logica de los mediadores
    private readonly MediatorService _mediatorService;

    public MediatorController(MediatorService mediatorService)
    {
        _mediatorService = mediatorService;
    }

    // POST api/Mediator - Crea un mediador nuevo (solo admin)
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMediatorDto dto)
    {
        var mediator = await _mediatorService.CreateAsync(dto);
        return Ok(mediator);
    }

    // GET api/Mediator - Trae todos los mediadores registrados
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var mediators = await _mediatorService.GetAllAsync();
        return Ok(mediators);
    }

    // GET api/Mediator/{id} - Trae un mediador por su ID
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

    // PUT api/Mediator/{id} - Actualiza los datos de un mediador
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

    // DELETE api/Mediator/{id} - Desactiva un mediador (no lo borra, solo lo desactiva)
    [HttpDelete("{id}")]
    public async Task<IActionResult> Deactivate(string id)
    {
        try
        {
            await _mediatorService.DeactivateAsync(id);
            return Ok(new { message = "Mediador desactivado correctamente." });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
}
