using BestRoute.Domain.Models;
using BestRoute.Domain.Models.Requests;
using System;
using System.Threading.Tasks;

namespace BestRoute.Domain.Interfaces
{
    public interface ISearchService
    {
        Task<string> GetBestRoute(BestRouteRequest action);
        Task<Tuple<bool, string>> CreateRoute(BestRouteCreateRequest action);
    }
}
