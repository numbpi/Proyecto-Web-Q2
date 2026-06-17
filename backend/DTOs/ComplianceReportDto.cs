using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos para reportar si se cumplio o no un punto del acuerdo
public class ComplianceReportDto
{
    [Required]
    public int PointIndex { get; set; }

    [Required]
    public string ComplianceStatus { get; set; } = string.Empty;
}
