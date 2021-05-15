using BestRoute.Domain.Interfaces;
using BestRoute.Domain.Models;
using BestRoute.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BestRoute.ConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            using IHost host = CreateHostBuilder().Build();
            ISearchService service = CreateScope(host.Services);
            var request = new BestRouteRequest();
            var errors = new List<ValidationResult>();
            Stopwatch stopWatch = new Stopwatch();

            do
            {
                Console.Write($"Enter the CSV filename. Ex: input-file.csv: ");
                string dataSource = Console.ReadLine();

                Console.Write($"Enter the route in format \"FROM-TO\". Ex: GRU-SCL: ");
                string route = Console.ReadLine();

                if (route.Split("-").Length == 2)
                {
                    request = new BestRouteRequest() {
                        From = route.Split("-")[0],
                        To = route.Split("-")[1],
                        DataSource = dataSource
                    };
                }

                Validator.TryValidateObject(request, new ValidationContext(request, null, null), errors, true);
            }
            while (errors.Count > 0);

            stopWatch.Start();
            var result = await service.GetBestRoute(request);
            stopWatch.Stop();

            Console.WriteLine($"{result} \n RunTime: " + stopWatch.Elapsed.ToString());
            await Task.Run(() => host.RunAsync());
        }

        static ISearchService CreateScope(IServiceProvider services)
        {
            using IServiceScope serviceScope = services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            ISearchService service = provider.GetRequiredService<ISearchService>();
            return service;
        }

        static IHostBuilder CreateHostBuilder()
            => Host.CreateDefaultBuilder().ConfigureServices((_, services)
                => services.AddScoped<ISearchService, SearchService>());
    }
}