namespace EventPlatform.WebApi;

public static class ApiRoutes
{
    public const string BaseRoute = "api/[controller]";
    
    public static class Identity
    {
        public const string Id = "{id}";
        public const string Register = "Register";
        public const string Login = "Login";
    }
}