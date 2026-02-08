using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.MainArea;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia;

internal static class Setup
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Navigation>();

        serviceCollection.AddSingleton<ISettings, Settings>();

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();

        serviceCollection.AddTransient<ToggleNavigationCommand>();
    }
}