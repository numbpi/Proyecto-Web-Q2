using Google.Cloud.Firestore;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class ReportService
{
    private readonly FireBaseService _fireBaseService;
    private readonly string collectionName = "reports";

    public ReportService(FireBaseService fireBase)
    {
        _fireBaseService = fireBase;
    }

    /// <summary>
    /// Genera y persiste un reporte según el tipo y filtros indicados.
    /// Tipos soportados: "casos", "acuerdos", "sesiones", "mediadores"
    /// </summary>
    public async Task<Report> GenerateAsync(GenerateReportDto dto, string userId)
    {
        var data = dto.Type switch
        {
            "casos" => await BuildCasesReportDataAsync(dto),
            "acuerdos" => await BuildAgreementsReportDataAsync(dto),
            "sesiones" => await BuildSessionsReportDataAsync(dto),
            "mediadores" => await BuildMediatorsReportDataAsync(dto),
            _ => throw new Exception($"Tipo de reporte no válido: {dto.Type}"),
        };

        var filters = new Dictionary<string, object>();
        if (dto.From.HasValue)
            filters["From"] = dto.From.Value;
        if (dto.To.HasValue)
            filters["To"] = dto.To.Value;
        if (!string.IsNullOrEmpty(dto.MediatorId))
            filters["MediatorId"] = dto.MediatorId;
        if (!string.IsNullOrEmpty(dto.Status))
            filters["Status"] = dto.Status;

        var reportId = Guid.NewGuid().ToString();

        var reportData = new Dictionary<string, object>
        {
            { "Id", reportId },
            { "Type", dto.Type },
            { "GeneratedBy", userId },
            { "Filters", filters },
            { "Data", data },
            { "CreatedAt", DateTime.UtcNow },
        };

        await _fireBaseService
            .GetCollection(collectionName)
            .Document(reportId)
            .SetAsync(reportData);

        var snapshot = await _fireBaseService
            .GetCollection(collectionName)
            .Document(reportId)
            .GetSnapshotAsync();

        return MapToReport(snapshot);
    }

    /// <summary>
    /// Obtiene un reporte por su Id.
    /// </summary>
    public async Task<Report> GetByIdAsync(string reportId)
    {
        var doc = await _fireBaseService
            .GetCollection(collectionName)
            .Document(reportId)
            .GetSnapshotAsync();

        if (!doc.Exists)
            throw new Exception("El reporte no existe");

        return MapToReport(doc);
    }

    /// <summary>
    /// Lista todos los reportes generados por un usuario.
    /// </summary>
    public async Task<List<Report>> GetByUserAsync(string userId)
    {
        var snapshot = await _fireBaseService
            .GetCollection(collectionName)
            .WhereEqualTo("GeneratedBy", userId)
            .GetSnapshotAsync();

        return snapshot.Documents.Select(MapToReport).ToList();
    }

    /// <summary>
    /// Lista todos los reportes (solo mediadores/admins deberían llamar esto).
    /// </summary>
    public async Task<List<Report>> GetAllAsync()
    {
        var snapshot = await _fireBaseService.GetCollection(collectionName).GetSnapshotAsync();

        return snapshot.Documents.Select(MapToReport).ToList();
    }

    // ─── Builders privados ────────────────────────────────────────────────────

    private async Task<Dictionary<string, object>> BuildCasesReportDataAsync(GenerateReportDto dto)
    {
        var snapshot = await _fireBaseService.GetCollection("cases").GetSnapshotAsync();

        var docs = snapshot.Documents.AsEnumerable();

        if (dto.From.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") >= dto.From.Value);
        if (dto.To.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") <= dto.To.Value);
        if (!string.IsNullOrEmpty(dto.Status))
            docs = docs.Where(d => d.GetValue<string>("Status") == dto.Status);
        if (!string.IsNullOrEmpty(dto.MediatorId))
            docs = docs.Where(d => d.GetValue<string>("MediatorId") == dto.MediatorId);

        var list = docs.ToList();

        var byStatus = list.GroupBy(d => d.GetValue<string>("Status"))
            .ToDictionary(g => g.Key, g => (object)g.Count());

        return new Dictionary<string, object>
        {
            { "Total", list.Count },
            { "PorEstado", byStatus },
        };
    }

    private async Task<Dictionary<string, object>> BuildAgreementsReportDataAsync(
        GenerateReportDto dto
    )
    {
        var snapshot = await _fireBaseService.GetCollection("agreements").GetSnapshotAsync();
        var docs = snapshot.Documents.AsEnumerable();

        if (dto.From.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") >= dto.From.Value);
        if (dto.To.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") <= dto.To.Value);
        if (!string.IsNullOrEmpty(dto.MediatorId))
            docs = docs.Where(d => d.GetValue<string>("MediatorId") == dto.MediatorId);

        var list = docs.ToList();

        var formalized = list.Count(d =>
            d.ContainsField("FormalizedAt") && d.GetValue<DateTime?>("FormalizedAt") != null
        );

        return new Dictionary<string, object>
        {
            { "Total", list.Count },
            { "Formalizados", formalized },
            { "Pendientes", list.Count - formalized },
        };
    }

    private async Task<Dictionary<string, object>> BuildSessionsReportDataAsync(
        GenerateReportDto dto
    )
    {
        var snapshot = await _fireBaseService.GetCollection("sessions").GetSnapshotAsync();
        var docs = snapshot.Documents.AsEnumerable();

        if (dto.From.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") >= dto.From.Value);
        if (dto.To.HasValue)
            docs = docs.Where(d => d.GetValue<DateTime>("CreatedAt") <= dto.To.Value);
        if (!string.IsNullOrEmpty(dto.MediatorId))
            docs = docs.Where(d => d.GetValue<string>("MediatorId") == dto.MediatorId);
        if (!string.IsNullOrEmpty(dto.Status))
            docs = docs.Where(d => d.GetValue<string>("Status") == dto.Status);

        var list = docs.ToList();

        var byStatus = list.GroupBy(d => d.GetValue<string>("Status"))
            .ToDictionary(g => g.Key, g => (object)g.Count());

        return new Dictionary<string, object>
        {
            { "Total", list.Count },
            { "PorEstado", byStatus },
        };
    }

    private async Task<Dictionary<string, object>> BuildMediatorsReportDataAsync(
        GenerateReportDto dto
    )
    {
        var mediatorsSnapshot = await _fireBaseService
            .GetCollection("mediators")
            .GetSnapshotAsync();
        var casesSnapshot = await _fireBaseService.GetCollection("cases").GetSnapshotAsync();

        var casesPerMediator = casesSnapshot
            .Documents.GroupBy(d =>
                d.ContainsField("MediatorId") ? d.GetValue<string>("MediatorId")
                : d.ContainsField("MediadorId") ? d.GetValue<string>("MediadorId")
                : null
            )
            .Where(g => g.Key != null)
            .ToDictionary(g => g.Key!, g => g.Count());

        var result = mediatorsSnapshot
            .Documents.Select(m => new Dictionary<string, object>
            {
                { "MediatorId", m.Id },
                { "CasosAsignados", casesPerMediator.GetValueOrDefault(m.Id, 0) },
            })
            .ToList<object>();

        return new Dictionary<string, object>
        {
            { "TotalMediadores", mediatorsSnapshot.Count },
            { "Detalle", result },
        };
    }

    // ─── Mapper ───────────────────────────────────────────────────────────────

    private static Report MapToReport(DocumentSnapshot doc)
    {
        return new Report
        {
            Id = doc.Id,
            Type = doc.ContainsField("Type")
                ? doc.GetValue<string>("Type") ?? string.Empty
                : string.Empty,
            GeneratedBy = doc.ContainsField("GeneratedBy")
                ? doc.GetValue<string>("GeneratedBy") ?? string.Empty
                : string.Empty,
            Filters = doc.ContainsField("Filters")
                ? doc.GetValue<Dictionary<string, object>>("Filters") ?? []
                : [],
            Data = doc.ContainsField("Data")
                ? doc.GetValue<Dictionary<string, object>>("Data") ?? []
                : [],
            CreatedAt = doc.ContainsField("CreatedAt")
                ? doc.GetValue<DateTime>("CreatedAt")
                : DateTime.MinValue,
        };
    }
}
