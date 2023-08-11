using EventPlatform.Domain.Common;

namespace EventPlatform.Domain.Specifications;

public interface ISpecification<T>
{
    public IReadOnlyList<Error> Errors { get; set; }
    bool IsSatisfiedBy(T entity);
}