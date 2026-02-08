using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using DustInTheWind.ClockAvalonia;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        InitializeClock();
    }

    private void InitializeClock()
    {
        AnalogClock clock = this.FindControl<AnalogClock>("AnalogClock");

        if (clock != null)
        {
            clock.PointerPressed += HandleClockPointerPressed;
        }
    }

    private void HandleClockPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PointerPointProperties properties = e.GetCurrentPoint(this).Properties;

        if (properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
        else if (properties.IsRightButtonPressed)
            ToggleNavigation();
    }

    private void ToggleNavigation()
    {
        if (DataContext is MainViewModel viewModel)
            viewModel.ToggleNavigationCommand.Execute(null);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ResizeGrip_DragDelta(object sender, VectorEventArgs e)
    {
        double minSize = 100;

        if (Width == minSize && e.Vector.X <= 0 &&
            Height == minSize && e.Vector.Y <= 0)
            return;

        double newWidth = Width + e.Vector.X;
        double newHeight = Height + e.Vector.Y;

        double size = Math.Min(newWidth, newHeight);
        size = Math.Max(size, minSize);

        Width = size;
        Height = size;
    }
}