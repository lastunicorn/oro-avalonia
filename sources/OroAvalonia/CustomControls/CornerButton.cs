using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;

namespace DustInTheWind.ClockAvalonia.ClearClock.CustomControls;

public class CornerButton : Button
{
    #region Corner Styled Property

    public static readonly StyledProperty<CornerType> CornerProperty =
        AvaloniaProperty.Register<CornerButton, CornerType>(nameof(Corner), CornerType.TopLeft);

    public CornerType Corner
    {
        get => GetValue(CornerProperty);
        set => SetValue(CornerProperty, value);
    }

    #endregion

    #region CornerRadius Styled Property

    public static readonly StyledProperty<double> CornerRadiusProperty = AvaloniaProperty.Register<CornerButton, double>(
        nameof(CornerRadius),
        0.1);

    public double CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    #endregion

    #region Geometry Styled Property

    public static readonly StyledProperty<Geometry> GeometryProperty = AvaloniaProperty.Register<CornerButton, Geometry>(
        nameof(Geometry));

    public Geometry Geometry
    {
        get => GetValue(GeometryProperty);
        private set => SetValue(GeometryProperty, value);
    }

    #endregion

    #region GeometryTransform Styled Property

    public static readonly StyledProperty<Transform> GeometryTransformProperty = AvaloniaProperty.Register<CornerButton, Transform>(
        nameof(GeometryTransform));

    public Transform GeometryTransform
    {
        get => GetValue(GeometryTransformProperty);
        set => SetValue(GeometryTransformProperty, value);
    }

    #endregion

    #region ContentHorizontalAlignment Styled Property

    public static readonly StyledProperty<HorizontalAlignment> ContentHorizontalAlignmentProperty =
        AvaloniaProperty.Register<CornerButton, HorizontalAlignment>(
            nameof(ContentHorizontalAlignment),
            HorizontalAlignment.Left);

    public HorizontalAlignment ContentHorizontalAlignment
    {
        get => GetValue(ContentHorizontalAlignmentProperty);
        set => SetValue(ContentHorizontalAlignmentProperty, value);
    }

    #endregion

    #region ContentVerticalAlignment Styled Property

    public static readonly StyledProperty<VerticalAlignment> ContentVerticalAlignmentProperty =
        AvaloniaProperty.Register<CornerButton, VerticalAlignment>(
            nameof(ContentVerticalAlignment),
            VerticalAlignment.Top);

    public VerticalAlignment ContentVerticalAlignment
    {
        get => GetValue(ContentVerticalAlignmentProperty);
        set => SetValue(ContentVerticalAlignmentProperty, value);
    }

    #endregion

    #region HoverBackground Styled Property

    public static readonly StyledProperty<IBrush> HoverBackgroundProperty =
        AvaloniaProperty.Register<CornerButton, IBrush>(nameof(HoverBackground));

    public IBrush HoverBackground
    {
        get => GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    #endregion

    static CornerButton()
    {
        CornerProperty.Changed.AddClassHandler<CornerButton>((button, e) => button.UpdateVisualElements());
    }

    public CornerButton()
    {
        CornerRadiusProperty.Changed.AddClassHandler<CornerButton>((cornerButton, e) => cornerButton.OnCornerRadiusChanged(e));

        UpdateVisualElements();
    }

    private void OnCornerRadiusChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateVisualElements();
    }

    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        UpdateVisualElements();
    }

    private void UpdateVisualElements()
    {
        Geometry = new CornerShape()
        {
            CornerRadius = CornerRadius
        };

        switch (Corner)
        {
            case CornerType.TopLeft:
                GeometryTransform = null;
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Top;

                break;

            case CornerType.TopRight:
                GeometryTransform = new RotateTransform(90, 0.5, 0.5);
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Top;
                break;

            case CornerType.BottomLeft:
                GeometryTransform = new RotateTransform(270, 0.5, 0.5);
                ContentHorizontalAlignment = HorizontalAlignment.Left;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            case CornerType.BottomRight:
                GeometryTransform = new RotateTransform(180, 0.5, 0.5);
                ContentHorizontalAlignment = HorizontalAlignment.Right;
                ContentVerticalAlignment = VerticalAlignment.Bottom;
                break;

            default:
                return;
        }
    }
}
