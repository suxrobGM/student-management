using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Infrastructure.EF.Data;
using StudentManagement.Infrastructure.EF.Options;

namespace StudentManagement.Infrastructure.EF;

public static class Registrar
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringSection = "LocalDB")
    {
        var connectionString = configuration.GetConnectionString(connectionStringSection);
        services.AddSingleton(new DatabaseContextOptions { ConnectionString = connectionString });
        services.AddDbContext<DatabaseContext>();
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        return services;
    }
}