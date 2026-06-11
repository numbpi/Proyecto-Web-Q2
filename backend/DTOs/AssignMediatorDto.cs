using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class AssignMediatorDto
{
    [Required]
    public string MediatorId { get; set; } = string.Empty;
}
