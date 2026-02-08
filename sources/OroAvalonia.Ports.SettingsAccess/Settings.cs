using System.Globalization;
using System.Text.Json;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess.Models;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.OroAvalonia.Ports.SettingsAccess;

public class Settings : ISettings
{
    private const string KeepOnTopKey = "KeepOnTop";
    private const string CounterclockwiseKey = "Counterclockwise";
    private const string RefreshRateKey = "RefreshRate";
    private const string WindowLeftKey = "StartUp:WindowLeft";
    private const string WindowTopKey = "StartUp:WindowTop";
    private const string WindowWidthKey = "StartUp:WindowWidth";
    private const string WindowHeightKey = "StartUp:WindowHeight";
    private const string ClockTemplateTypeKey = "ClockTemplateType";

    private readonly IConfigurationRoot configuration;
    private readonly SettingsStorage settingsStorage = new();

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

            if (rawValue == null)
                return true;

            bool success = bool.TryParse(rawValue, out bool value);
            return success ? value : true;
        }
        set
        {
            configuration[KeepOnTopKey] = value.ToString();
            Save();
            OnKeepOnTopChanged();
        }
    }

    public bool Counterclockwise
    {
        get
        {
            string rawValue = configuration[CounterclockwiseKey];

            if (rawValue == null)
                return false;

            bool success = bool.TryParse(rawValue, out bool value);
            return success ? value : true;
        }
        set
        {
            configuration[CounterclockwiseKey] = value.ToString();
            Save();
            OnCounterclockwiseChanged();
        }
    }

    public double RefreshRate
    {
        get
        {
            string rawValue = configuration[RefreshRateKey];

            if (rawValue == null)
                return 10;

            bool success = double.TryParse(rawValue, out double value);
            return success ? value : 10;
        }
        set
        {
            configuration[RefreshRateKey] = value.ToString();
            Save();
            OnRefreshRateChanged();
        }
    }

    public double WindowLeft
    {
        get
        {
            string rawValue = configuration[WindowLeftKey];
            return rawValue != null ? double.Parse(rawValue, CultureInfo.InvariantCulture) : double.NaN;
        }
        set
        {
            configuration[WindowLeftKey] = value.ToString();
            Save();
        }
    }

    public double WindowTop
    {
        get
        {
            string rawValue = configuration[WindowTopKey];
            return rawValue != null ? double.Parse(rawValue, CultureInfo.InvariantCulture) : double.NaN;
        }
        set
        {
            configuration[WindowTopKey] = value.ToString();
            Save();
        }
    }

    public double WindowWidth
    {
        get
        {
            string rawValue = configuration[WindowWidthKey];
            return rawValue != null ? double.Parse(rawValue, CultureInfo.InvariantCulture) : double.NaN;
        }
        set
        {
            configuration[WindowWidthKey] = value.ToString();
            Save();
        }
    }

    public double WindowHeight
    {
        get
        {
            string rawValue = configuration[WindowHeightKey];
            return rawValue != null ? double.Parse(rawValue, CultureInfo.InvariantCulture) : double.NaN;
        }
        set
        {
            configuration[WindowHeightKey] = value.ToString();
            Save();
        }
    }

    public string ClockTemplateType
    {
        get
        {
            string rawValue = configuration[ClockTemplateTypeKey];
            return rawValue;
        }
        set
        {
            configuration[ClockTemplateTypeKey] = value;
            Save();
        }
    }

    public void SetWindowLocation(double left, double top)
    {
        configuration[WindowLeftKey] = left.ToString(CultureInfo.InvariantCulture);
        configuration[WindowTopKey] = top.ToString(CultureInfo.InvariantCulture);

        Save();
    }

    public void SetWindowSize(double width, double height)
    {
        configuration[WindowWidthKey] = width.ToString(CultureInfo.InvariantCulture);
        configuration[WindowHeightKey] = height.ToString(CultureInfo.InvariantCulture);

        Save();
    }

    public event EventHandler KeepOnTopChanged;
    public event EventHandler CounterclockwiseChanged;
    public event EventHandler RefreshRateChanged;

    private void OnKeepOnTopChanged()
    {
        KeepOnTopChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnCounterclockwiseChanged()
    {
        CounterclockwiseChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnRefreshRateChanged()
    {
        RefreshRateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Save()
    {
        AppSettings appSettingsRoot = new()
        {
            KeepOnTop = KeepOnTop,
            Counterclockwise = Counterclockwise,
            RefreshRate = RefreshRate
        };

        appSettingsRoot.StartUp ??= new StartUp();

        appSettingsRoot.StartUp.WindowLeft = double.IsNaN(WindowLeft)
            ? null
            : WindowLeft;
        appSettingsRoot.StartUp.WindowTop = double.IsNaN(WindowTop)
            ? null
            : WindowTop;
        appSettingsRoot.StartUp.WindowWidth = double.IsNaN(WindowWidth)
            ? null
            : WindowWidth;
        appSettingsRoot.StartUp.WindowHeight = double.IsNaN(WindowHeight)
            ? null
            : WindowHeight;

        appSettingsRoot.ClockTemplateType = ClockTemplateType;

        settingsStorage.Save(appSettingsRoot);
    }
}
