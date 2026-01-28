using System;
using Avalonia;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia;

internal sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        IServiceProvider serviceProvider = ConfigureServices();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static IServiceProvider ConfigureServices()
    {
        ServiceCollection serviceCollection = new();
        Setup.ConfigureServices(serviceCollection);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        
        Ioc.Default.ConfigureServices(serviceProvider);

        return serviceProvider;
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}
