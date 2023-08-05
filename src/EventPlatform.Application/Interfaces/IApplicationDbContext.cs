using System.Data;

namespace EventPlatform.Application.Interfaces;

public interface IApplicationDbContext
{
    public IDbConnection CreateConnection();
}