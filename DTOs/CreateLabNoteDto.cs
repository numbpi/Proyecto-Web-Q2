<<<<<<< HEAD

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

=======
namespace Proyecto_Web_Q2.DTOs;

public class CreateLabNoteDto
{
    public string Title { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; } = 1;
    public bool IsPublic { get; set; } = false;
>>>>>>> 42b05774d045aad39a00d0093ff159e5de1680a6
    public string Tags { get; set; } = string.Empty;
}
