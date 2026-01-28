using System;
using System.Windows.Input;

namespace DustInTheWind.OroAvalonia.ViewModels;

public class ToggleNavigationCommand : ICommand
{
    private readonly Navigation navigation;
    
    public event EventHandler CanExecuteChanged;
    
    public ToggleNavigationCommand(Navigation navigation)
    {
        this.navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }
    
    public bool CanExecute(object parameter) => true;
    
    public void Execute(object parameter)
    {
        navigation.IsNavigationVisible = !navigation.IsNavigationVisible;
    }
}