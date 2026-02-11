using Avalonia.Controls;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;
using Microsoft.Extensions.DependencyInjection;

namespace DustInTheWind.OroAvalonia;

public class PageFactory : IPageFactory
{
    private readonly IServiceProvider serviceProvider;

    public PageFactory(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public TView CreatePage<TView, TViewModel>()
        where TView : Control
        where TViewModel : PageViewModel
    {
        TView view = serviceProvider.GetService<TView>();
        view.DataContext = serviceProvider.GetService<TViewModel>();
        return view;
    }
}
