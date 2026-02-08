namespace DustInTheWind.OroAvalonia.Ports.SettingsAccess.Models;

internal class AppSettings
{
    public bool KeepOnTop { get; set; }

    public bool Counterclockwise { get; set; }

    public double RefreshRate { get; set; }

    public StartUp StartUp { get; set; }

    public string ClockTemplateType { get; set; }
}