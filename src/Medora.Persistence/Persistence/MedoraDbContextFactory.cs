using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Medora.Infrastructure.Persistence;

public sealed class MedoraDbContextFactory : IDesignTimeDbContextFactory<MedoraDbContext>
{
    public MedoraDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Medora.Api"));
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.Exists(basePath) ? basePath : Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = configuration.GetConnectionString("MedoraDb")
            ?? throw new InvalidOperationException("Connection string 'MedoraDb' is not configured.");

        var builder = new DbContextOptionsBuilder<MedoraDbContext>();
        builder.UseSqlServer(connectionString);
        return new MedoraDbContext(builder.Options);
    }
}
