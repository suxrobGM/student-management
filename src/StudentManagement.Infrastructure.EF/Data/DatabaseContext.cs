using Microsoft.EntityFrameworkCore;
using StudentManagement.Infrastructure.EF.Helpers;
using StudentManagement.Infrastructure.EF.Options;

namespace StudentManagement.Infrastructure.EF.Data;

public class DatabaseContext : DbContext
{
    private readonly string _connectionString;

    public DatabaseContext(DatabaseContextOptions options)
    {
        _connectionString = options.ConnectionString ?? ConnectionStrings.LocalDB;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            DbContextHelpers.ConfigureMySql(_connectionString, options);
        }
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Student>().ToTable("students");

        builder.Entity<Student>(entity =>
        {
            entity.ToTable("students");
            entity.Property(m => m.Id)
                .ValueGeneratedNever();
        });
    }
}
