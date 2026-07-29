namespace WebApplication1.Models;

public class AdminEntity
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
}