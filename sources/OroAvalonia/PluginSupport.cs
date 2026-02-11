using System.Reflection;

namespace DustInTheWind.OroAvalonia;

internal static class PluginSupport
{
    private const string templateDirectoryName = "templates";

    public static IEnumerable<Assembly> LoatTemplateAssemblies()
    {
        string templateDirectoryPath = Path.Combine(AppContext.BaseDirectory, templateDirectoryName);

        if (!Directory.Exists(templateDirectoryPath))
            yield break;

        string[] filePaths = Directory.GetFiles(templateDirectoryPath, "*.dll", SearchOption.AllDirectories);

        foreach (string filePath in filePaths)
        {
            PluginLoadContext pluginLoadContext = new(filePath);
            yield return pluginLoadContext.LoadFromAssemblyPath(filePath);
        }
    }
}