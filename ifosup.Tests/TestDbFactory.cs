using System.Data.Common;
using ifosup.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ifosup.Tests;

public sealed class TestDbFactory : IDisposable
{
    private readonly DbConnection _connection;

    public TestDbFactory()
    {
        _connection = new SqliteConnection("Data Source=:memory:");
        _connection.Open();
    }

    public ApplicationDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlite(_connection)
            .EnableSensitiveDataLogging()
            .Options;

        var db = new ApplicationDbContext(options);
        db.Database.EnsureCreated();
        return db;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}
