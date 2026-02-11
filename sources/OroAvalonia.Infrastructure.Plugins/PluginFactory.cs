using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia.PluginModel;

public class PluginFactory<T> : IPluginFactory<T>
{
    private readonly IServiceProvider serviceProvider;

    public PluginFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public T Create()
    {
        return serviceProvider.GetService<T>();
    }

    T IPluginFactory<T>.Create(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        if (type != typeof(T) && !typeof(T).IsAssignableFrom(type))
            throw new ArgumentException($"Type {type.FullName} is not {typeof(T).FullName}.");

        return (T)serviceProvider.GetService(type);
    }
}
