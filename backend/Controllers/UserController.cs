using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return Unauthorized();

            var user = await _userService.GetUserAsync(userId);

            if (user == null)
                return NotFound(new { message = "Usuario no encontrado" });

            return Ok(
                new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role,
                }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Este endpoint llama al método GetAllUsersAsync() del UserService.
    // Su función es obtener todos los usuarios registrados en Firebase
    // y devolverlos al frontend para poder mostrarlos en la vista
    // de administración de usuarios (/admin/users).

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();

            return Ok(
                users.Select(user => new
                {
                    user.Id,
                    user.FullName,
                    user.Email,
                    user.Role,
                    user.CreatedAt,
                })
            );
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
