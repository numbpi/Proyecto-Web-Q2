using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

//aqui vamos a definir los datos que se necesitan para crear un mediador.

public class CreateMediatorDto
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string Zone { get; set; } = string.Empty;

    [Required]
    public string Specialty { get; set; } = string.Empty;

    public bool IsAvailable { get; set; } = true;

    public string? UserId { get; set; }
}

// No lleva Id, CreatedAt ni ActiveCases, porque esos los genera el servidor ya de un solo
