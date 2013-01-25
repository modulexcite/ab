using System;
using metrics;
using metrics.Serialization;

namespace ab
{
    public class M 
    {
        internal const string Header = "__m__track__";
        private const string Separator = "__";
        
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

        public static string Dump()
        {
            return Serializer.Serialize(Metrics.AllSorted);
        }
    }
}
