namespace EventPlatform.Domain.Common;

public class OperationResultT<TValue> : OperationResult
{
    public TValue? Value{ get; private set; }

    public void SetTValue(TValue value)
    {
        Value = value;
    }
}