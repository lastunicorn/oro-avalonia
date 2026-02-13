using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace DustInTheWind.OroAvalonia.Presentation.Behaviors;

/// <summary>
/// This behavior adds a drag-and-drop functionality on a <see cref="Window"/>.
/// </summary>
public static class WindowDragBehavior
{
    #region EnableDrag Attached Property

    public static readonly AttachedProperty<bool> EnableDragProperty = AvaloniaProperty.RegisterAttached<Control, bool>(
        "EnableDrag",
        typeof(WindowDragBehavior),
        false);

    public static bool GetEnableDrag(AvaloniaObject obj)
    {
        return obj.GetValue(EnableDragProperty);
    }

    public static void SetEnableDrag(AvaloniaObject obj, bool value)
    {
        obj.SetValue(EnableDragProperty, value);
    }

    #endregion

    static WindowDragBehavior()
    {
        EnableDragProperty.Changed.AddClassHandler<Control>(OnEnableDragChanged);
    }

    private static void OnEnableDragChanged(Control control, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue is bool oldValue && oldValue)
            control.PointerPressed -= HandlePointerPressed;

        if (e.NewValue is bool newValue && newValue)
            control.PointerPressed += HandlePointerPressed;
    }

    private static void HandlePointerPressed(object sender, PointerPressedEventArgs e)
    {
        if (sender is not Control control)
            return;

        PointerPointProperties properties = e.GetCurrentPoint(control).Properties;

        if (properties.IsLeftButtonPressed)
        {
            Window window = control as Window ?? control.GetVisualAncestors()
                .OfType<Window>()
                .FirstOrDefault();

            window?.BeginMoveDrag(e);
        }
    }
}
