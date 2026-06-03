using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class UpdateStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
