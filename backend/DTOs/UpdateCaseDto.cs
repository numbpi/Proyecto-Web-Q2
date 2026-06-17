using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos para actualizar un caso (estado o mediador)
public class UpdateCaseDto
{
    [Required]
    public string Status { get; set; } = string.Empty;

    [Required]
    public string? MediatorId { get; set; } = null;
}
