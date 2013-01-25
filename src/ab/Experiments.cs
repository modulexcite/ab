using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ab
{
    public class Experiments
    {
        private static readonly ConcurrentDictionary<ExperimentKey, Experiment> Inner = new ConcurrentDictionary<ExperimentKey, Experiment>();

        public static IDictionary<ExperimentKey, Experiment> All
        {
            get { return new ReadOnlyDictionary<ExperimentKey, Experiment>(Inner); }
        }

        public static IDictionary<ExperimentKey, Experiment> AllSorted
        {
            get { return new ReadOnlyDictionary<ExperimentKey, Experiment>(new SortedDictionary<ExperimentKey, Experiment>(Inner)); }
        }

        public static Experiment Register(string name, params string[] metrics)
        {
            return GetOrAdd(new ExperimentKey(name), new Experiment(name, metrics));
        }

        private static T GetOrAdd<T>(ExperimentKey name, T experiment) where T : Experiment
        {
            Experiment value;
            if (Inner.TryGetValue(name, out value))
            {
                return (T)value;
            }
            var added = Inner.AddOrUpdate(name, experiment, (n, m) => m);
            return added == null ? experiment : (T)added;
        }
    }
}