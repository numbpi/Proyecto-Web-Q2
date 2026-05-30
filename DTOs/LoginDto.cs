using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class LoginDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
