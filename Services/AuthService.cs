using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Google.Cloud.Firestore;
using Microsoft.IdentityModel.Tokens;
using Proyecto_Web_Q2.DTOs;
using Proyecto_Web_Q2.Models;

namespace Proyecto_Web_Q2.Services;

public class AuthService
{
    private readonly FireBaseService _firebaseService;
    private readonly IConfiguration _configuration;

    public AuthService(FireBaseService firebaseService, IConfiguration configuration)
    {
        _firebaseService = firebaseService;
        _configuration = configuration;
    }

    public async Task<User> Register(RegisterDto dto)
    {
        var collection = _firebaseService.GetCollection("users");
        var existing = await collection.WhereEqualTo("Email", dto.Email).GetSnapshotAsync();

        if (existing.Count > 0)
            throw new Exception("Ya existe un usuario con ese correo");

        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = HashPasword(dto.Password),
            Role = "user",
            CreatedAt = DateTime.UtcNow,
        };

        await collection
            .Document(user.Id)
            .SetAsync(
                new Dictionary<string, object>
                {
                    { "Id", user.Id },
                    { "FullName", user.FullName },
                    { "Email", user.Email },
                    { "PasswordHash", user.PasswordHash },
                    { "Role", user.Role },
                    { "CreatedAt", user.CreatedAt },
                }
            );
        return user;
    }

    public async Task<string> Login(LoginDto dto)
    {
        var collection = _firebaseService.GetCollection("users");
        var snapshot = await collection.WhereEqualTo("Email", dto.Email).GetSnapshotAsync();

        if (snapshot.Count == 0)
            throw new Exception("No existe ningun usario con esa credencial");

        var doc = snapshot.Documents[0];
        var data = doc.ToDictionary();

        var user = new User
        {
            Id = data["Id"].ToString()!,
            FullName = data["FullName"].ToString()!,
            Email = data["Email"].ToString()!,
            PasswordHash = data["PasswordHash"].ToString()!,
            Role = data["Role"].ToString()!,
            CreatedAt = ((Timestamp)data["CreatedAt"]).ToDateTime(),
        };

        if (!VerifyPassword(dto.Password, user.PasswordHash))
            throw new Exception("La contraseña es incorrecta, intentalo de nuevo");

        return GenerateToken(user);
    }

    // METODOS QUE NOS DIO EL INGENIERO

    private string GenerateToken(User user)
    {
        // El token lleva cierta informacion, Id, Email y Role del usuario que hizo login
        // Para proteccion de los endpoints, se sabe quien los esta llamando
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"], //Quien lo genera, nuestro token lo genera la app
            audience: _configuration["Jwt:Audience"], // Para quien lo genera, clientes / front-end
            claims: claims, // Estos son los datos del usuario
            expires: DateTime.UtcNow.AddHours(8), //Tiempo de vida del token
            signingCredentials: creds // Firma de seguridad
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static bool VerifyPassword(string dtoPassword, string userPasswordHash)
    {
        return HashPasword(dtoPassword) == userPasswordHash;
    }

    // Para encriptar la constraseña
    private static string HashPasword(string password)
    {
        // SHA256 - tipo de encriptacion
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }
}

