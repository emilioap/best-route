namespace BestRoute.Domain.Models
{
    public class Edge
    {
        public double Length { get; set; }
        public double Cost { get; set; }
        public Node ConnectedNode { get; set; }
        public override string ToString() => "-> " + ConnectedNode.ToString();
    }
}
