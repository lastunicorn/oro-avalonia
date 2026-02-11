using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;

namespace DustInTheWind.OroAvalonia.Infrastructure.PageModel;

/// <summary>
/// Interaction logic for RoundPage.xaml
/// </summary>
public class RoundPage : UserControl
{
    #region Title Styled Property

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<RoundPage, string>(
        nameof(Title),
        "Page");

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    #endregion

    #region Subtitle Styled Property

    public static readonly StyledProperty<string> SubtitleProperty = AvaloniaProperty.Register<RoundPage, string>(
        nameof(Subtitle));

    public string Subtitle
    {
        get => GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    #endregion

    #region CloseCommand Styled Property

    public static readonly StyledProperty<ICommand> CloseCommandProperty = AvaloniaProperty.Register<RoundPage, ICommand>(
        nameof(CloseCommand));

    public ICommand CloseCommand
    {
        get => GetValue(CloseCommandProperty);
        set => SetValue(CloseCommandProperty, value);
    }

    #endregion
}
