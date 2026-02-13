using System.Text.Json;
using System.Text.Json.Nodes;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess.Models;

namespace DustInTheWind.OroAvalonia.Ports.SettingsAccess;

internal class SettingsStorage
{
    private readonly object synchronizationObject = new();
    private readonly Timer timer;
    private AppSettings appSettings;

    private readonly Lazy<JsonSerializerOptions> serializerOptions = new(() =>
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            WriteIndented = true
        };

        return jsonSerializerOptions;
    });

    public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(100);

    public SettingsStorage()
    {
        timer = new Timer(HandleTimerTick);
    }

    public void Save(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);

        lock (synchronizationObject)
        {
            this.appSettings = appSettings;
            timer.Change(Delay, Timeout.InfiniteTimeSpan);
        }
    }

    private void HandleTimerTick(object state)
    {
        lock (synchronizationObject)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            JsonObject jsonObject = OpenAppSettingsFile(filePath);
            JsonObject newSettingsJson = JsonSerializer.SerializeToNode(appSettings, serializerOptions.Value)?.AsObject()
                ?? new JsonObject();

            foreach (KeyValuePair<string, JsonNode> property in newSettingsJson)
                jsonObject[property.Key] = property.Value?.DeepClone();

            string outputJson = JsonSerializer.Serialize(jsonObject, serializerOptions.Value);
            File.WriteAllText(filePath, outputJson);
        }
    }

    private JsonObject OpenAppSettingsFile(string filePath)
    {
        if (!File.Exists(filePath))
            return new JsonObject();

        string existingJson = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<JsonObject>(existingJson, serializerOptions.Value)
            ?? new JsonObject();
    }
}
