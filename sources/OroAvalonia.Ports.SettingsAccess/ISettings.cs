namespace DustInTheWind.OroAvalonia.Ports.SettingsAccess;

public interface ISettings
{
    bool KeepOnTop { get; set; }

    bool Counterclockwise { get; set; }

    double RefreshRate { get; set; }

    double WindowLeft { get; set; }

    double WindowTop { get; set; }

    double WindowWidth { get; set; }

    double WindowHeight { get; set; }

    string ClockTemplateType { get; set; }

    event EventHandler KeepOnTopChanged;
    event EventHandler CounterclockwiseChanged;
    event EventHandler RefreshRateChanged;

    void SetWindowLocation(double left, double top);

    void SetWindowSize(double width, double height);
}
