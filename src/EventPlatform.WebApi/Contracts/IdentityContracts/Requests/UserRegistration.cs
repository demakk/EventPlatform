namespace EventPlatform.WebApi.Contracts.IdentityContracts.Requests;

public class UserRegistration
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string? Password { get; set; }
    public string? Phone { get; set; }
}