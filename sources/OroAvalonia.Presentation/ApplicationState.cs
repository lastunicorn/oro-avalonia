using DustInTheWind.ClockAvalonia.Templates;

namespace DustInTheWind.OroWpf.Presentation;

public class ApplicationState
{
    public List<Type> AvailableTemplateTypes { get; set; }

    public ClockTemplate ClockTemplate
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnClockTemplateChanged();
        }
    }

    public event EventHandler ClockTemplateChanged;

    public void OnClockTemplateChanged()
    {
        ClockTemplateChanged?.Invoke(this, EventArgs.Empty);
    }
}
