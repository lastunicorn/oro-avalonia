using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.ViewModels;
using DustInTheWind.OroWpf.Presentation;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public partial class MainViewModel : ViewModelBase
{
    private readonly ISettings settings;
    private readonly Navigation navigation;
    private readonly ApplicationState applicationState;
    private bool keepOnTop;
    private ClockTemplate clockTemplate;
    private IMovement clockMovement;
    private bool isNavigationVisible;

    public bool KeepOnTop
    {
        get => keepOnTop;
        set
        {
            if (keepOnTop == value)
                return;

            keepOnTop = value;
            OnPropertyChanged();
        }
    }

    public ClockTemplate ClockTemplate
    {
        get => clockTemplate;
        set
        {
            if (clockTemplate == value)
                return;

            clockTemplate = value;
            OnPropertyChanged();
        }
    }

    public IMovement ClockMovement
    {
        get => clockMovement;
        set
        {
            if (clockMovement == value)
                return;

            clockMovement = value;
            OnPropertyChanged();
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
        }
    }

    public bool IsNavigationVisible
    {
        get => isNavigationVisible;
        set
        {
            if (isNavigationVisible == value)
                return;

            isNavigationVisible = value;
            OnPropertyChanged();
        }
    }

    public ISettings Settings => settings;

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public MainViewModel(ISettings settings, Navigation navigation,
        ToggleNavigationCommand toggleNavigationCommand,
        ApplicationState applicationState)
    {
        this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));

        ToggleNavigationCommand = toggleNavigationCommand ?? throw new ArgumentNullException(nameof(toggleNavigationCommand));
        
        navigation.IsNavigationVisibleChanged += HandleIsNavigationVisibleChanged;
        settings.KeepOnTopChanged += HandleKeepOnTopChanged;
        settings.RefreshRateChanged += HandleRefreshRateChanged;
        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;

        Initialize();
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;

            IsNavigationVisible = navigation.IsNavigationVisible;

            ClockTemplate = applicationState.ClockTemplate;

            LocalTimeMovement clockMovement = new()
            {
                TickInterval = (int)Math.Round(1000 / settings.RefreshRate)
            };
            clockMovement.Start();

            ClockMovement = clockMovement;

            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }

    private void HandleIsNavigationVisibleChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            IsNavigationVisible = navigation.IsNavigationVisible;
        });
    }

    private void HandleKeepOnTopChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            KeepOnTop = settings.KeepOnTop;
        });
    }

    private void HandleRefreshRateChanged(object sender, EventArgs e)
    {
        if (ClockMovement == null)
            return;

        Initialize(() =>
        {
            ClockMovement.TickInterval = (int)Math.Round(1000 / settings.RefreshRate);
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
}
