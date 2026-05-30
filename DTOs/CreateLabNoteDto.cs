
using System.ComponentModel.DataAnnotations;
namespace Proyecto_Web_Q2.DTOs;


//aqui no se va usar UserId porque se debe salir del token
public class CreateLabNoteDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Observation { get; set; } = string.Empty;

    [Required]
    public string Category { get; set; } = string.Empty;

    [Required]
    public int Priority { get; set; }

    public bool IsPublic { get; set; }

    public string Tags { get; set; } = string.Empty;
}
