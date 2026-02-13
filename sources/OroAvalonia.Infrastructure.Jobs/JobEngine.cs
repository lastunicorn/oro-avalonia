namespace DustInTheWind.OroAvalonia.Infrastructure.Jobs;

public class JobEngine
{
    private readonly List<IJob> jobs = [];
    private bool isRunning;

    public JobEngine()
    {
    }

    public void Add(IJob job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job));

        if (jobs.Contains(job))
            return;

        jobs.Add(job);

        if (isRunning)
            job.Start();
    }

    public void AddRange(IEnumerable<IJob> jobs)
    {
        if (jobs == null)
            return;

        foreach (IJob job in jobs)
        {
            if (job != null)
                this.jobs.Add(job);
        }
    }

    public void Remove(IJob job)
    {
        if (job == null)
            throw new ArgumentNullException(nameof(job));

        if (!jobs.Contains(job))
            return;

        if (isRunning)
            job.Stop();

        jobs.Remove(job);
    }

    public void Start()
    {
        if (isRunning)
            return;

        isRunning = true;

        foreach (IJob job in jobs)
            job.Start();
    }

    public void Stop()
    {
        if (!isRunning)
            return;

        isRunning = false;

        foreach (IJob job in jobs)
            job.Stop();
    }
}
