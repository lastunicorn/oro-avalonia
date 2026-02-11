using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.ViewModels;

namespace DustInTheWind.OroAvalonia.Presentation.SettingsArea;

public class SettingsViewModel : ViewModelBase
{
    private readonly ISettings settings;

    public bool KeepOnTop
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                settings.KeepOnTop = value;
        }
    }

    public RotationDirection ClockDirection
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
            {
                settings.Counterclockwise = value switch
                {
                    RotationDirection.Clockwise => false,
                    RotationDirection.Counterclockwise => true,
                    _ => throw new InvalidOperationException($"Unknown rotation direction: {value}")
                };
            }
        }
    }

    public double RefreshRate
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                settings.RefreshRate = value;
        }
    }

    public SettingsViewModel(ISettings settings)
    {
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        settings.KeepOnTopChanged += HandleKeepOnTopChanged;
        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;
        settings.RefreshRateChanged += HandleRefreshRateChanged;

        Initialize();
    }

    private void HandleKeepOnTopChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;
        });
    }

    private void HandleCounterclockwiseChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }

    private void HandleRefreshRateChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            RefreshRate = settings.RefreshRate;
        });
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;
            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
            RefreshRate = settings.RefreshRate;
        });
    }
}
