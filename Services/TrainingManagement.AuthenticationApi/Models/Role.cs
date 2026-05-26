namespace TrainingManagement.AuthenticationApi.Models;

/// <summary>
/// Role model for authorization
/// </summary>
public class Role
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}
