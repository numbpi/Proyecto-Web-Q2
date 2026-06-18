using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos que se necesitan para asignar un mediador a un caso
public class AssignMediatorDto
{
    [Required]
    public string MediatorId { get; set; } = string.Empty;
}
