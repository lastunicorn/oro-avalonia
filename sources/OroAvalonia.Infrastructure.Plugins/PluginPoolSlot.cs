namespace DustInTheWind.OroAvalonia.PluginModel;

internal class PluginPoolSlot<T>
{
    public Type Type { get; }

    public T Instance { get; set; }

    public PluginPoolSlot(Type type)
    {
        Type = type ?? throw new ArgumentNullException(nameof(type));
    }
}
