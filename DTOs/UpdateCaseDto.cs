using System.ComponentModel.DataAnnotations;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.DTOs;

public class UpdateCaseDto
{
    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public string? MediatorId { get; set; } = null;
}
