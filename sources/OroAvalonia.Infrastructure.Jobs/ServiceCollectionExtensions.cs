using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace DustInTheWind.OroAvalonia.Infrastructure.Jobs;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJobsFromAssemblyContaining<T>(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddSingleton<JobEngine>();

        Assembly assembly = typeof(T).Assembly;

        IEnumerable<Type> jobTypes = assembly.GetTypes()
            .Where(x => x.IsClass && !x.IsAbstract && typeof(IJob).IsAssignableFrom(x));

        foreach (Type jobType in jobTypes)
            serviceCollection.AddTransient(typeof(IJob), jobType);

        return serviceCollection;
    }
}
