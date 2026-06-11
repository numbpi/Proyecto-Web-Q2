using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController(DashboardService dbS) : ControllerBase
{
    private readonly DashboardService _dashboardService = dbS;

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
