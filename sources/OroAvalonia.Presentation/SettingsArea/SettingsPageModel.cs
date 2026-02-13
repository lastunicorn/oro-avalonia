using System.Reflection;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;

namespace DustInTheWind.OroAvalonia.Presentation.SettingsArea;

public class SettingsPageModel : PageViewModel
{
    public string Title { get; }

    public string Subtitle { get; }

    public SettingsViewModel SettingsViewModel { get; }

    public SettingsCloseCommand SettingsCloseCommand { get; }

    public TemplatesViewModel TemplatesViewModel { get; }

    public SettingsPageModel(
        SettingsViewModel settingsViewModel,
        SettingsCloseCommand settingsCloseCommand,
        TemplatesViewModel templatesViewModel)
    {
        SettingsViewModel = settingsViewModel ?? throw new ArgumentNullException(nameof(settingsViewModel));
        SettingsCloseCommand = settingsCloseCommand ?? throw new ArgumentNullException(nameof(settingsCloseCommand));
        TemplatesViewModel = templatesViewModel ?? throw new ArgumentNullException(nameof(templatesViewModel));

        Assembly assembly = Assembly.GetEntryAssembly();
        Title = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        Version version = assembly.GetName().Version;
        Subtitle = version.ToString(3);
    }
}
