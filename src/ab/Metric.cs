namespace ab
{
    public interface IMetric
    {
        
    }

    public class Metric : IMetric
    {
        public string Tag { get; set; }
        public string Description { get; set; }

        public Metric(string tag, string description)
        {
            Tag = tag;
            Description = description;
        }

        public static void Track(string tag, int increment = 1)
        {
            
        }
    }
}
