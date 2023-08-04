namespace EventPlatform.Domain.Models;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}