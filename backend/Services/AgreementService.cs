using Google.Cloud.Firestore;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class AgreementService(FireBaseService fireBase)
{
    // Servicio de Firebase para conectarse a la base de datos
    private readonly FireBaseService _fireBaseService = fireBase;
    // Nombre de la coleccion en Firestore
    private readonly string collectionName = "agreements";

    // Crea un acuerdo nuevo en Firebase (solo mediador)
    public async Task<Agreement> CreateAsync(CreateAgreementDto dto, string userId)
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

        var casoStatus = caseDoc.GetValue<string>("Status");

        if (!casoStatus.Equals("en mediacion"))
            throw new Exception("El caso se debe encontrar en mediacion para crear el acuerdo");

        var existingAgreement = await _fireBaseService
            .GetCollection(collectionName)
            .WhereEqualTo("CaseId", dto.CaseId)
            .GetSnapshotAsync();

        if (existingAgreement.Count > 0)
            throw new Exception("Ya existe un acuerdo para este caso");

        var mediatorDoc = mediatorSnapshot.Documents[0];
        var mediatorId = mediatorDoc.Id;

        var agreementId = Guid.NewGuid().ToString();

        var agreementPoints = dto
            .Points.Select(p => new Dictionary<string, object>
            {
                { "Description", p.Description },
                { "Deadline", p.Deadline },
                { "ComplianceStatus", "pendiente" },
            })
            .ToList<object>();

        var data = new Dictionary<string, object>
        {
            { "Id", agreementId },
            { "CaseId", dto.CaseId },
            { "MediatorId", mediatorId },
            { "Points", agreementPoints },
            { "ConfirmedByReporter", false },
            { "ConfirmedByRespondent", false },
            { "FormalizedAt", null! },
            { "CreatedAt", DateTime.UtcNow },
        };

        await _fireBaseService.GetCollection(collectionName).Document(agreementId).SetAsync(data);

        return MapToAgreement(
            await _fireBaseService
                .GetCollection(collectionName)
                .Document(agreementId)
                .GetSnapshotAsync()
        );
    }

    // Confirma o rechaza un acuerdo. Si ambas partes confirman, se formaliza automaticamente
    public async Task<Agreement> ConfirmAsync(ConfirmAgreementDto dto, string userId)
    {
        var agreementDoc = await _fireBaseService
            .GetCollection(collectionName)
            .Document(dto.AgreementId)
            .GetSnapshotAsync();

        if (!agreementDoc.Exists)
            throw new Exception("El acuerdo no existe en este caso");

        if (agreementDoc.ContainsField("FormalizedAt"))
        {
            var formalizedAt = agreementDoc.GetValue<DateTime?>("FormalizedAt");
            if (formalizedAt != null)
                throw new Exception("El acuerdo ya esta formalizado");
        }

        var caseId = agreementDoc.GetValue<string>("CaseId");
        var caseDoc = await _fireBaseService
            .GetCollection("cases")
            .Document(caseId)
            .GetSnapshotAsync();

        var reporterId = caseDoc.GetValue<string>("ReporterId");
        var respondentId = caseDoc.GetValue<string>("RespondentId");

        if (userId != reporterId && userId != respondentId)
            throw new UnauthorizedAccessException(
                "Solo las personas del caso pueden confirmar el acuerdo"
            );

        var updates = new Dictionary<string, object>();

        if (userId == reporterId)
            updates["ConfirmedByReporter"] = true;
        else
            updates["ConfirmedByRespondent"] = true;

        await agreementDoc.Reference.UpdateAsync(updates);

        var updatedDoc = await agreementDoc.Reference.GetSnapshotAsync();
        var confirmedReporter = updatedDoc.GetValue<bool>("ConfirmedByReporter");
        var confirmedRespondent = updatedDoc.GetValue<bool>("ConfirmedByRespondent");

        if (confirmedReporter && confirmedRespondent)
        {
            var formalizeUpdates = new Dictionary<string, object>
            {
                { "FormalizedAt", DateTime.UtcNow },
            };
            await agreementDoc.Reference.UpdateAsync(formalizeUpdates);

            var caseUpdates = new Dictionary<string, object>
            {
                { "Status", "resuelto" },
                { "ClosedAt", DateTime.UtcNow },
            };
            await caseDoc.Reference.UpdateAsync(caseUpdates);
        }

        // Retornar el agreement actualizado
        var finalDoc = await agreementDoc.Reference.GetSnapshotAsync();
        return MapToAgreement(finalDoc);
    }

    // Busca el acuerdo de un caso por su ID
    public async Task<Agreement?> GetByCaseIdAsync(string caseId)
    {
        var snapshot = await _fireBaseService
            .GetCollection(collectionName)
            .WhereEqualTo("CaseId", caseId)
            .GetSnapshotAsync();

        if (snapshot.Count == 0)
            return null;

        return MapToAgreement(snapshot.Documents[0]);
    }

    // Reporta si un punto del acuerdo se cumplio o no (solo las partes del caso)
    public async Task<Agreement> ReportComplianceAsync(
        string agreementId,
        int pointIndex,
        string complianceStatus,
        string userId
    )
    {
        var agreementDoc = await _fireBaseService
            .GetCollection(collectionName)
            .Document(agreementId)
            .GetSnapshotAsync();

        if (!agreementDoc.Exists)
            throw new Exception("El acuerdo no existe");

        if (
            !agreementDoc.ContainsField("FormalizedAt")
            || agreementDoc.GetValue<DateTime?>("FormalizedAt") == null
        )
            throw new Exception("El acuerdo aún no está formalizado");

        var caseId = agreementDoc.GetValue<string>("CaseId");
        var caseDoc = await _fireBaseService
            .GetCollection("cases")
            .Document(caseId)
            .GetSnapshotAsync();

        var reporterId = caseDoc.GetValue<string>("ReporterId");
        var respondentId = caseDoc.GetValue<string>("RespondentId");

        if (userId != reporterId && userId != respondentId)
            throw new UnauthorizedAccessException(
                "Solo las partes del caso pueden reportar cumplimiento"
            );

        var points = agreementDoc.GetValue<List<object>>("Points");

        if (pointIndex < 0 || pointIndex >= points.Count)
            throw new Exception("Índice de punto inválido");

        var pointDict = (Dictionary<string, object>)points[pointIndex];
        pointDict["ComplianceStatus"] = complianceStatus;

        await agreementDoc.Reference.UpdateAsync("Points", points);

        var updatedDoc = await agreementDoc.Reference.GetSnapshotAsync();
        return MapToAgreement(updatedDoc);
    }

    // Convierte los datos de Firebase a un objeto Agreement
    private static Agreement MapToAgreement(DocumentSnapshot doc)
    {
        return new Agreement
        {
            Id = doc.Id,
            CaseId = doc.GetValue<string>("CaseId") ?? string.Empty,
            MediatorId = doc.GetValue<string>("MediatorId") ?? string.Empty,
            Points = doc.ContainsField("Points")
                ? (
                    doc.GetValue<List<object>>("Points")
                        ?.Select(p =>
                        {
                            var dict = (Dictionary<string, object>)p;
                            return new AgreementPoint
                            {
                                Description =
                                    dict.GetValueOrDefault("Description")?.ToString() ?? "",
                                Deadline = dict.ContainsKey("Deadline")
                                    ? ((Timestamp)dict["Deadline"]).ToDateTime()
                                    : DateTime.UtcNow,
                                ComplianceStatus =
                                    dict.GetValueOrDefault("ComplianceStatus")?.ToString()
                                    ?? "pendiente",
                            };
                        })
                        .ToList()
                    ?? []
                )
                : [],
            ConfirmedByReporter = doc.GetValue<bool>("ConfirmedByReporter"),
            ConfirmedByRespondent = doc.GetValue<bool>("ConfirmedByRespondent"),
            FormalizedAt = doc.ContainsField("FormalizedAt")
                ? doc.GetValue<DateTime?>("FormalizedAt")
                : null,
            CreatedAt = doc.GetValue<DateTime>("CreatedAt"),
        };
    }
}
