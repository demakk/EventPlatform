namespace EventPlatform.WebApi.Contracts.IdentityContracts.Requests;

public class UserLogin
{
    public string Email { get; set; }
    public string Password { get; set; }
}