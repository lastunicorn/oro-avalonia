using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace DustInTheWind.OroAvalonia.Presentation.CustomControls;

public class CornerThumb : Thumb
{
    #region Geometry Styled Property

    public static readonly StyledProperty<Geometry> GeometryProperty = AvaloniaProperty.Register<CornerThumb, Geometry>(
        nameof(Geometry));

    public Geometry Geometry
    {
        get => GetValue(GeometryProperty);
        set => SetValue(GeometryProperty, value);
    }

    #endregion

    #region CornerRadius Styled Property

    public static readonly StyledProperty<double> CornerRadiusProperty = AvaloniaProperty.Register<CornerThumb, double>(
        name: nameof(CornerRadius),
        defaultValue: 10);

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    #endregion

    static CornerThumb()
    {
        CornerRadiusProperty.Changed.AddClassHandler<CornerThumb>(HandleCornerRadiusPropertyChanged);
    }

    private static void HandleCornerRadiusPropertyChanged(CornerThumb cornerThumb, AvaloniaPropertyChangedEventArgs e)
    {
        cornerThumb.UpdateVisualElements();
    }

    public CornerThumb()
    {
        UpdateVisualElements();
    }

    private void UpdateVisualElements()
    {
        Geometry = new TopLeftCornerShape()
        {
            CornerRadius = CornerRadius
        };
    }
}
