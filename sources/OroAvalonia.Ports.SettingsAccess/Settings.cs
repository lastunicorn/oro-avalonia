using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.OroAvalonia.Ports.SettingsAccess;

public class Settings : ISettings
{
    private const string KeepOnTopKey = "KeepOnTop";
    private readonly IConfigurationRoot configuration;

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

    public Settings()
    {
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public bool KeepOnTop
    {
        get
        {
            string rawValue = configuration[KeepOnTopKey];
            return rawValue != null && bool.Parse(rawValue);
        }
        set
        {
            configuration[KeepOnTopKey] = value.ToString();
            Save();
            OnKeepOnTopChanged();
        }
    }

    public event EventHandler KeepOnTopChanged;

    private void OnKeepOnTopChanged()
    {
        KeepOnTopChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Save()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        string inputJson = File.ReadAllText(filePath);

        AppSettings appSettingsRoot = JsonSerializer.Deserialize<AppSettings>(inputJson);
        appSettingsRoot.KeepOnTop = KeepOnTop;

        string outputJson = JsonSerializer.Serialize(appSettingsRoot, serializerOptions.Value);
        File.WriteAllText(filePath, outputJson);
    }
}
