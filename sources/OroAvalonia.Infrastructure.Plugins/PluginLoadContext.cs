using System.Reflection;
using System.Runtime.Loader;

namespace DustInTheWind.OroAvalonia.PluginModel;

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver resolver;

    public PluginLoadContext(string pluginPath)
        : base(isCollectible: false)
    {
        resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly Load(AssemblyName assemblyName)
    {
        string path = resolver.ResolveAssemblyToPath(assemblyName);
        
        if (path != null)
            return LoadFromAssemblyPath(path);

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        string path = resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        
        if (path != null)
            return LoadUnmanagedDllFromPath(path);
        
        return base.LoadUnmanagedDll(unmanagedDllName);
    }
}