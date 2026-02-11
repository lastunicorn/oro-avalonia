using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia.PluginModel;

public static class PluginPoolSetup
{
    public static void AddPluginPool<T>(this IServiceCollection serviceCollection, Action<PluginPoolOptions> options)
        where T : class
    {
        PluginPoolOptions pluginPoolOptions = new();
        options(pluginPoolOptions);

        serviceCollection.AddSingleton(services =>
        {
            string directoryPath = (Path.IsPathRooted(pluginPoolOptions.DirectoryPath))
                ? pluginPoolOptions.DirectoryPath
                : Path.Combine(AppContext.BaseDirectory, pluginPoolOptions.DirectoryPath);

            IPluginFactory<T> pluginFactory = services.GetService<IPluginFactory<T>>();

            PluginPool<T> pluginPool = new(pluginFactory);

            IEnumerable<Assembly> assemblies = LoadAssemblies(directoryPath).ToList();

            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly.GetTypes()
                    .Where(x => x.IsClass && !x.IsAbstract && x.IsSubclassOf(typeof(T)));

                foreach (Type type in types)
                    pluginPool.Add(type);
            }

            return pluginPool;
        });
    }

    private static IEnumerable<Assembly> LoadAssemblies(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            yield break;

        string[] filePaths = Directory.GetFiles(directoryPath, "*.dll", SearchOption.AllDirectories);

        foreach (string filePath in filePaths)
        {
            PluginLoadContext pluginLoadContext = new(filePath);
            yield return pluginLoadContext.LoadFromAssemblyPath(filePath);
        }
    }
}
