using Avalonia.Controls;
using Avalonia.Input;

namespace DustInTheWind.OroAvalonia.Presentation.MainArea;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
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