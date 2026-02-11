using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.MainArea;
using DustInTheWind.OroWpf.Presentation;

namespace DustInTheWind.OroAvalonia.Presentation.ClockArea;

public class ClockPageModel : PageViewModel
{
    private readonly ApplicationState applicationState;
    private readonly ISettings settings;

    public ClockTemplate ClockTemplate
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

    public IMovement ClockMovement
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

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public ClockPageModel(
        ApplicationState applicationState,
        ISettings settings,
        ToggleNavigationCommand toggleNavigationCommand)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        ToggleNavigationCommand = toggleNavigationCommand ?? throw new ArgumentNullException(nameof(toggleNavigationCommand));

        applicationState.ClockTemplateChanged += HandleClockTemplateChanged;
        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;
        settings.RefreshRateChanged += HandleRefreshRateChanged;

        Initialize();
    }

    private void Initialize()
    {
        Initialize(() =>
        {
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

    private void HandleClockTemplateChanged(object sender, EventArgs e)
    {
        Initialize(() =>
        {
            ClockTemplate = applicationState.ClockTemplate;
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
        if (ClockMovement == null)
            return;

        Initialize(() =>
        {
            ClockMovement.TickInterval = (int)Math.Round(1000 / settings.RefreshRate);
        });
    }
}
