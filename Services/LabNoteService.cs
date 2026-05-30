using Google.Cloud.Firestore;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class LabNoteService
{
    private readonly FireBaseService _fireBaseService;
    private const string CollectionName = "labNotes";

    public LabNoteService(FireBaseService fireBaseService)
    {
        _fireBaseService = fireBaseService;
    }

    public async Task<LabNote> CreateAsync(CreateLabNoteDto dto, string userId)
    {
        ValidateLabNote(dto);

        var note = new LabNote
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Observation = dto.Observation,
            Category = dto.Category,
            Priority = dto.Priority,
            IsPublic = dto.IsPublic,
            Tags = dto.Tags,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        var collection = _fireBaseService.GetCollection(CollectionName);
        await collection.Document(note.Id).SetAsync(note);

        return note;
    }

    public async Task<List<LabNote>> GetByUserAsync(string userId)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);

        var snapshot = await collection
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        var notes = new List<LabNote>();

        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                notes.Add(document.ConvertTo<LabNote>());
            }
        }

        return notes;
    }

    public async Task<bool> DeleteAsync(string id, string userId)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var documentRef = collection.Document(id);
        var snapshot = await documentRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new KeyNotFoundException("La nota no existe.");
        }

        var note = snapshot.ConvertTo<LabNote>();

        if (note.UserId != userId)
        {
            throw new UnauthorizedAccessException("Ojo, No tienes el permiso para eliminar esta nota.");
        }

        await documentRef.DeleteAsync();
        return true;
    }

    private void ValidateLabNote(CreateLabNoteDto dto)
    {
        var validCategories = new[] { "Quimica", "Biologia", "Fisica", "Otro" };

        if (!validCategories.Contains(dto.Category))
        {
            throw new ArgumentException("La categoría solo puede ser: Quimica, Biologia, Fisica u Otro.");
        }

        if (dto.Priority < 1 || dto.Priority > 3)
        {
            throw new ArgumentException("La prioridad solo puede ser 1, 2 o 3.");
        }
    }
}