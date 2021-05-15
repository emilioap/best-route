using BestRoute.Domain.Interfaces;
using BestRoute.Domain.Models;
using BestRoute.Domain.Models.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BestRoute.Service
{
    public class SearchService : ISearchService
    {
        public Node StartNode { get; set; }
        public Node EndNode { get; set; }
        public LinkedList<Node> BestRoute { get; private set; }
        public string DataSource { get; private set; }

        public SearchService()
        {
            BestRoute = new();
        }

        public Task<string> GetBestRoute(BestRouteRequest action)
        {
            StartNode = new Node() { Name = action.From.ToUpper() };
            EndNode = new Node() { Name = action.To.ToUpper() };
            DataSource = action.DataSource;

            DijkstraSearch();
            BuildShortestPath(BestRoute, EndNode);
            BestRoute.AddFirst(StartNode);

            var itinerary = new BestRouteResponse() { BestRoute = BestRoute };

            var message = itinerary.BestRoute.Last() == EndNode
                ? $"Best route found: {itinerary} > $ {itinerary.Total}"
                : "No routes found for this params :(";

            return Task.FromResult(message);
        }

        public Task<Tuple<bool, string>> CreateRoute(BestRouteCreateRequest action)
        {
            try
            {
                StringBuilder builder = new();
                string line = $"{action.From},{action.To},{action.Cost}";
                string filePath = GetFilePath(action.DataSource);

                builder.AppendLine(line);
                File.AppendAllText(filePath, builder.ToString());
            }
            catch (Exception ex)
            {
                return Task.FromResult(Tuple.Create(false, ex.Message));
            }

            return Task.FromResult(Tuple.Create(true, "Route sucessful added!"));
        }

        private void DijkstraSearch()
        {
            var dataSource = ProcessFile(DataSource);
            var nodeList = BuildStarterNode(dataSource);

            StartNode = nodeList.Where(x => x.Name == StartNode.Name).FirstOrDefault();
            StartNode.MoreCheap = 0;

            var priorityQueue = new List<Node>();
            priorityQueue.Add(StartNode);

            do
            {
                priorityQueue = priorityQueue.OrderBy(x => x.MoreCheap.Value).ToList();
                var node = priorityQueue.First();
                priorityQueue.Remove(node);

                if (node.Connections != null)
                {
                    foreach (var connection in node.Connections.OrderBy(x => x.Cost))
                    {
                        var childNode = connection.ConnectedNode;
                        childNode.Connections = nodeList.Where(x => x.Name == childNode.Name).Select(x => x.Connections).FirstOrDefault();

                        if (childNode.Visited)
                            continue;

                        if (childNode.MoreCheap == null || node.MoreCheap + connection.Cost < childNode.MoreCheap)
                        {
                            childNode.MoreCheap = node.MoreCheap + connection.Cost;
                            childNode.MoreClose = node;

                            if (!priorityQueue.Contains(childNode))
                                priorityQueue.Add(childNode);
                        }

                        childNode.Visited = true;

                        if (childNode.Name == EndNode.Name)
                        {
                            EndNode = childNode;
                            return;
                        }
                    }
                }

                node.Visited = true;

                if (node.Name == EndNode.Name)
                {
                    EndNode = node;
                    return;
                }

            } while (priorityQueue.Any());
        }

        private List<Route> ProcessFile(string dataSource)
        {
            List<Route> routes = new();
            string[] lines;
            string filePath = GetFilePath(dataSource);

            try
            {
                using (StreamReader reader = File.OpenText(filePath))
                    lines = reader.ReadToEnd().Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                foreach (string line in lines)
                    routes.Add(new Route()
                    {
                        From = line.Split(",")[0],
                        To = line.Split(",")[1],
                        Cost = int.Parse(line.Split(",")[2])
                    });
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return routes;
        }

        private string GetFilePath(string dataSource)
        {
            string path = Directory
                .GetParent(Environment.CurrentDirectory).ToString().Split("BestRoute\\")[0];
            return Path.Combine(path, string.Concat("BestRoute\\Repository\\", dataSource));
        }

        private void BuildShortestPath(LinkedList<Node> list, Node node)
        {
            if (node.MoreClose != null)
            {
                list.AddFirst(node);
                BuildShortestPath(list, node.MoreClose);
            }
        }

        private List<Node> BuildStarterNode(List<Route> routes)
        {
            var list = new List<Node>();
            var origins = routes.GroupBy(x => x.From).Select(s => s.Key).ToList();

            foreach (var key in origins)
            {
                list.Add(new Node()
                {
                    Name = key,
                    Visited = false,
                    Connections = routes
                        .Where(s => s.From.Contains(key))
                        .Select(s => new Edge()
                        {
                            ConnectedNode = new Node() { Name = s.To },
                            Cost = s.Cost
                        }).ToList()
                });
            }

            return list;
        }
    }
}
