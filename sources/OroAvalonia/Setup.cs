using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.ViewModels;
using DustInTheWind.OroAvalonia.Views;
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