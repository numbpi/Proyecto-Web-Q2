using System.ComponentModel.DataAnnotations;

namespace Proyecto_Web_Q2.DTOs;

// Datos que se mandan al crear un acuerdo
public class CreateAgreementDto
{
    [Required]
    public string CaseId { get; set; } = string.Empty;

    [Required]
    public string AgreementText { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CreateAgreementPointDto> Points { get; set; } = [];
}

// Datos de un punto del acuerdo (cada compromiso que se toma)
public class CreateAgreementPointDto
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime Deadline { get; set; } = DateTime.UtcNow;
}
