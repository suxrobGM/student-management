using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Infrastructure.EF.Data;

public static class SeedData
{
    public static void MigrateDatabase(DatabaseContext context)
    {
        context.Database.Migrate();
    }
}
