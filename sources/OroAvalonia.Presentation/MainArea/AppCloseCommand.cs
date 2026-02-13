using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public class AppCloseCommand : ICommand
{
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            desktopLifetime.Shutdown();
    }
}
