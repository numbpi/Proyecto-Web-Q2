using Proyecto_Web_Q2.DTOs;

namespace Proyecto_Web_Q2.Services;

public class DashboardService(FireBaseService fireBase)
{
    private readonly FireBaseService _fireBaseService = fireBase;

    public async Task<DashboardDto> GetAsync()
    {
        // Para los caso
        var casesSnapshot = await _fireBaseService.GetCollection("cases").GetSnapshotAsync();
        var totalCasos = casesSnapshot.Count;
        var casosPorStatus = casesSnapshot
            .Documents.GroupBy((d) => d.GetValue<string>("Status"))
            .ToDictionary(g => g.Key, g => g.Count());

        // Para los Mediadores
        var mediatorsSnapshot = await _fireBaseService
            .GetCollection("mediators")
            .WhereEqualTo("IsActive", true)
            .GetSnapshotAsync();
        var totalMediadoresActivos = mediatorsSnapshot.Count;

        // Para los acuerdos
        var agreementsSnapshot = await _fireBaseService
            .GetCollection("agreements")
            .GetSnapshotAsync();
        var totalAcuerdos = agreementsSnapshot.Count;
        var acuerdosFormalizadosTotal = agreementsSnapshot.Documents.Count(d =>
            d.ContainsField("FormalizedAt") && d.GetValue<DateTime?>("FormalizedAt") != null
        );
        var acuerdosPendientesTotal = totalAcuerdos - acuerdosFormalizadosTotal;

        // Para los usuarios
        var usersSnapshot = await _fireBaseService.GetCollection("users").GetSnapshotAsync();
        var usuariosRegistradosTotal = usersSnapshot.Count;

        return new DashboardDto
        {
            TotalCases = totalCasos,
            CasesByStatus = casosPorStatus,
            ActiveMediators = totalMediadoresActivos,
            FormalizedAgreements = acuerdosFormalizadosTotal,
            PendingAgreements = acuerdosPendientesTotal,
            TotalUsers = usuariosRegistradosTotal,
        };
    }
}
