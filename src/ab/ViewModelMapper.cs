using System;
using System.Collections.Generic;
using System.Linq;
using metrics.Core;

namespace ab
{
    public class ExperimentViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string CreatedAt { get; set; }
        public IEnumerable<AlternativeViewModel> Alternatives { get; set; }
    }
    public class AlternativeViewModel
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public int Participants { get; set; }
        public int Converted { get; set; }
        public double ConversionRate { get; set; }
    }
    public class MetricViewModel
    {
        public string Name { get; set; }
        public IDictionary<long, List<Sample>> Samples { get; set; } 
    }

    internal class ViewModelMapper
    {
        private static readonly SampleRepository SampleRepository;
        static ViewModelMapper()
        {
            SampleRepository = new SampleRepository();
        }

        public static IEnumerable<MetricViewModel> ProjectMetrics(IEnumerable<KeyValuePair<MetricName, IMetric>> allMetrics)
        {
            var samples = SampleRepository.GetAllHashedByDay();
            
            return allMetrics.Select(metric =>
            {
                var name = metric.Key.Name.Replace(M.Header, "");
                var splitOn = name.IndexOf(M.Separator, StringComparison.Ordinal);
                var day = name.Substring(splitOn + 2);
                name = name.Substring(0, splitOn);
                var vm = new MetricViewModel
                {
                    Name = name,
                    Samples = samples
                };
                return vm;
            });
        }

        public static IEnumerable<ExperimentViewModel> ProjectExperiments(IEnumerable<Experiment> allExperiments)
        {
            return allExperiments.Select(experiment => new ExperimentViewModel
            {
                Name = experiment.Name,
                Description = experiment.Description,
                Type = "(A/B Test)",
                CreatedAt = experiment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Alternatives = ProjectAlternatives(experiment)
            });
        }

        public static IEnumerable<AlternativeViewModel> ProjectAlternatives(Experiment experiment)
        {
            var index = 1;
            foreach (var alternative in experiment._alternatives)
            {
                var vm = new AlternativeViewModel();
                vm.Name = "Option " + index;
                vm.Value = alternative.ToString();
                vm.Participants = experiment.ParticipantsByAlternative[index];
                vm.Converted = experiment.ConvertedByAlternative[index];
                vm.ConversionRate = vm.Participants > 0 ? (vm.Converted / (double)vm.Participants) * 100 : 0;
                index++;

                yield return vm;
            }
        }
    }
}