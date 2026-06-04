using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

//esto de aqui sirve para recibir los datos cuando se quiere crear una notificacion
public class CreateNotificationDto
{
    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Message { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;
}
