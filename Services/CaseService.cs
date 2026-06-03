using Google.Cloud.Firestore;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class CaseService(FireBaseService fb)
{
    private readonly FireBaseService _firebaseService = fb;
    private readonly string collectionName = "cases";

    public async Task<ConflictCase> CreateAsync(CreateCaseDto dto, string userId)
    {
        DocumentSnapshot userDoc = await _firebaseService
            .GetCollection("users")
            .Document(userId)
            .GetSnapshotAsync();

        if (!userDoc.Exists)
            throw new Exception("Usuario no encontrado");

        string reporterName = userDoc.GetValue<string>("FullName");

        DocumentSnapshot respondentDoc = await _firebaseService
            .GetCollection("users")
            .Document(dto.RespondentId)
            .GetSnapshotAsync();

        if (!respondentDoc.Exists)
            throw new Exception("Usuario denunciado no encontrado");

        string respondentName = respondentDoc.GetValue<string>("FullName");

        var collection = _firebaseService.GetCollection(collectionName);

        var activeStatuses = new[] { "nuevo", "asignado", "en mediacion" };

        var existing = await collection
            .WhereEqualTo("ReporterId", userId)
            .WhereEqualTo("RespondentId", dto.RespondentId)
            .WhereIn("Status", activeStatuses)
            .GetSnapshotAsync();

        if (existing.Count > 0)
            throw new Exception("Ya existe un caso activo entre estas partes");

        var caso = new ConflictCase
        {
            Id = Guid.NewGuid().ToString(),
            ReporterId = userId,
            ReporterName = reporterName,
            RespondentId = dto.RespondentId,
            RespondentName = respondentName,
            ConflictType = dto.ConflictType,
            Status = "nuevo",
            Description = dto.Description,
            Address = dto.Address,
            CreatedAt = DateTime.UtcNow,
            EvidenceUrls = [],
        };

        var data = new Dictionary<string, object>
        {
            { "Id", caso.Id },
            { "ReporterId", caso.ReporterId },
            { "ReporterName", caso.ReporterName },
            { "RespondentId", caso.RespondentId },
            { "RespondentName", caso.RespondentName },
            { "ConflictType", caso.ConflictType },
            { "Status", caso.Status },
            { "Description", caso.Description },
            { "Address", caso.Address },
            { "CreatedAt", caso.CreatedAt },
            { "EvidenceUrls", caso.EvidenceUrls },
        };

        await collection.Document(caso.Id).SetAsync(data);

        return caso;
    }

    public async Task<List<ConflictCase>> GetCasesAsync(string userId, string role)
    {
        var collection = _firebaseService.GetCollection(collectionName);

        return role switch
        {
            "admin" => (await collection.GetSnapshotAsync()).Documents.Select(MapToCase).ToList(),

            "mediator" => (await collection.WhereEqualTo("MediadorId", userId).GetSnapshotAsync())
                .Documents.Select(MapToCase)
                .ToList(),

            _ => await GetMyCaseAsync(userId), //Puede ser user: Reporter + respondent
        };
    }

    public async Task<List<ConflictCase>> GetMyCaseAsync(string userId)
    {
        var collection = _firebaseService.GetCollection(collectionName);

        var reporterTask = collection.WhereEqualTo("ReporterId", userId).GetSnapshotAsync();

        var respondentTask = collection.WhereEqualTo("RespondentId", userId).GetSnapshotAsync();

        var mediatorTask = collection.WhereEqualTo("MediadorId", userId).GetSnapshotAsync();

        await Task.WhenAll(reporterTask, respondentTask, mediatorTask);

        var casos = new Dictionary<string, ConflictCase>();

        foreach (var doc in reporterTask.Result.Documents)
            if (doc.Exists)
                casos[doc.Id] = MapToCase(doc);

        foreach (var doc in respondentTask.Result.Documents)
            if (doc.Exists && !casos.ContainsKey(doc.Id))
                casos[doc.Id] = MapToCase(doc);

        foreach (var doc in mediatorTask.Result.Documents)
            if (doc.Exists && !casos.ContainsKey(doc.Id))
                casos[doc.Id] = MapToCase(doc);

        return casos.Values.ToList();
    }

    public async Task<ConflictCase?> GetByIdAsync(string caseId)
    {
        var doc = await _firebaseService
            .GetCollection(collectionName)
            .Document(caseId)
            .GetSnapshotAsync();

        return doc.Exists ? MapToCase(doc) : null;
    }

    public async Task<ConflictCase> AssignMediatorAsync(string caseId, string mediatorId)
    {
        var doc = await _firebaseService
            .GetCollection(collectionName)
            .Document(caseId)
            .GetSnapshotAsync();

        if (!doc.Exists)
            throw new Exception("El caso n fue encontrado");

        var caso = MapToCase(doc);

        if (caso.Status != "nuevo")
            throw new Exception(
                "Solo se puede asginar un mediador si el caso esta en estado Nuevo"
            );

        var updates = new Dictionary<string, object>
        {
            { "MediadorId", mediatorId },
            { "Status", "asignado" },
            { "AssignedAt", DateTime.UtcNow },
        };

        await doc.Reference.UpdateAsync(updates);

        caso.MediadorId = mediatorId;
        caso.Status = "asignado";
        caso.AssignedAt = DateTime.UtcNow;

        return caso;
    }

    public async Task<ConflictCase> UpdateStatusAsync(
        string caseId,
        string newStatus,
        string userId
    )
    {
        var doc = await _firebaseService
            .GetCollection(collectionName)
            .Document(caseId)
            .GetSnapshotAsync();

        if (!doc.Exists)
            throw new Exception("Caso no encontrado");

        var caso = MapToCase(doc);

        // buscar el UserId del mediador asignado en la coleccion "mediators"
        var mediatorDoc = await _firebaseService
            .GetCollection("mediators")
            .Document(caso.MediadorId)
            .GetSnapshotAsync();

        string? mediatorUserId = mediatorDoc.Exists && mediatorDoc.ContainsField("UserId")
            ? mediatorDoc.GetValue<string?>("UserId")
            : null;

        if (mediatorUserId != userId)
            throw new UnauthorizedAccessException(
                "Solo el mediador asignado puede cambiar el estado del caso"
            );

        bool transicionValida =
            (caso.Status == "asignado" && newStatus == "en mediacion") ||
            (caso.Status == "en mediacion" && newStatus == "resuelto") ||
            (caso.Status == "en mediacion" && newStatus == "cerrado sin acuerdo");

        if (!transicionValida)
            throw new Exception($"No se puede pasar de '{caso.Status}' a '{newStatus}'");

        var updates = new Dictionary<string, object> { { "Status", newStatus } };

        if (newStatus is "resuelto" or "cerrado sin acuerdo")
            updates["ClosedAt"] = DateTime.UtcNow;

        await doc.Reference.UpdateAsync(updates);

        caso.Status = newStatus;

        if (newStatus is "resuelto" or "cerrado sin acuerdo")
            caso.ClosedAt = DateTime.UtcNow;

        return caso;
    }

    // Metodos para solo usarse aca
    private static ConflictCase MapToCase(DocumentSnapshot doc)
    {
        return new ConflictCase
        {
            Id = doc.Id,
            ReporterId = doc.GetValue<string>("ReporterId") ?? string.Empty,
            ReporterName = doc.GetValue<string>("ReporterName") ?? string.Empty,
            RespondentId = doc.GetValue<string>("RespondentId") ?? string.Empty,
            RespondentName = doc.GetValue<string>("RespondentName") ?? string.Empty,
            ConflictType = doc.GetValue<string>("ConflictType") ?? string.Empty,
            Description = doc.GetValue<string>("Description") ?? string.Empty,
            Address = doc.GetValue<string>("Address") ?? string.Empty,
            Status = doc.GetValue<string>("Status") ?? "nuevo",
            MediadorId = doc.ContainsField("MediadorId")
                ? doc.GetValue<string>("MediadorId")
                : null,
            EvidenceUrls = doc.GetValue<List<string>>("EvidenceUrls") ?? [],
            CreatedAt = doc.GetValue<DateTime>("CreatedAt"),
            AssignedAt = doc.ContainsField("AssignedAt")
                ? doc.GetValue<DateTime?>("AssignedAt")
                : null,
            ClosedAt = doc.ContainsField("ClosedAt") ? doc.GetValue<DateTime?>("ClosedAt") : null,
        };
    }
}
