namespace DustInTheWind.OroAvalonia.PluginModel;

public class PluginPool<T>
    where T : class
{
    private readonly object syncRoot = new();
    private readonly List<PluginPoolSlot<T>> slots = [];
    private readonly IPluginFactory<T> pluginFactory;

    public PluginPool(IPluginFactory<T> pluginFactory)
    {
        this.pluginFactory = pluginFactory ?? throw new ArgumentNullException(nameof(pluginFactory));
    }

    public void AddRange(IEnumerable<Type> types)
    {
        if (types == null)
            throw new ArgumentNullException(nameof(types));

        foreach (Type type in types)
            AddInternal(type);
    }

    public bool Add(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        return AddInternal(type);
    }

    private bool AddInternal(Type type)
    {
        if (type != typeof(T) && !typeof(T).IsAssignableFrom(type))
            throw new ArgumentException($"Type {type.FullName} is not {typeof(T).FullName}.");

        if (slots.Any(x => x.Type == type))
            return false;

        PluginPoolSlot<T> slot = new(type);
        slots.Add(slot);

        return true;
    }

    public IEnumerable<T> GetAll()
    {
        foreach (PluginPoolSlot<T> slot in slots)
        {
            lock (syncRoot)
            {
                if (slot.Instance == null)
                    slot.Instance = pluginFactory.Create(slot.Type);
            }

            yield return slot.Instance;
        }
    }

    public T Get(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        PluginPoolSlot<T> slot = slots.FirstOrDefault(x => x.Type == type);

        if (slot == null)
            return null;

        lock (syncRoot)
        {
            if (slot.Instance == null)
                slot.Instance = pluginFactory.Create(type);
        }

        return slot.Instance;
    }

    public T GetByTypeName(string assemblyQualifiedName)
    {
        if (string.IsNullOrWhiteSpace(assemblyQualifiedName))
            return null;

        PluginPoolSlot<T> slot = slots.FirstOrDefault(x => x.Type.AssemblyQualifiedName == assemblyQualifiedName);

        if (slot == null)
            return null;

        lock (syncRoot)
        {
            if (slot.Instance == null)
                slot.Instance = pluginFactory.Create(slot.Type);
        }

        return slot.Instance;
    }
}
