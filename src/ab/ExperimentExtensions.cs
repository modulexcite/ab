using System.Collections.Generic;

namespace ab
{
    internal static class ExperimentExtensions
    {
        public static Dictionary<int, int> ParticipantsByAlternative(this Experiment experiment)
        {
            var hash = experiment.EmptyHash();
            foreach (var participant in experiment.Participants)
            {
                if (!participant.Shown.HasValue)
                {
                    continue;
                }
                hash[participant.Shown.Value]++;
            }
            return hash;
        }

        public static IDictionary<int, int> ConvertedByAlternative(this Experiment experiment)
        {
            var hash = experiment.EmptyHash();
            foreach (var participant in experiment.Participants)
            {
                if (!participant.Shown.HasValue)
                {
                    continue;
                }
                if (participant.Converted)
                {
                    hash[participant.Shown.Value]++;
                }
            }
            return hash;
        }

        public static IDictionary<int, int> ConversionsByAlternative(this Experiment experiment)
        {
            var hash = experiment.EmptyHash();
            foreach (var participant in experiment.Participants)
            {
                if (!participant.Shown.HasValue)
                {
                    continue;
                }
                hash[participant.Shown.Value] += participant.Conversions;
            }
            return hash;
        }
    }
}