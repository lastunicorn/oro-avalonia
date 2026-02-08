using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.ViewModels;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public partial class MainViewModel : ViewModelBase
{
    private readonly ISettings settings;
    private readonly Navigation navigation;
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

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public MainViewModel(ISettings settings, Navigation navigation, ToggleNavigationCommand toggleNavigationCommand)
    {
        this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

        ToggleNavigationCommand = toggleNavigationCommand ?? throw new ArgumentNullException(nameof(toggleNavigationCommand));

        KeepOnTop = settings.KeepOnTop;

        navigation.IsNavigationVisibleChanged += HandleIsNavigationVisibleChanged;
        IsNavigationVisible = navigation.IsNavigationVisible;

        Type templateType = typeof(SunTemplate);
        ClockTemplate = Activator.CreateInstance(templateType) as ClockTemplate;
        
        LocalTimeMovement clockMovement = new();
        clockMovement.Start();

        ClockMovement = clockMovement;
    }

    private void HandleIsNavigationVisibleChanged(object sender, EventArgs e)
    {
        IsNavigationVisible = navigation.IsNavigationVisible;
    }
}
