using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ab
{
    public class Scoring
    {
        public static Lazy<Func<Experiment, int>> Default = new Lazy<Func<Experiment, int>>(HighestDistinctConvertingAlternative);

        private static Func<Experiment, int> HighestDistinctConvertingAlternative()
        {
            return experiment =>
            {
                if (!experiment.Participants.Any())
                {
                    return 1;
                }

                var hash = experiment.ParticipantsByAlternative();
                IEnumerator enumerator = hash.GetEnumerator();
                enumerator.MoveNext();
                var winner = (KeyValuePair<int, int>)enumerator.Current;

                foreach (var alternative in hash)
                {
                    if (alternative.Value > winner.Value)
                    {
                        winner = alternative;
                    }
                }
                return winner.Key;
            };
        }
    }
}