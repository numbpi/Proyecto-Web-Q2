namespace Proyecto_Web_Q2.Models;

public class MediationSession
{
    public string Id { get; set; } = string.Empty;
    public string CaseId { get; set; } = string.Empty;
    public string MediatorId { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string ScheduledTime { get; set; } = string.Empty;
    public string Modality { get; set; } = string.Empty;
    public string MeetingLink { get; set; } = string.Empty;
    public string Status { get; set; } = "programada";
    public string SessionNotes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}