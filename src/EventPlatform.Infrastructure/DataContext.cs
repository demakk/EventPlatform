using System.Data;
using EventPlatform.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EventPlatform.Infrastructure;

public class DataContext : IApplicationDbContext
{
    private readonly string _connectionString;
    public DataContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default");
    }

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}