using System;
using Avalonia;
using Avalonia.Controls;

namespace DustInTheWind.OroAvalonia;

public class SquareDecorator : Decorator
{
    protected override Size MeasureOverride(Size availableSize)
    {
        Thickness padding = Padding;

        double availableWidth = Math.Max(0, availableSize.Width - padding.Left - padding.Right);
        double availableHeight = Math.Max(0, availableSize.Height - padding.Top - padding.Bottom);

        double size = Math.Min(availableWidth, availableHeight);

        Child?.Measure(new Size(size, size));

        // Return square INCLUDING padding

        double width = size + padding.Left + padding.Right;
        double height = size + padding.Top + padding.Bottom;

        return new Size(width, height);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        Thickness padding = Padding;

        double innerWidth = Math.Max(0, finalSize.Width - padding.Left - padding.Right);
        double innerHeight = Math.Max(0, finalSize.Height - padding.Top - padding.Bottom);

        double size = Math.Min(innerWidth, innerHeight);

        Rect rect = new(padding.Left, padding.Top, size, size);

        Child?.Arrange(rect);

        // Keep the decorator square
        double outerSize = size + padding.Left + padding.Right;
        return new Size(outerSize, outerSize);
    }
}
