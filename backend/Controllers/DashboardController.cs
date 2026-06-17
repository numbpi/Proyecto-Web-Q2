using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(DashboardService dbS) : ControllerBase
{
    // Servicio que tiene la logica del dashboard
    private readonly DashboardService _dashboardService = dbS;

    // GET api/Dashboard - Trae los datos del dashboard (totales de casos, mediadores, etc.)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var data = await _dashboardService.GetAsync();
            return Ok(data);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
