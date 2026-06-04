using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class NotificationService
{
    private readonly FireBaseService _fireBaseService;
    private const string CollectionName = "notifications";

    public NotificationService(FireBaseService fireBaseService)
    {
        _fireBaseService = fireBaseService;
    }

    // Aqui crea una notificacion nueva en firebase
    public async Task<Notification> CreateAsync(CreateNotificationDto dto)
    {
        ValidateType(dto.Type);

        var notification = new Notification
        {
            Id = Guid.NewGuid().ToString(),
            UserId = dto.UserId,
            Title = dto.Title,
            Message = dto.Message,
            Type = dto.Type,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        var collection = _fireBaseService.GetCollection(CollectionName);
        await collection.Document(notification.Id).SetAsync(notification);

        return notification;
    }

    // esto nos sirve para buscar las notificaciones de usuario especifico 
    public async Task<List<Notification>> GetByUserAsync(string userId)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);

        var snapshot = await collection
            .WhereEqualTo("UserId", userId)
            .GetSnapshotAsync();

        var notifications = new List<Notification>();

        foreach (var document in snapshot.Documents)
        {
            if (document.Exists)
            {
                notifications.Add(document.ConvertTo<Notification>());
            }
        }

        return notifications;
    }

    // Marca una notificacion como leida 
    public async Task<bool> MarkAsReadAsync(string id, string userId)
    {
        var collection = _fireBaseService.GetCollection(CollectionName);
        var documentRef = collection.Document(id);
        var snapshot = await documentRef.GetSnapshotAsync();

        //Valida que la notificacion le pertenezca al usuario y si no lanza error
        if (!snapshot.Exists)
        {
            throw new KeyNotFoundException("La notificación no existe");
        }

        var notification = snapshot.ConvertTo<Notification>();

        if (notification.UserId != userId)
        {
            throw new UnauthorizedAccessException("No tienes permiso para modificar esta notificacion");
        }

        await documentRef.UpdateAsync("IsRead", true);

        return true;
    }

    //Basicamente solo valida el tipo de notificacion
    private void ValidateType(string type)
    {
        var validTypes = new[] { "Case", "Session", "Agreement", "General" };

        if (!validTypes.Contains(type))
        {
            throw new ArgumentException("El tipo solo puede ser uno de estos: Case, Session, Agreement o General.");
        }
    }
}