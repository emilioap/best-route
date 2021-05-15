using System.Collections.Generic;
using System.Linq;

namespace BestRoute.Domain.Models
{
    public class BestRouteResponse
    {
        public LinkedList<Node> BestRoute { get; set; }
        public int Connections {
            get { return BestRoute.Count - 1; }
        }
        public double Total {
            get { return (double)BestRoute.Select(x => x.MoreCheap).Sum(); }
        }

        public BestRouteResponse()
        {
            BestRoute = new LinkedList<Node>();
        }

        public override string ToString()
            => string.Join(" -> ", BestRoute.Select(s => s.Name.ToString()));
    }
}
