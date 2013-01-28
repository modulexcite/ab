using System.Collections.Generic;
using metrics.Core;

namespace ab
{
    public static class Metrics
    {
        public static string Json()
        {
            var model = ScrubInternalMetrics();
            return metrics.Serialization.Serializer.Serialize(new { metrics = model });
        }

        private static IDictionary<MetricName, IMetric> ScrubInternalMetrics()
        {
            var readModel = metrics.Metrics.AllSorted;
            var model = new Dictionary<MetricName, IMetric>();
            foreach (var entry in readModel)
            {
                if (!entry.Key.Name.StartsWith(M.Header))
                {
                    model.Add(entry.Key, entry.Value);
                }
            }
            return model;
        }
    }
}