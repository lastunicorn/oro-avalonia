using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.ClockAvalonia.TimeProviders;

namespace DustInTheWind.ClockAvalonia.ClearClock.Views;

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
            //clock.ClockTemplate = new DefaultTemplate();
            //clock.ClockTemplate = new FancyTemplate();
            //clock.ClockTemplate = new PandaTemplate();
            //clock.ClockTemplate = new PlayfulTemplate();
            clock.ClockTemplate = new SunTemplate();

            LocalTimeProvider timeProvider = new();
            timeProvider.Start();

            clock.TimeProvider = timeProvider;

            clock.PointerPressed += HandleClockPointerPressed;
        }
    }

    private void HandleClockPointerPressed(object sender, PointerPressedEventArgs e)
    {
        PointerPointProperties properties = e.GetCurrentPoint(this).Properties;

        if (properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ResizeGrip_DragDelta(object sender, VectorEventArgs e)
    {
        Grid mainContainer = this.FindControl<Grid>("MainContainer");
        
        if (mainContainer == null)
            return;

        double minSize = 100;

        if (mainContainer.Width == minSize && e.Vector.X <= 0 &&
            mainContainer.Height == minSize && e.Vector.Y <= 0)
            return;

        double newWidth = mainContainer.Width + e.Vector.X;
        double newHeight = mainContainer.Height + e.Vector.Y;

        double size = Math.Min(newWidth, newHeight);
        size = Math.Max(size, minSize);

        mainContainer.Width = size;
        mainContainer.Height = size;
    }
}