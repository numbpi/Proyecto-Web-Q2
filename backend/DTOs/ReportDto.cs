namespace Proyecto_Web_Q2.DTOs;

// DTO para generar un reporte
public class GenerateReportDto
{
    public string Type { get; set; } = string.Empty; // "casos", "acuerdos", "sesiones", "mediadores"
    public DateTime? From { get; set; } // Fecha inicio (opcional)
    public DateTime? To { get; set; } // Fecha fin (opcional)
    public string? MediatorId { get; set; } // Filtrar por mediador (opcional)
    public string? Status { get; set; } // Filtrar por estado (opcional)
}

// DTO de respuesta de un reporte
public class ReportDto
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string GeneratedBy { get; set; } = string.Empty;
    public Dictionary<string, object> Filters { get; set; } = [];
    public Dictionary<string, object> Data { get; set; } = [];
    public DateTime CreatedAt { get; set; }
}
