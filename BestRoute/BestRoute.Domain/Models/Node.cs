using System.Collections.Generic;

namespace BestRoute.Domain.Models
{
    public class Node
    {
        public string Name { get; set; }
        public List<Edge> Connections { get; set; } = new List<Edge>();
        public double? MoreCheap { get; set; }
        public Node MoreClose { get; set; }
        public bool Visited { get; set; }
        public override string ToString() => Name;
    }
}
