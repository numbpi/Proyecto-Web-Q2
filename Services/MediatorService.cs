using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class MediatorService
{
    private readonly FireBaseService _fireBaseService;
    private const string CollectionName = "mediators";

    public MediatorService(FireBaseService fireBaseService)
    {
        _fireBaseService = fireBaseService;
    }

    public async Task<Mediator> CreateAsync(CreateMediatorDto dto)
    {
        var mediator = new Mediator
        {
            Id = Guid.NewGuid().ToString(),
            FullName = dto.FullName,
            Zone = dto.Zone,
            Specialty = dto.Specialty,
            IsAvailable = dto.IsAvailable,
            IsActive = true,
            ActiveCases = 0,
            CreatedAt = DateTime.UtcNow
        };

        var collection = _fireBaseService.GetCollection(CollectionName);
        await collection.Document(mediator.Id).SetAsync(mediator);

        return mediator;
    }

    public async Task<List<Mediator>> GetAllAsync()
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var snapshot = await collection.GetSnapshotAsync();

        var mediators = new List<Mediator>();

        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                mediators.Add(document.ConvertTo<Mediator>());
            }
        }

        return mediators;
    }

    public async Task<Mediator> GetByIdAsync(string id)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var document = await collection.Document(id).GetSnapshotAsync();

        if (!document.Exists)
        {
            throw new KeyNotFoundException("El mediador no existe.");
        }

        return document.ConvertTo<Mediator>();
    }

    public async Task<Mediator> UpdateAsync(string id, UpdateMediatorDto dto)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var documentRef = collection.Document(id);
        var snapshot = await documentRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new KeyNotFoundException("El mediador no existe.");
        }

        var mediator = snapshot.ConvertTo<Mediator>();

        mediator.FullName = dto.FullName;
        mediator.Zone = dto.Zone;
        mediator.Specialty = dto.Specialty;
        mediator.IsAvailable = dto.IsAvailable;
        mediator.IsActive = dto.IsActive;

        await documentRef.SetAsync(mediator);

        return mediator;
    }

    public async Task<bool> DeactivateAsync(string id)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var documentRef = collection.Document(id);
        var snapshot = await documentRef.GetSnapshotAsync();

        if (!snapshot.Exists)
        {
            throw new KeyNotFoundException("El mediador no existe.");
        }

        await documentRef.UpdateAsync("IsActive", false);

        return true;
    }
}