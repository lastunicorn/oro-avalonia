using Avalonia.Controls;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.ClockArea;
using DustInTheWind.OroAvalonia.Presentation.SettingsArea;
using DustInTheWind.OroAvalonia.Presentation.ViewModels;
using DustInTheWind.OroWpf.Presentation;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public partial class MainViewModel : ViewModelBase
{
    private readonly ISettings settings;
    private readonly PageEngine pageEngine;
    private readonly ApplicationState applicationState;
    private readonly IPageFactory pageFactory;
    private bool keepOnTop;
    private ClockTemplate clockTemplate;
    private IMovement clockMovement;
    private bool isNavigationVisible;

    public Control CurrentPage
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;

            OnPropertyChanged();
        }
    }

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

    public bool IsSettingsPageActive
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public ISettings Settings => settings;

    public ToggleNavigationCommand ToggleNavigationCommand { get; }

    public SettingsCommand SettingsCommand { get; }

    public AppCloseCommand AppCloseCommand { get; }

    public MainViewModel(ISettings settings, PageEngine pageEngine,
        ApplicationState applicationState,
        IPageFactory pageFactory,
        ToggleNavigationCommand toggleNavigationCommand,
        SettingsCommand settingsCommand,
        AppCloseCommand appCloseCommand)
    {
        this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));
        this.pageEngine = pageEngine ?? throw new ArgumentNullException(nameof(pageEngine));
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.pageFactory = pageFactory ?? throw new ArgumentNullException(nameof(pageFactory));
        
        ToggleNavigationCommand = toggleNavigationCommand ?? throw new ArgumentNullException(nameof(toggleNavigationCommand));
        SettingsCommand = settingsCommand ?? throw new ArgumentNullException(nameof(settingsCommand));
        AppCloseCommand = appCloseCommand ?? throw new ArgumentNullException(nameof(appCloseCommand));

        pageEngine.CurrentPageChanged += HandlePageChanged;
        pageEngine.IsNavigationVisibleChanged += HandleIsNavigationVisibleChanged;
        settings.KeepOnTopChanged += HandleKeepOnTopChanged;
        settings.RefreshRateChanged += HandleRefreshRateChanged;
        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;

        Initialize();
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            DisplayCurrentPage();

            KeepOnTop = settings.KeepOnTop;

            IsNavigationVisible = pageEngine.IsNavigationVisible;

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
            IsNavigationVisible = pageEngine.IsNavigationVisible;
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

    private void HandlePageChanged(object sender, EventArgs e)
    {
        RemoveCurrentPage();
        DisplayCurrentPage();
    }

    private void RemoveCurrentPage()
    {
        if (CurrentPage != null)
        {
            if (CurrentPage.DataContext is PageViewModel pageViewModel)
                pageViewModel.PrepareForClose();

            CurrentPage = null;
        }
    }

    private void DisplayCurrentPage()
    {
        if (pageEngine.CurrentPage?.ViewType == typeof(SettingsPage))
        {
            CurrentPage = pageFactory.CreatePage<SettingsPage, SettingsPageModel>();
            IsSettingsPageActive = true;
        }
        else if (pageEngine.CurrentPage?.ViewType == typeof(ClockPage))
        {
            CurrentPage = GetOrCreateClockPage();
            IsSettingsPageActive = false;
        }
        else
        {
            pageEngine.SelectPage("clock");
        }
    }

    private ClockPage clockPage;

    private ClockPage GetOrCreateClockPage()
    {
        clockPage ??= pageFactory.CreatePage<ClockPage, ClockPageModel>();
        return clockPage;
    }
}
