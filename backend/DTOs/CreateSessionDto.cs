using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos que se mandan al programar una sesion de mediacion
public class CreateSessionDto
{
    [Required]
    public string CaseId { get; set; } = string.Empty;

    [Required]
    public DateTime ScheduledDate { get; set; }

    [Required]
    public string ScheduledTime { get; set; } = string.Empty;

    [Required]
    public string Modality { get; set; } = string.Empty;

    public string MeetingLink { get; set; } = string.Empty;

    public string SessionNotes { get; set; } = string.Empty;
}
