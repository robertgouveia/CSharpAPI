using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class RepositoryContext : DbContext
{
    // allows for configuration
    public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }
    
    public DbSet<Employee>? Employees { get; set; }
    public DbSet<Company>? Companies { get; set; }
}