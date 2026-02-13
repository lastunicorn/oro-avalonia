using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using CommunityToolkit.Mvvm.DependencyInjection;
using DustInTheWind.OroAvalonia.Infrastructure.Jobs;
using DustInTheWind.OroAvalonia.Presentation.MainArea;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();

            desktop.MainWindow = CreateMainWindow();
            
            CreateAndStartJobs();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static MainWindow CreateMainWindow()
    {
        MainWindow mainWindow = Ioc.Default.GetService<MainWindow>();
        mainWindow.DataContext = Ioc.Default.GetService<MainViewModel>();

        return mainWindow;
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        DataAnnotationsValidationPlugin[] dataValidationPluginsToRemove = BindingPlugins.DataValidators
            .OfType<DataAnnotationsValidationPlugin>()
            .ToArray();

        // remove each entry found
        foreach (DataAnnotationsValidationPlugin plugin in dataValidationPluginsToRemove)
            BindingPlugins.DataValidators.Remove(plugin);
    }

    private void CreateAndStartJobs()
    {
        JobEngine jobEngine = Ioc.Default.GetService<JobEngine>();

        IEnumerable<IJob> jobs = Ioc.Default.GetServices<IJob>();
        jobEngine.AddRange(jobs);

        jobEngine.Start();
    }
}