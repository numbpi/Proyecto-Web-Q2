using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class UpdateMediatorDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Zone { get; set; } = string.Empty;

    [Required]
    public string Specialty { get; set; } = string.Empty;

    public bool IsAvailable { get; set; }

    public bool IsActive { get; set; }
}
