using System;
using System.Collections.Generic;
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
        private static readonly MetricRepository MetricRepository;

        static Experiments()
        {
            JsConfig.EmitLowercaseUnderscoreNames = true;
            JsConfig.PropertyConvention = JsonPropertyConvention.Lenient;

            ExperimentRepository = new ExperimentRepository();
            MetricRepository = new MetricRepository();
        }

        public static string Json()
        {
            return JsonSerializer.SerializeToString(new 
            {
                experiments = ViewModelMapper.ProjectExperiments(ExperimentRepository.GetAll().OrderByDescending(e => e.CreatedAt)),
                /*metrics = ViewModelMapper.ProjectMetrics(metrics.Metrics.AllSorted)*/
            });
        }

        public static void Register(string name, string description, Func<string> identify = null, Func<Experiment, bool> conclude = null, Func<Experiment, IOrderedEnumerable<KeyValuePair<int, double>>> score = null, Func<string, int, int> splitOn = null, object[] alternatives = null, params string[] metrics)
        {
            var experiment = new Experiment(name, description, identify, conclude, score, splitOn, alternatives, metrics);
            ExperimentRepository.Save(experiment);

            foreach(var entry in experiment.Metrics)
            {
                var metric = MetricRepository.GetByName(entry);
                if(metric == null)
                {
                    metric = new InMemoryMetric(entry);
                    MetricRepository.Save(metric);
                }

                metric.Hook += (sender, args) =>
                {
                    if (!experiment.IsActive)
                    {
                        return;
                    }

                    experiment.CurrentParticipant.Conversions++;
                    experiment.CurrentParticipant.Seen++;
                    var index = experiment.AlternativeIndex;
                    //def track!(metric_id, timestamp, count, *args)
                    //   return unless active?
                    //   identity = identity() rescue nil
                    //   if identity
                    //     return if connection.ab_showing(@id, identity)
                    //     index = alternative_for(identity)
                    //     connection.ab_add_conversion @id, index, identity, count
                    //     check_completion!
                    //   end
                    // end
                };
            }
        }

        public static Experiment Get(string name)
        {
            return ExperimentRepository.GetByName(name);
        }
    }
}