namespace DustInTheWind.OroAvalonia.Infrastructure.Jobs;

public interface IJob
{
    void Start();

    void Stop();
}