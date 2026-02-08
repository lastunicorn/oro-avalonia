namespace DustInTheWind.OroAvalonia.Infrastructure.PageModel;

public class Page
{
    public string Id { get; init; }

    public Type ViewType { get; init; }
    
    public Type ViewModelType { get; init; }
}