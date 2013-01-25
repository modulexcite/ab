using System;
using metrics;

namespace ab
{
    public class M : IMetric
    {
        public const string Header = "__m__track__";
        private const string Separator = "__";

        public string Name { get; set; }

        public object[] GetValues(DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public string Description { get; set; }

        public M(string tag, string description)
        {
            Name = tag;
            Description = description;
        }

        public static void Track(string metric, int increment = 1)
        {
            if (increment <= 0) return;
            var metricName = GetMetricName(metric);
            var counter = Metrics.Counter(typeof(M), metricName);
            counter.Increment(increment);
        }

        private static string GetMetricName(string tag)
        {
            return string.Concat(Header, tag, Separator, DateTime.Today.ToUnixTime());
        }
    }
}
