namespace Proyecto_Web_Q2.DTOs;

public class DashboardDto
{
    public int TotalCases { get; set; } // Nos va dar cuantos casos hay en el FB
    public Dictionary<string, int>? CasesByStatus { get; set; } // Van estar agrupados segun el status por ejemplo =>  {"nuevo": 5, "asignado": 3 y etc... xd}
    public int ActiveMediators { get; set; } // Nos va devolver los mediadores que si andan chambeando
    public int FormalizedAgreements { get; set; } // Nos va devolver cuantos acuerdos ya fuerzon formalizados o mejor dichos donde ya se llego aun veredicto
    public int PendingAgreements { get; set; } // Nos va devolver cuantos acuerdos quedan pendientes
    public int TotalUsers { get; set; } // Nos va devolver cuantos usarios hay registrados en el FB
}
