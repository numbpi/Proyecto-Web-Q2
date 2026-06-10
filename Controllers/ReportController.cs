namespace Proyecto_Web_Q2.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController(ReportService reportService) : ControllerBase
{
    private readonly ReportService _reportService = reportService;
 
    private string GetUserId() =>
        User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("No se pudo obtener el usuario autenticado");
 
    /// <summary>
    /// Genera un nuevo reporte según tipo y filtros.
    /// Tipos: "casos", "acuerdos", "sesiones", "mediadores"
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Generate([FromBody] GenerateReportDto dto)
    {
        try
        {
            var userId = GetUserId();
            var report = await _reportService.GenerateAsync(dto, userId);
 
            var response = new ReportDto
            {
                Id = report.Id,
                Type = report.Type,
                GeneratedBy = report.GeneratedBy,
                Filters = report.Filters,
                Data = report.Data,
                CreatedAt = report.CreatedAt,
            };
 
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
 
    /// <summary>
    /// Obtiene un reporte por su Id.
    /// </summary>
    [HttpGet("{reportId}")]
    public async Task<IActionResult> GetById(string reportId)
    {
        try
        {
            var report = await _reportService.GetByIdAsync(reportId);
 
            var response = new ReportDto
            {
                Id = report.Id,
                Type = report.Type,
                GeneratedBy = report.GeneratedBy,
                Filters = report.Filters,
                Data = report.Data,
                CreatedAt = report.CreatedAt,
            };
 
            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }
 
    /// <summary>
    /// Obtiene todos los reportes generados por el usuario autenticado.
    /// </summary>
    [HttpGet("my")]
    public async Task<IActionResult> GetMine()
    {
        try
        {
            var userId = GetUserId();
            var reports = await _reportService.GetByUserAsync(userId);
 
            var response = reports.Select(r => new ReportDto
            {
                Id = r.Id,
                Type = r.Type,
                GeneratedBy = r.GeneratedBy,
                Filters = r.Filters,
                Data = r.Data,
                CreatedAt = r.CreatedAt,
            });
 
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
 
    /// <summary>
    /// Lista todos los reportes del sistema (uso administrativo/mediadores).
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var reports = await _reportService.GetAllAsync();
 
            var response = reports.Select(r => new ReportDto
            {
                Id = r.Id,
                Type = r.Type,
                GeneratedBy = r.GeneratedBy,
                Filters = r.Filters,
                Data = r.Data,
                CreatedAt = r.CreatedAt,
            });
 
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
