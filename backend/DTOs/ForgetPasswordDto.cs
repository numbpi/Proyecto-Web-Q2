using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class ForgetPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
}
