using System;
using System.Linq;
using ServiceStack.Text;

namespace ab
{
    /// <summary>
    /// A central repository for registering experiments with available metrics. Both experiments and metrics are loosely referenced by name.
    /// </summary>
    public static class Experiments 
    {
        private static readonly ExperimentRepository ExperimentRepository;

        static Experiments()
        {
            JsConfig.EmitLowercaseUnderscoreNames = true;
            JsConfig.PropertyConvention = JsonPropertyConvention.Lenient;
            ExperimentRepository = new ExperimentRepository();
        }

        public static string Json()
        {
            return JsonSerializer.SerializeToString(new 
            {
                experiments = ViewModelMapper.ProjectExperiments(ExperimentRepository.GetAll().OrderByDescending(e => e.CreatedAt)),
                metrics = ViewModelMapper.ProjectMetrics(metrics.Metrics.AllSorted)
            });
        }
        
        public static void Register(string name, string description, Func<string> identify = null, Func<Experiment, bool> conclude = null, Func<Experiment, int> score = null, Func<string, int, int> splitOn = null, object[] alternatives = null, params string[] metrics)
        {
            var experiment = new Experiment(name, description, identify, conclude, score, splitOn, alternatives, metrics);
            ExperimentRepository.Save(experiment);
            M.CounterFor(metrics.Where(m => m != null).Select(m => m.Trim()));
        }

        public static Experiment Get(string name)
        {
            return ExperimentRepository.GetByName(name);
        }
    }
}