using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos para actualizar el estado de un caso
public class UpdateStatusDto
{
    [Required]
    public string Status { get; set; } = string.Empty;
}
