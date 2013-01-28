using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ab
{
    public class SampleRepository
    {
        internal static IProducerConsumerCollection<Sample> Samples { get; set; }

        static SampleRepository()
        {
            Samples = new ConcurrentBag<Sample>();
        }

        public void Save(Sample sample)
        {
            Samples.TryAdd(sample);
        }

        public IDictionary<long, List<Sample>> GetAllHashedByDay()
        {
            var hash = new Dictionary<long, List<Sample>>();
            foreach(var entry in Samples)
            {
                var day = entry.SampledAt.Date.ToUnixTime();
                List<Sample> samples;
                if(!hash.TryGetValue(day, out samples))
                {
                    samples = new List<Sample>();
                    hash.Add(day, samples);
                }
                samples.Add(entry);
            }
            return hash;
        }
    }
}