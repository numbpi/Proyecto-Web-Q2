using Google.Cloud.Firestore;

namespace Proyecto_Web_Q2.Models;

// Aqui vamos a definir que datos tendra un mediador en firebase

[FirestoreData]
public class Mediator
{
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    [FirestoreProperty]
    public string FullName { get; set; } = string.Empty;

    [FirestoreProperty]
    public string Zone { get; set; } = string.Empty;

    // tipo de conflictos ya sea por ruido o mascotas.
    [FirestoreProperty]
    public string Specialty { get; set; } = string.Empty;

    // indica si está disponible para recibir casos
    [FirestoreProperty]
    public bool IsAvailable { get; set; }

    //indica si el mediador sigue activo en el sistema
    [FirestoreProperty]
    public bool IsActive { get; set; } = true;

    //cantidad de casos activos asignados
    [FirestoreProperty]
    public int ActiveCases { get; set; }

    // referencia al Id del usuario en la coleccion "users"
    [FirestoreProperty]
    public string? UserId { get; set; }

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; }
}
