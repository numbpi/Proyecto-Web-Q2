using Google.Cloud.Firestore;

namespace Proyecto_Web_Q2.Models;

[FirestoreData]
public class Notification
{
    //este es el identificador unico del usuario
    [FirestoreProperty]
    public string Id { get; set; } = string.Empty;

    // este es el usuario que recive la notificacion
    [FirestoreProperty]
    public string UserId { get; set; } = string.Empty;

    // el titulo del mensaje
    [FirestoreProperty]
    public string Title { get; set; } = string.Empty;

    //El mensaje
    [FirestoreProperty]
    public string Message { get; set; } = string.Empty;

    // aqui va el tipo ya sea un caso una sesion o un Agreement
    [FirestoreProperty]
    public string Type { get; set; } = string.Empty;

    // verifica si ya fue leida
    [FirestoreProperty]
    public bool IsRead { get; set; }

    [FirestoreProperty]
    public DateTime CreatedAt { get; set; }
}
