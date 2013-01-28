using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.Text;
using metrics;

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
            var experiments = ViewModelMapper.ProjectExperiments(ExperimentRepository.GetAll().OrderByDescending(e => e.CreatedAt));
            var metrics = ViewModelMapper.ProjectMetrics(Metrics.AllSorted);

            return JsonSerializer.SerializeToString(new { experiments, metrics });
        }
        
        public static void Register(string name, string description, Func<string> identify = null, Func<Experiment, bool> conclude = null, Func<Experiment, int> choose = null, object[] alternatives = null, params string[] metrics)
        {
            var experiment = new Experiment(name, description, identify, conclude, choose, alternatives, metrics);
            ExperimentRepository.Save(experiment);
            M.CounterFor(metrics.Where(m => m != null).Select(m => m.Trim()));
        }
    }
}