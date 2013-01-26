using System;
using System.Collections.Concurrent;
using System.Linq;
using metrics;

namespace ab
{
    public class M 
    {
        private const string Separator = "__";
        internal const string Header = "__m__track__";
        
        internal static IProducerConsumerCollection<Sample> Samples { get; set; }

        static M()
        {
            Samples = new ConcurrentBag<Sample>();
        }
        
        public static void Track(string metric, int increment = 1)
        {
            if (increment <= 0) return;

            var now = DateTime.Now;
            var today = DateTime.Today.ToUnixTime();
            
            // Track the counter
            var counter = Metrics.Counter(typeof(M), InternalMetric(metric, today));
            counter.Increment(increment);

            // Track a metric tick for analysis (currently is not correctly counting by date!)
            Samples.TryAdd(new Sample
            {
                Metric = metric,
                Value = counter.Count,
                SampledAt = now
            });
            
            // Track conversions for any registered experiments
            var experiments = Experiments.All.Values.Where(e => e.HasMetric(metric));
            foreach(var experiment in experiments)
            {
                experiment.Conversions[experiment.Alternative]++;
                experiment.CurrentParticipant.Converted = true;
            }
        }

        private static string InternalMetric(string tag, long timestamp)
        {
            return string.Concat(Header, tag, Separator, timestamp);
        }
    }
}
