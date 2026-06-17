using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos para registrar un usuario nuevo
public class RegisterDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = "user";
}
