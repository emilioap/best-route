using BestRoute.Domain.Interfaces;
using BestRoute.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BestRoute.Test
{
    public class ConfigFixture
    {
        public ServiceProvider ServiceProvider { get; private set; }
        public ApplicationBuilder ApplicationBuilder { get; private set; }

        public ConfigFixture()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient<ISearchService, SearchService>();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            ApplicationBuilder = new ApplicationBuilder(ServiceProvider);
        }
    }
}
