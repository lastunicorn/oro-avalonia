using DustInTheWind.OroAvalonia.Ports.SettingsAccess;

namespace DustInTheWind.OroAvalonia.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly ISettings settings;
    private bool keepOnTop;

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

    public MainViewModel(ISettings settings)
    {
        this.settings = settings ?? throw new System.ArgumentNullException(nameof(settings));

        KeepOnTop = settings.KeepOnTop;
    }
}
