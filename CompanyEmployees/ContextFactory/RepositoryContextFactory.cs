using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory;

// allows for an instance of the context
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build(); // appsettings config

        // GetConnectionString() shorthand for GetSection("ConnectionStrings")[name]
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlServer(configuration.GetConnectionString("sqlConnection"), // using config
                b => b.MigrationsAssembly("CompanyEmployees")); // migrations

        return new RepositoryContext(builder.Options); // includes sql connection
    }
}