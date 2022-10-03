using StudentManagement.Infrastructure;

namespace StudentManagement.App;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        Services = ConfigureServices();
    }
    
    public new static App Current => (App)Application.Current;
    
    public IServiceProvider Services { get; }
    
    private static IServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddScoped<MainWindowViewModel>();
        services.AddTransient<AddEditWindowViewModel>();
        services.AddInfrastructureLayer();
        return services.BuildServiceProvider();
    }
}