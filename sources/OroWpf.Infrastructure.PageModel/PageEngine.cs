namespace DustInTheWind.OroAvalonia.Infrastructure.PageModel;

public class PageEngine
{
    public Page CurrentPage
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnCurrentPageChanged();
        }
    }

    public List<Page> Pages { get; } = [];

    public bool IsNavigationVisible
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnIsNavigationVisibleChanged();
        }
    }

    public event EventHandler CurrentPageChanged;
    public event EventHandler IsNavigationVisibleChanged;

    public void SelectPage(string pageId)
    {
        Page pageToSelect = Pages.FirstOrDefault(x => x.Id == pageId);

        if (pageToSelect == null)
            throw new ArgumentException($"Page with id '{pageId}' not found.", nameof(pageId));

        CurrentPage = pageToSelect;
    }

    public void ToggleNavigation()
    {
        IsNavigationVisible = !IsNavigationVisible;
    }

    public virtual void OnCurrentPageChanged()
    {
        CurrentPageChanged?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnIsNavigationVisibleChanged()
    {
        IsNavigationVisibleChanged?.Invoke(this, EventArgs.Empty);
    }

    public void CloseCurrentPage()
    {
        CurrentPage = null;
    }
}
