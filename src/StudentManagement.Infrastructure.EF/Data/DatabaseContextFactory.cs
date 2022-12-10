using Microsoft.EntityFrameworkCore.Design;
using StudentManagement.Infrastructure.EF.Options;

namespace StudentManagement.Infrastructure.EF.Data;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        return new DatabaseContext(new DatabaseContextOptions());
    }
}
