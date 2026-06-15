namespace Proyecto_Web_Q2.Models;

public class Report
{
    public string Id { get; set; } = string.Empty; // Id del reporte
    public string Type { get; set; } = string.Empty; // "casos", "acuerdos", "sesiones", "mediadores"
    public string GeneratedBy { get; set; } = string.Empty; // UserId de quien generó el reporte
    public Dictionary<string, object> Filters { get; set; } = []; // Filtros aplicados (fechas, mediador, etc.)
    public Dictionary<string, object> Data { get; set; } = []; // Datos del reporte (resumen/stats)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
