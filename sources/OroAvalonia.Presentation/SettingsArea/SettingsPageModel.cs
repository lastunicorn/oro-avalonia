using System.Reflection;
using DustInTheWind.OroAvalonia.Infrastructure.PageModel;

namespace DustInTheWind.OroAvalonia.Presentation.SettingsArea;

public class SettingsPageModel : PageViewModel
{
    public string Title { get; }

    public string Subtitle { get; }

    public SettingsViewModel SettingsViewModel { get; }

    public SettingsCloseCommand SettingsCloseCommand { get; }

    public SettingsPageModel(
        SettingsViewModel settingsViewModel,
        SettingsCloseCommand settingsCloseCommand)
    {
        SettingsViewModel = settingsViewModel;
        SettingsCloseCommand = settingsCloseCommand;

        Assembly assembly = Assembly.GetEntryAssembly();
        Title = assembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

        Version version = assembly.GetName().Version;
        Subtitle = version.ToString(3);
    }
}
