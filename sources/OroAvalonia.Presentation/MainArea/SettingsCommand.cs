using System.Windows.Input;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public class SettingsCommand : ICommand
{
    private readonly PageEngine pageEngine;

    public event EventHandler CanExecuteChanged;

    public SettingsCommand(PageEngine pageEngine)
    {
        this.pageEngine = pageEngine ?? throw new ArgumentNullException(nameof(pageEngine));
    }

    public bool CanExecute(object parameter) => true;

    public void Execute(object parameter)
    {
        if (pageEngine.CurrentPage == null || pageEngine.CurrentPage.Id == "settings")
            pageEngine.SelectPage("clock");
        else
            pageEngine.SelectPage("settings");
    }
}