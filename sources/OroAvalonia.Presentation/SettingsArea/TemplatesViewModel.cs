using System.Collections.ObjectModel;
using DustInTheWind.ClockAvalonia.Movements;
using DustInTheWind.ClockAvalonia.Shapes;
using DustInTheWind.ClockAvalonia.Templates;
using DustInTheWind.OroAvalonia.Ports.SettingsAccess;
using DustInTheWind.OroAvalonia.Presentation.ViewModels;
using DustInTheWind.OroWpf.Presentation;

namespace DustInTheWind.OroAvalonia.Presentation.SettingsArea;

public class TemplatesViewModel : ViewModelBase
{
    private readonly ApplicationState applicationState;
    private readonly ISettings settings;

    public ObservableCollection<TemplateItemModel> TemplateTypes { get; } = [];

    public TemplateItemModel SelectedTemplateType
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();

            if (!IsInitializing)
                PublishTemplate(field);
        }
    }

    public ClockTemplate SelectedTemplate
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public IMovement ClockMovement
    {
        get => field;
        private set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public RotationDirection ClockDirection
    {
        get => field;
        set
        {
            if (field == value)
                return;

            field = value;
            OnPropertyChanged();
        }
    }

    public TemplatesViewModel(ApplicationState applicationState, ISettings settings)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        settings.CounterclockwiseChanged += HandleCounterclockwiseChanged;

        Initialize();
    }

    private void HandleCounterclockwiseChanged(object sender, EventArgs e)
    {
        ClockDirection = settings.Counterclockwise
            ? RotationDirection.Counterclockwise
            : RotationDirection.Clockwise;
    }

    private void Initialize()
    {
        Initialize(() =>
        {
            if (applicationState.AvailableTemplateTypes?.Count > 0)
            {
                IEnumerable<TemplateItemModel> clockTemplates = applicationState.AvailableTemplateTypes
                    .Select(x => new TemplateItemModel
                    {
                        Name = x.Name
                            .Replace("ClockTemplate", "")
                            .Replace("Template", ""),
                        Type = x
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                foreach (TemplateItemModel templateItemModel in clockTemplates)
                    TemplateTypes.Add(templateItemModel);
            }

            SelectedTemplate = applicationState.ClockTemplate;

            if (applicationState.ClockTemplate != null)
            {
                Type selectedClockTemplateType = applicationState.ClockTemplate.GetType();
                SelectedTemplateType = TemplateTypes
                    .FirstOrDefault(x => x.Type == selectedClockTemplateType);
            }

            LocalTimeMovement localTimeMovement = new();
            localTimeMovement.Start();

            ClockMovement = localTimeMovement;

            ClockDirection = settings.Counterclockwise
                ? RotationDirection.Counterclockwise
                : RotationDirection.Clockwise;
        });
    }

    private void PublishTemplate(TemplateItemModel templateInfo)
    {
        ClockTemplate template = (ClockTemplate)Activator.CreateInstance(templateInfo.Type);
        applicationState.ClockTemplate = template;
        SelectedTemplate = template;
    }
}
