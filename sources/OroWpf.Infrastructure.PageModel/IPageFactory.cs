using Avalonia.Controls;

namespace DustInTheWind.OroAvalonia.Infrastructure.PageModel;

public interface IPageFactory
{
    TView CreatePage<TView, TViewModel>()
        where TView : Control
        where TViewModel : PageViewModel;
}