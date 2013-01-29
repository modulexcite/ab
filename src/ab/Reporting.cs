using System.Collections.Generic;

namespace ab
{
    public class Reporting
    {
        public static Report Score(double probablity = 90.0)
        {
            return new Report();
        }

        public static string[] Conclusion(Report report)
        {
            return new string[0];
        }

        public class Report
        {
            public IEnumerable<Alternative> Alternatives { get; private set; }
            public Alternative Choice { get; set; }
            public Alternative Base { get; set; }
            public Alternative Least { get; set; }
        }

        // http://stackoverflow.com/questions/5336457/how-to-calculate-a-standard-deviation-array
        public class Alternative
        {
            public double ZScore { get; set; }
            public double Probability { get; set; }
            public double Difference { get; set; }
        }
    }
}

//# -- Reporting --

//     # Scores alternatives based on the current tracking data. This method
//     # returns a structure with the following attributes:
//     # [:alts] Ordered list of alternatives, populated with scoring info.
//     # [:base] Second best performing alternative.
//     # [:least] Least performing alternative (but more than zero conversion).
//     # [:choice] Choice alterntive, either the outcome or best alternative.
//     #
//     # Alternatives returned by this method are populated with the following
//     # attributes:
//     # [:z_score] Z-score (relative to the base alternative).
//     # [:probability] Probability (z-score mapped to 0, 90, 95, 99 or 99.9%).
//     # [:difference] Difference from the least performant altenative.
//     #
//     # The choice alternative is set only if its probability is higher or
//     # equal to the specified probability (default is 90%).
//     def score(probability = 90)
//       alts = alternatives
//       # sort by conversion rate to find second best and 2nd best
//       sorted = alts.sort_by(&:measure)
//       base = sorted[-2]
//       # calculate z-score
//       pc = base.measure
//       nc = base.participants
//       alts.each do |alt|
//         p = alt.measure
//         n = alt.participants
//         alt.z_score = (p - pc) / ((p * (1-p)/n) + (pc * (1-pc)/nc)).abs ** 0.5
//         alt.probability = AbTest.probability(alt.z_score)
//       end
//       # difference is measured from least performant
//       if least = sorted.find { |alt| alt.measure > 0 }
//         alts.each do |alt|
//           if alt.measure > least.measure
//             alt.difference = (alt.measure - least.measure) / least.measure * 100
//           end
//         end
//       end
//       # best alternative is one with highest conversion rate (best shot).
//       # choice alternative can only pick best if we have high probability (>90%).
//       best = sorted.last if sorted.last.measure > 0.0
//       choice = outcome ? alts[outcome.id] : (best && best.probability >= probability ? best : nil)
//       Struct.new(:alts, :best, :base, :least, :choice).new(alts, best, base, least, choice)
//     end

//     # Use the result of #score to derive a conclusion. Returns an
//     # array of claims.
//     def conclusion(score = score)
//       claims = []
//       participants = score.alts.inject(0) { |t,alt| t + alt.participants }
//       claims << case participants
//         when 0 ; "There are no participants in this experiment yet."
//         when 1 ; "There is one participant in this experiment."
//         else ; "There are #{participants} participants in this experiment."
//       end
//       # only interested in sorted alternatives with conversion
//       sorted = score.alts.select { |alt| alt.measure > 0.0 }.sort_by(&:measure).reverse
//       if sorted.size > 1
//         # start with alternatives that have conversion, from best to worst,
//         # then alternatives with no conversion.
//         sorted |= score.alts
//         # we want a result that's clearly better than 2nd best.
//         best, second = sorted[0], sorted[1]
//         if best.measure > second.measure
//           diff = ((best.measure - second.measure) / second.measure * 100).round
//           better = " (%d%% better than %s)" % [diff, second.name] if diff > 0
//           claims << "The best choice is %s: it converted at %.1f%%%s." % [best.name, best.measure * 100, better]
//           if best.probability >= 90
//             claims << "With %d%% probability this result is statistically significant." % score.best.probability
//           else
//             claims << "This result is not statistically significant, suggest you continue this experiment."
//           end
//           sorted.delete best
//         end
//         sorted.each do |alt|
//           if alt.measure > 0.0
//             claims << "%s converted at %.1f%%." % [alt.name.gsub(/^o/, "O"), alt.measure * 100]
//           else
//             claims << "%s did not convert." % alt.name.gsub(/^o/, "O")
//           end
//         end
//       else
//         claims << "This experiment did not run long enough to find a clear winner."
//       end
//       claims << "#{score.choice.name.gsub(/^o/, "O")} selected as the best alternative." if score.choice
//       claims
//     end