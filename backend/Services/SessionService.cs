using Google.Cloud.Firestore;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class SessionService(FireBaseService fireBaseService)
{
    // Servicio de Firebase para conectarse a la base de datos
    private readonly FireBaseService _fireBaseService = fireBaseService;
    // Nombre de la coleccion en Firestore
    private readonly string collectionName = "sessions";

    // Crea una sesion de mediacion en Firebase (solo mediador)
    public async Task<MediationSession> CreateAsync(CreateSessionDto dto, string userId)
    {
        var caseDoc = await _fireBaseService
            .GetCollection("cases")
            .Document(dto.CaseId)
            .GetSnapshotAsync();

        if (!caseDoc.Exists)
            throw new Exception("El caso no existe");

        var mediatorSnapshot = await _fireBaseService
            .GetCollection("mediators")
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        if (mediatorSnapshot.Count == 0)
            throw new UnauthorizedAccessException("El usuario no es un mediador");

        var mediatorDoc = mediatorSnapshot.Documents[0];
        var mediatorId = mediatorDoc.Id;

        var caseMediatorId = caseDoc.ContainsField("MediatorId")
            ? caseDoc.GetValue<string>("MediatorId")
            : null;

        if (caseMediatorId != mediatorId)
            throw new UnauthorizedAccessException(
                "Solo el mediador asignado puede programar sesiones para este caso"
            );

        var caseStatus = caseDoc.GetValue<string>("Status");

        if (caseStatus != "asignado" && caseStatus != "en mediacion")
            throw new Exception("Solo se pueden programar sesiones en casos asignados o en mediacion");

        if (dto.Modality.ToLower() == "virtual" && string.IsNullOrWhiteSpace(dto.MeetingLink))
            throw new Exception("Debe ingresar un enlace para sesiones virtuales");

        var sessionId = Guid.NewGuid().ToString();

        var session = new MediationSession
        {
            Id = sessionId,
            CaseId = dto.CaseId,
            MediatorId = mediatorId,
            ScheduledDate = dto.ScheduledDate,
            ScheduledTime = dto.ScheduledTime,
            Modality = dto.Modality,
            MeetingLink = dto.MeetingLink,
            Status = "programada",
            SessionNotes = dto.SessionNotes,
            CreatedAt = DateTime.UtcNow,
        };

        var data = new Dictionary<string, object>
        {
            { "Id", session.Id },
            { "CaseId", session.CaseId },
            { "MediatorId", session.MediatorId },
            { "ScheduledDate", session.ScheduledDate },
            { "ScheduledTime", session.ScheduledTime },
            { "Modality", session.Modality },
            { "MeetingLink", session.MeetingLink ?? string.Empty },
            { "Status", session.Status },
            { "SessionNotes", session.SessionNotes ?? string.Empty },
            { "CreatedAt", session.CreatedAt },
        };

        await _fireBaseService
            .GetCollection(collectionName)
            .Document(session.Id)
            .SetAsync(data);

        if (caseStatus == "asignado")
        {
            await caseDoc.Reference.UpdateAsync(
                new Dictionary<string, object>
                {
                    { "Status", "en mediacion" },
                }
            );
        }

        return session;
    }

    // Trae las sesiones de un caso especifico
    public async Task<List<MediationSession>> GetByCaseIdAsync(string caseId)
    {
        var snapshot = await _fireBaseService
            .GetCollection(collectionName)
            .WhereEqualTo("CaseId", caseId)
            .GetSnapshotAsync();

        return snapshot.Documents.Select(MapToSession).ToList();
    }

    // Trae las sesiones del mediador que esta logueado
    public async Task<List<MediationSession>> GetByMediatorAsync(string userId)
    {
        var mediatorSnapshot = await _fireBaseService
            .GetCollection("mediators")
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        if (mediatorSnapshot.Count == 0)
            throw new UnauthorizedAccessException("El usuario no es un mediador");

        var mediatorId = mediatorSnapshot.Documents[0].Id;

        var snapshot = await _fireBaseService
            .GetCollection(collectionName)
            .WhereEqualTo("MediatorId", mediatorId)
            .GetSnapshotAsync();

        return snapshot.Documents.Select(MapToSession).ToList();
    }

    // Actualiza el estado de una sesion (realizada, reprogramada, etc.)
    public async Task<MediationSession> UpdateStatusAsync(
        string sessionId,
        string status,
        string? notes,
        string userId
    )
    {
        var sessionDoc = await _fireBaseService
            .GetCollection(collectionName)
            .Document(sessionId)
            .GetSnapshotAsync();

        if (!sessionDoc.Exists)
            throw new Exception("La sesion no existe");

        var mediatorSnapshot = await _fireBaseService
            .GetCollection("mediators")
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        if (mediatorSnapshot.Count == 0)
            throw new UnauthorizedAccessException("El usuario no es un mediador");

        var mediatorId = mediatorSnapshot.Documents[0].Id;
        var sessionMediatorId = sessionDoc.GetValue<string>("MediatorId");

        if (sessionMediatorId != mediatorId)
            throw new UnauthorizedAccessException(
                "Solo el mediador asignado puede actualizar esta sesion"
            );

        var updates = new Dictionary<string, object>
        {
            { "Status", status },
            { "SessionNotes", notes ?? string.Empty },
        };

        await sessionDoc.Reference.UpdateAsync(updates);

        var updatedDoc = await sessionDoc.Reference.GetSnapshotAsync();

        return MapToSession(updatedDoc);
    }

    // Convierte los datos de Firebase a un objeto MediationSession
    private static MediationSession MapToSession(DocumentSnapshot doc)
    {
        return new MediationSession
        {
            Id = doc.Id,
            CaseId = doc.GetValue<string>("CaseId") ?? string.Empty,
            MediatorId = doc.GetValue<string>("MediatorId") ?? string.Empty,
            ScheduledDate = doc.GetValue<DateTime>("ScheduledDate"),
            ScheduledTime = doc.GetValue<string>("ScheduledTime") ?? string.Empty,
            Modality = doc.GetValue<string>("Modality") ?? string.Empty,
            MeetingLink = doc.ContainsField("MeetingLink")
                ? doc.GetValue<string?>("MeetingLink")
                : null,
            Status = doc.GetValue<string>("Status") ?? "programada",
            SessionNotes = doc.ContainsField("SessionNotes")
                ? doc.GetValue<string?>("SessionNotes")
                : null,
            CreatedAt = doc.GetValue<DateTime>("CreatedAt"),
        };
    }
}