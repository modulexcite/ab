using System;
using System.Collections.Generic;
using System.Linq;
using metrics.Core;

namespace ab
{
    internal class ViewModelMapper
    {
        public class ExperimentViewModel
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Type { get; set; }
            public string CreatedAt { get; set; }
            public IEnumerable<AlternativeViewModel> Alternatives { get; set; }
            public bool Active { get; set; }
            public DateTime? ConcludedAt { get; set; }
        }
        public class AlternativeViewModel
        {
            public string Name { get; set; }
            public string Value { get; set; }
            public int Participants { get; set; }
            public int Converted { get; set; }
            public double ConversionRate { get; set; }
            public bool Showing { get; set; }
            public bool Choice { get; set; }
        }
        public class MetricViewModel
        {
            public string Name { get; set; }
            public IDictionary<long, List<Sample>> Samples { get; set; }
        }

        private static readonly SampleRepository SampleRepository;
        static ViewModelMapper()
        {
            SampleRepository = new SampleRepository();
        }

        public static IEnumerable<ExperimentViewModel> ProjectExperiments(IEnumerable<Experiment> allExperiments)
        {
            return allExperiments.Select(experiment => new ExperimentViewModel
            {
                Name = experiment.Name,
                Description = experiment.Description,
                Type = "(A/B Test)",
                CreatedAt = experiment.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                Alternatives = ProjectAlternatives(experiment),
                Active = !experiment.ConcludedAt.HasValue,
                ConcludedAt = experiment.ConcludedAt
            });
        }

        public static IEnumerable<AlternativeViewModel> ProjectAlternatives(Experiment experiment)
        {
            var index = 1;
            foreach (var alternative in experiment.Alternatives)
            {
                var vm = new AlternativeViewModel();
                vm.Name = "Option " + (char)(index + 64);
                vm.Value = alternative.ToString();
                vm.Participants = experiment.ParticipantsByAlternative()[index];
                vm.Converted = experiment.ConvertedByAlternative()[index];
                vm.ConversionRate = vm.Participants > 0 ? (vm.Converted / (double)vm.Participants) * 100 : 0;
                vm.Showing = experiment.AlternativeValue.ToString() == vm.Value;
                vm.Choice = experiment.Score(experiment) == index;
                index++;

                yield return vm;
            }
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
    }
}