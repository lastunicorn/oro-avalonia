using System.Windows.Input;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public class SettingsCommand : ICommand
{
    private readonly Navigation navigation;

    public event EventHandler CanExecuteChanged;

    public SettingsCommand(Navigation navigation)
    {
        this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        if (navigation.CurrentPage == null || navigation.CurrentPage.Id == "settings")
            navigation.SelectPage("clock");
        else
            navigation.SelectPage("settings");
    }
}