using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ab
{
    public class ExperimentRepository
    {
        private static readonly ConcurrentDictionary<ExperimentKey, Experiment> Inner = new ConcurrentDictionary<ExperimentKey, Experiment>();
        
        public void Convert(string metric)
        {
            foreach (var experiment in GetByMetric(metric))
            {
                experiment.CurrentParticipant.Conversions++;
                experiment.CurrentParticipant.Seen++;
            }
        }

        public void Save(Experiment experiment)
        {
            var key = new ExperimentKey(experiment.Name);
            Inner.AddOrUpdate(key, experiment, (n, m) => m);
        }

        public IEnumerable<Experiment> GetAll()
        {
            return Inner.Values;
        }

        public IEnumerable<Experiment> GetByMetric(string metric)
        {
            foreach (var item in Inner.Values)
            {
                if (item.HasMetric(metric))
                {
                    yield return item;
                }
            }
        }

        public Experiment GetByName(string experiment)
        {
            foreach (var item in Inner.Values)
            {
                if (item.Name == experiment)
                {
                    return item;
                }
            }
            return null;
        }
    }
}