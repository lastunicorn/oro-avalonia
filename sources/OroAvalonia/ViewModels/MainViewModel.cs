using System;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.ClockAvalonia.TimeProviders;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;

namespace DustInTheWind.OroAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ISettings settings;
    private readonly Navigation navigation;
    private bool keepOnTop;
    private ClockTemplate clockTemplate;
    private ITimeProvider timeProvider;
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

    public ITimeProvider TimeProvider
    {
        get => timeProvider;
        set
        {
            if (timeProvider == value)
                return;

            timeProvider = value;
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

        //ClockTemplate = new DefaultTemplate();
        //ClockTemplate = new FancyTemplate();
        //ClockTemplate = new PandaTemplate();
        //ClockTemplate = new PlayfulTemplate();

        Type templateType = typeof(SunTemplate);
        ClockTemplate = Activator.CreateInstance(templateType) as ClockTemplate;
        
        LocalTimeProvider timeProvider = new();
        timeProvider.Start();

        TimeProvider = timeProvider;
    }

    private void HandleIsNavigationVisibleChanged(object sender, EventArgs e)
    {
        IsNavigationVisible = navigation.IsNavigationVisible;
    }
}
