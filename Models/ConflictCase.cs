namespace Proyecto_Web_Q2.Models;

public class ConflictCase
{
    public string Id { get; set; } = string.Empty;
    public string ReporterId { get; set; } = string.Empty;
    public string ReporterName { get; set; } = string.Empty;
    public string RespondentId { get; set; } = string.Empty;
    public string RespondentName { get; set; } = string.Empty;
    public string ConflictType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = "nuevo";
    public string? MediatorId { get; set; } = null;
    public List<string> EvidenceUrls { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? AssignedAt { get; set; } = null;
    public DateTime? ClosedAt { get; set; } = null;
}
