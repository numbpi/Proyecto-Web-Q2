using Google.Cloud.Firestore;

namespace Proyecto_Web_Q2.Models;

public class AgreementPoint
{
    public string Description { get; set; } = string.Empty;
    public DateTime Deadline { get; set; } = DateTime.UtcNow;
    public string ComplianceStatus { get; set; } = "pendiente";
}
