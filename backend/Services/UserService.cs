using Google.Cloud.Firestore;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class UserService
{
    private readonly FireBaseService _fireBaseService;

    public UserService(FireBaseService fireBase)
    {
        _fireBaseService = fireBase;
    }

    public async Task<User?> GetUserAsync(string id)
    {
        var doc = await _fireBaseService.GetCollection("users").Document(id).GetSnapshotAsync();

        if (!doc.Exists)
            return null;

        var data = doc.ToDictionary();

        var user = new User
        {
            Id = data["Id"].ToString()!,
            FullName = data["FullName"].ToString()!,
            Email = data["Email"].ToString()!,
            Role = data["Role"].ToString()!,
            CreatedAt = ((Timestamp)data["CreatedAt"]).ToDateTime(),
        };

        return user;
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        var snapshot = await _fireBaseService
            .GetCollection("users")
            .GetSnapshotAsync();

        var users = new List<User>();

        foreach (var doc in snapshot.Documents)
        {
            var data = doc.ToDictionary();

            users.Add(new User
            {
                Id = data["Id"].ToString()!,
                FullName = data["FullName"].ToString()!,
                Email = data["Email"].ToString()!,
                Role = data["Role"].ToString()!,
                CreatedAt = ((Timestamp)data["CreatedAt"]).ToDateTime()
            });
        }

        return users;
    }
}