using System.Reflection;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation;
using DustInTheWind.OroAvalonia.Presentation.MainArea;
using DustInTheWind.OroWpf.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia;

internal static class Setup
{
    public static void ConfigureServices(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<Navigation>();

        ISettings settings = new Settings();
        serviceCollection.AddSingleton(settings);

        ApplicationState applicationState = CreateApplicationState(settings);
        serviceCollection.AddSingleton(applicationState);

        serviceCollection.AddTransient<MainWindow>();
        serviceCollection.AddTransient<MainViewModel>();

        serviceCollection.AddTransient<ToggleNavigationCommand>();
        serviceCollection.AddTransient<SettingsCommand>();
    }

    private static ApplicationState CreateApplicationState(ISettings settings)
    {
        IEnumerable<Assembly> assemblies = LoatTemplateAssemblies();

        List<Type> templateTypes = EnumerateClockTemplates(assemblies)
            .ToList();

        ApplicationState applicationState = new()
        {
            AvailableTemplateTypes = templateTypes
        };

        if (templateTypes?.Count > 0)
        {
            Type selectedTemplateType = LoadTemplateTypeFromSettings(settings, templateTypes);

            applicationState.ClockTemplate = (ClockTemplate)Activator.CreateInstance(selectedTemplateType);
        }

        return applicationState;
    }

    private static IEnumerable<Assembly> LoatTemplateAssemblies()
    {
        yield return typeof(DefaultTemplate).Assembly;

        foreach (Assembly assembly in PluginSupport.LoatTemplateAssemblies())
            yield return assembly;
    }

    private static Type LoadTemplateTypeFromSettings(ISettings settings, List<Type> templateTypes)
    {
        string savedTemplateTypeName = settings.ClockTemplateType;

        if (!string.IsNullOrEmpty(savedTemplateTypeName))
        {
            Type savedTemplateType = templateTypes
                .FirstOrDefault(x => x.FullName == savedTemplateTypeName || x.Name == savedTemplateTypeName);

            if (savedTemplateType != null)
                return savedTemplateType;
        }

        return templateTypes.FirstOrDefault(x => x == typeof(DefaultTemplate)) ?? templateTypes.First();
    }

    private static IEnumerable<Type> EnumerateClockTemplates(IEnumerable<Assembly> assemblies)
    {
        return assemblies
            .SelectMany(x =>
            {
                try
                {
                    return x.GetTypes();
                }
                catch (ReflectionTypeLoadException)
                {
                    return [];
                }
            })
            .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(ClockTemplate)));
    }
}