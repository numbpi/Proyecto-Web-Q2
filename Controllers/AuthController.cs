using Microsoft.AspNetCore.Mvc;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Services;

namespace Proyecto_Web_Q2.Controllers;

/*
NOTA:

Siempre deben colocar [Route("X cosa aca")]
porque el ApiController solo va hacer target a ellos aunque tengamos la clase y todo , pero no eso.
No va reconcer esa petición si no tiene eso
*/

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    private readonly AuthService _authService = authService;

    // Este es para registrarnos el request line seria => POST/api/Auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        try
        {
            var user = await _authService.Register(dto);

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
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    // Este es para hacer login, la request line seria => POST /api/Auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            // Si todo queda bien nos dara un JWT
            var token = await _authService.Login(dto);

            //Devolvemos el token al frontend para que lo guarde
            //El frontend debe mandarlo en cada peticion como Bearer Token
            return Ok(new { token });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
}
