namespace Proyecto_Web_Q2.Models;

public class PasswordReset
{
    public string Token { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; } = DateTime.UtcNow;
    public bool Used { get; set; } = false;
}
