namespace Proyecto_Web_Q2.Models;

public class LabNote
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Title { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; } = 0;
    public bool IsPublic { get; set; } = false;
    public string Tags { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string UserId { get; set; } = string.Empty;
}