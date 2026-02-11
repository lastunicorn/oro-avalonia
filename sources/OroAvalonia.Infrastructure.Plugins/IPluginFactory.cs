namespace DustInTheWind.OroAvalonia.PluginModel;

public interface IPluginFactory<T>
{
    T Create();

    T Create(Type type);
}
