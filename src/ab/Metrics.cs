using System.Collections.Concurrent;

namespace ab
{
    public class Metrics
    {
        private static readonly ConcurrentDictionary<MetricKey, IMetric> _metrics = new ConcurrentDictionary<MetricKey, IMetric>();
    }
}