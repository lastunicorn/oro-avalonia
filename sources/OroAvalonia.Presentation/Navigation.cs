//using DustInTheWind.OroAvalonia.Infrastructure.PageModel;

//namespace DustInTheWind.OroAvalonia.Presentation;

//public class Navigation
//{
//    private bool isNavigationVisible;

//    public Page CurrentPage
//    {
//        get => field;
//        private set
//        {
//            if (field == value)
//                return;

//            field = value;
//            OnCurrentPageChanged();
//        }
//    }

//    public List<Page> Pages { get; } = [];

//    public bool IsNavigationVisible
//    {
//        get => isNavigationVisible;
//        set
//        {
//            if (isNavigationVisible == value)
//                return;

//            isNavigationVisible = value;
//            OnIsNavigationVisibleChanged(EventArgs.Empty);
//        }
//    }

//    public event EventHandler CurrentPageChanged;
//    public event EventHandler IsNavigationVisibleChanged;

//    public void SelectPage(string pageId)
//    {
//        Page pageToSelect = Pages.FirstOrDefault(x => x.Id == pageId);

//        if (pageToSelect == null)
//            throw new ArgumentException($"Page with id '{pageId}' not found.", nameof(pageId));

//        CurrentPage = pageToSelect;
//    }

//    public void ToggleNavigation()
//    {
//        IsNavigationVisible = !IsNavigationVisible;
//    }

//    public virtual void OnCurrentPageChanged()
//    {
//        CurrentPageChanged?.Invoke(this, EventArgs.Empty);
//    }

//    private void OnIsNavigationVisibleChanged(EventArgs e)
//    {
//        IsNavigationVisibleChanged?.Invoke(this, e);
//    }
//}
