using System.Runtime.InteropServices.JavaScript;

namespace EventPlatform.Domain.Common;

public class OperationResult
{
    public bool IsError { get; protected set; }
    
    public List<Error>? Errors { get; } = new();
    

    public static OperationResult Success()
    {
        return new OperationResult { IsError = false };
    }

    public void AddError(Error error)
    {
        IsError = true;
        Errors?.Add(error);
    }

}