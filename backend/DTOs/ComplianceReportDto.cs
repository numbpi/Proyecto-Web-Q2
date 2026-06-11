using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

public class ComplianceReportDto
{
    [Required]
    public int PointIndex { get; set; }

    [Required]
    public string ComplianceStatus { get; set; } = string.Empty;
}
