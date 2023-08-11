using EventPlatform.Domain.Common;
using EventPlatform.Domain.Models;

namespace EventPlatform.Domain.Specifications;

public class RegistrationSpecification : ISpecification<User>
{
    public IReadOnlyList<Error> Errors { get; set; }

    public bool IsSatisfiedBy(User entity)
    {
        var errors = new List<Error>(Errors);
        
        if (string.IsNullOrWhiteSpace(entity.FullName))
        {
            errors.Add(new
                Error{Code = ErrorCode.ApplicationError, ErrorMessage = "FullName cannot be null or whitespace"});
        }

        if (string.IsNullOrWhiteSpace(entity.Email))
        {
            errors.Add(new
                Error{Code = ErrorCode.ApplicationError, ErrorMessage = "Email cannot be null or whitespace"});
        }

        if (string.IsNullOrWhiteSpace(entity.UserName))
        {
            errors.Add(new
                Error{Code = ErrorCode.ApplicationError, ErrorMessage = "UserName cannot be null or whitespace"});
        }
        
        return !errors.Any();
    }
}