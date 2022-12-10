using Microsoft.Extensions.Configuration;
using StudentManagement.Infrastructure.EF;
using System.IO;

namespace StudentManagement.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Configuration = BuildConfiguration();
        Services = ConfigureServices();
    }
    
    public new static App Current => (App)Application.Current;
    
    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }
    
    private IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();


        services.AddScoped<MainWindowViewModel>();
        services.AddTransient<AddEditWindowViewModel>();
        services.AddInfrastructureLayer(Configuration, "RemoteDB");
        return services.BuildServiceProvider();
    }

    private IConfiguration BuildConfiguration()
    {
        var builder = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        return builder.Build();
    }
}