using System;
using System.Collections.Generic;
using metrics;
using metrics.Core;

namespace ab
{
    public class M 
    {
        private static readonly SampleRepository SampleRepository;
        private static readonly ExperimentRepository ExperimentRepository;

        internal const string Separator = "__";
        internal const string Header = "__m__track__";
        
        static M()
        {
            SampleRepository = new SampleRepository();
            ExperimentRepository = new ExperimentRepository();
        }

        public static void Track(string metric, int increment = 1)
        {
            if (increment <= 0) return;

            var counter = CounterFor(metric);
            counter.Increment(increment);

            SampleRepository.Save(new Sample
            {
                Metric = metric,
                Value = counter.Count,
                SampledAt = DateTime.UtcNow
            });
            
            ExperimentRepository.Convert(metric);
        }

        internal static void CounterFor(IEnumerable<string> metrics)
        {
            foreach(var metric in metrics)
            {
                CounterFor(metric);
            }
        }

        internal static CounterMetric CounterFor(string metric, long? today = null)
        {
            var counter = Metrics.Counter(typeof(M), InternalMetric(metric, today ?? DateTime.Today.ToUnixTime()));
            return counter;
        }

        private static string InternalMetric(string tag, long timestamp)
        {
            return string.Concat(Header, tag, Separator, timestamp);
        }
    }
}
