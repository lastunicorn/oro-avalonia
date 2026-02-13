using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;

namespace DustInTheWind.OroAvalonia.Presentation.Behaviors;

/// <summary>
/// Provides attached properties and behaviors for persisting and restoring the size and position of a window using
/// application-defined settings.
/// </summary>
/// <remarks>
/// The WindowLocationBehavior class enables automatic saving and loading of window location and size by
/// associating an ISettings implementation with a Window. This is useful for applications that want to remember window
/// placement between sessions. To use, attach the Settings property to a Window and provide an ISettings instance that
/// handles storage and retrieval of window state.
/// </remarks>
public static class WindowSizeAndLocationBehavior
{
    #region Settings Attached Property

    public static readonly AttachedProperty<ISettings> SettingsProperty = AvaloniaProperty.RegisterAttached<Window, ISettings>(
        "Settings",
        typeof(WindowSizeAndLocationBehavior));

    public static ISettings GetSettings(AvaloniaObject obj)
    {
        return obj.GetValue(SettingsProperty);
    }

    public static void SetSettings(AvaloniaObject obj, ISettings value)
    {
        obj.SetValue(SettingsProperty, value);
    }

    #endregion

    static WindowSizeAndLocationBehavior()
    {
        SettingsProperty.Changed.AddClassHandler<Window>(OnSettingsChanged);
    }

    private static void OnSettingsChanged(Window window, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue != null)
        {
            window.Opened -= Window_Opened;
            window.PositionChanged -= Window_PositionChanged;
        }

        if (e.NewValue != null)
            window.Opened += Window_Opened;
    }

    private static void Window_Opened(object sender, EventArgs e)
    {
        if (sender is not Window window)
            return;

        ISettings settings = GetSettings(window);
        if (settings != null)
        {
            LoadWindowLocation(window, settings);
            LoadWindowSize(window, settings);

            window.Opened -= Window_Opened;

            EnsureWindowIsOnScreen(window);

            window.PositionChanged += Window_PositionChanged;
            window.SizeChanged += Window_SizeChanged;
        }
    }

    private static void Window_PositionChanged(object sender, PixelPointEventArgs e)
    {
        if (sender is Window window)
        {
            ISettings settings = GetSettings(window);

            if (settings != null)
            {
                PixelPoint position = window.Position;
                settings.SetWindowLocation(position.X, position.Y);
            }
        }
    }

    private static void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (sender is Window window)
        {
            ISettings settings = GetSettings(window);
            settings?.SetWindowSize(window.Width, window.Height);
        }
    }

    private static void ContainerElement_PropertyChanged(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == Control.BoundsProperty && sender is Control container)
        {
            Window window = container.FindAncestorOfType<Window>();
            if (window != null)
            {
                ISettings settings = GetSettings(window);

                if (settings != null)
                {
                    Rect bounds = container.Bounds;
                    settings.SetWindowSize(bounds.Width, bounds.Height);
                }
            }
        }
    }

    private static void LoadWindowLocation(Window window, ISettings settings)
    {
        double left = settings.WindowLeft;
        double top = settings.WindowTop;

        if (!double.IsNaN(left) && !double.IsNaN(top))
            window.Position = new PixelPoint((int)left, (int)top);
    }

    private static void LoadWindowSize(Window window, ISettings settings)
    {
        double width = settings.WindowWidth;
        double height = settings.WindowHeight;

        if (!double.IsNaN(width) && !double.IsNaN(height))
        {
            window.Width = width;
            window.Height = height;
        }
    }

    private static void EnsureWindowIsOnScreen(Window window)
    {
        double windowWidth = window.Bounds.Width;
        double windowHeight = window.Bounds.Height;

        if (windowWidth == 0 || double.IsNaN(windowWidth))
            return;

        if (windowHeight == 0 || double.IsNaN(windowHeight))
            return;

        IReadOnlyList<Screen> screens = window.Screens.All;
        if (screens.Count == 0)
            return;

        PixelRect virtualScreenBounds = screens
            .Select(screen => screen.Bounds)
            .Aggregate((r1, r2) => r1.Union(r2));

        PixelPoint position = window.Position;

        bool isCompletelyOutside =
            position.X + windowWidth < virtualScreenBounds.X ||
            position.X > virtualScreenBounds.X + virtualScreenBounds.Width ||
            position.Y + windowHeight < virtualScreenBounds.Y ||
            position.Y > virtualScreenBounds.Y + virtualScreenBounds.Height;

        if (isCompletelyOutside)
        {
            Screen primaryScreen = window.Screens.Primary ?? screens[0];
            PixelRect primaryBounds = primaryScreen.Bounds;

            int newX = (int)((primaryBounds.Width - windowWidth) / 2) + primaryBounds.X;
            int newY = (int)((primaryBounds.Height - windowHeight) / 2) + primaryBounds.Y;

            window.Position = new PixelPoint(newX, newY);
        }
        else
        {
            int newX = position.X;
            int newY = position.Y;

            if (position.X < virtualScreenBounds.X)
                newX = virtualScreenBounds.X;
            else if (position.X + windowWidth > virtualScreenBounds.X + virtualScreenBounds.Width)
                newX = (int)(virtualScreenBounds.X + virtualScreenBounds.Width - windowWidth);

            if (position.Y < virtualScreenBounds.Y)
                newY = virtualScreenBounds.Y;
            else if (position.Y + windowHeight > virtualScreenBounds.Y + virtualScreenBounds.Height)
                newY = (int)(virtualScreenBounds.Y + virtualScreenBounds.Height - windowHeight);

            if (newX != position.X || newY != position.Y)
                window.Position = new PixelPoint(newX, newY);
        }
    }
}
