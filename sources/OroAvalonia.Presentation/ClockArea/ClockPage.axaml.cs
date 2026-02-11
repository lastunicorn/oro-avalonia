using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;

namespace DustInTheWind.OroAvalonia.Presentation.ClockArea;

public partial class ClockPage : UserControl
{
    public ClockPage()
    {
        InitializeComponent();
    }

    private void UserControl_PointerPressed(object sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        PointerPointProperties properties = e.GetCurrentPoint(this).Properties;

        if (properties.IsLeftButtonPressed)
            this.GetVisualAncestors()
                .OfType<Window>()
                .FirstOrDefault()
                .BeginMoveDrag(e);
        else if (properties.IsRightButtonPressed)
            ToggleNavigation();
    }

    private void ToggleNavigation()
    {
        if (DataContext is ClockPageModel viewModel)
            viewModel.ToggleNavigationCommand.Execute(null);
    }
}