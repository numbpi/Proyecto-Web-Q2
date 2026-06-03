using System.ComponentModel.DataAnnotations;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.DTOs;

public class CreateCaseDto
{
    [Required]
    public string RespondentId { get; set; } = string.Empty;

    [Required]
    public string ConflictType { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Address { get; set; } = string.Empty;
}
