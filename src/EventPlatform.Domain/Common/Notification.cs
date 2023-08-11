namespace EventPlatform.Domain.Common;

public class Notification
{
    private readonly List<Error> _errors = new List<Error>();

    public IReadOnlyList<Error> Errors => _errors;

    public bool HasErrors => _errors.Any();

    public void AddError(Error error)
    {
        _errors.Add(error);
    }
}