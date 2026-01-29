using System;

namespace DustInTheWind.OroAvalonia;

public class Navigation
{
    private bool isNavigationVisible;

    public bool IsNavigationVisible
    {
        get => isNavigationVisible;
        set
        {
            if (isNavigationVisible == value)
                return;

            isNavigationVisible = value;
            OnIsNavigationVisibleChanged(EventArgs.Empty);
        }
    }

    public event EventHandler IsNavigationVisibleChanged;

    private void OnIsNavigationVisibleChanged(EventArgs e)
    {
        IsNavigationVisibleChanged?.Invoke(this, e);
    }
}
