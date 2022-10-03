using Microsoft.Extensions.DependencyInjection;
using StudentManagement.Infrastructure.Options;

namespace StudentManagement.Infrastructure;

public static class Registrar
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddSingleton(new RepositoryOptions());
        services.AddScoped<IRepository<Student>, StudentRepository>();
        return services;
    }
}