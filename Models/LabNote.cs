<<<<<<< HEAD
namespace Proyecto_Web_Q2.Models;

public class LabNote
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsPublic { get; set; }
    public string Tags { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
=======
﻿namespace Proyecto_Web_Q2.Models;

public class LabNote
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); 
    public string Title { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int Priority { get; set; } = 0;
    public bool IsPublic { get; set; } = false;
    public string Tags { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
>>>>>>> 42b05774d045aad39a00d0093ff159e5de1680a6
    public string UserId { get; set; } = string.Empty;
}