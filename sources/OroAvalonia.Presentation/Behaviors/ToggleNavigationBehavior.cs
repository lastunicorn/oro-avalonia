using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace DustInTheWind.OroAvalonia.Presentation.Behaviors;

public static class ToggleNavigationBehavior
{
    #region Command Attached Property

    public static readonly AttachedProperty<ICommand> CommandProperty = AvaloniaProperty.RegisterAttached<Control, ICommand>(
        "Command",
        typeof(ToggleNavigationBehavior),
        null);

    public static ICommand GetCommand(AvaloniaObject obj)
    {
        return obj.GetValue(CommandProperty);
    }

    public static void SetCommand(AvaloniaObject obj, ICommand value)
    {
        obj.SetValue(CommandProperty, value);
    }

    #endregion

    static ToggleNavigationBehavior()
    {
        CommandProperty.Changed.AddClassHandler<Control>(OnCommandChanged);
    }

    private static void OnCommandChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is ICommand)
            control.PointerPressed -= HandlePointerPressed;

        if (e.NewValue is ICommand)
            control.PointerPressed += HandlePointerPressed;
    }

    private static void HandlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is not Control control)
            return;

        PointerPointProperties properties = e.GetCurrentPoint(control).Properties;

        if (properties.IsRightButtonPressed)
        {
            ICommand command = GetCommand(control);
            command?.Execute(null);
        }
    }
}
