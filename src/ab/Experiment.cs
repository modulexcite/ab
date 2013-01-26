using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ab
{
    public class Experiment : ICopyable<Experiment>
    {
        private readonly HashSet<string> _metrics;
        private readonly object[] _alternatives;

        public string Name { get; private set; }
        public string Description { get; set; }
        public Func<string> Identify { get; set; }
        public int Alternatives
        {
            get { return _alternatives.Length; }
        }

        public int? Outcome { get; set; }
        public DateTime? ConcludedAt { get; set; }
        public Func<Experiment, bool> Conclude { get; set; }
        public Func<Experiment, int> Choose { get; set; }

        private readonly IDictionary<string, Participant> _participants; 
        public IEnumerable<Participant> Participants
        {
            get { return _participants.Values; }
        }

        internal IDictionary<int, int> Conversions { get; private set; }
        
        protected internal Experiment(string name, string description, object[] alternatives = null, params string[] metrics)
        {
            Name = name;
            Description = description;
            
            _alternatives = alternatives ?? new object[] { true, false };
            _metrics = new HashSet<string>(metrics);
            
            Identify = Identity.Get.Value;
            Conclude = experiment => false;
            Choose = HighestDistinctConvertingAlternative();

            _participants = new ConcurrentDictionary<string, Participant>();
            
            Conversions = new ConcurrentDictionary<int, int>();
            for(var i = 1; i <= Alternatives; i++)
            {
                Conversions.Add(i, 0);
            }
        }

        internal bool HasMetric(string metric)
        {
            return _metrics.Contains(metric);
        }

        private Func<Experiment, int> HighestDistinctConvertingAlternative()
        {
            return experiment =>
            {
                if (_participants.Count == 0)
                {
                    return 1;
                }
                var hash = new Dictionary<int, int>();
                foreach (var participant in _participants.Values)
                {
                    if (!participant.Shown.HasValue)
                    {
                        continue;
                    }
                    int alternative;
                    if (!hash.TryGetValue(participant.Shown.Value, out alternative))
                    {
                        hash.Add(alternative, 1);
                    }
                    else
                    {
                        hash[alternative]++;
                    }
                }
                var winner = hash.First();
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

        public Experiment Copy
        {
            get { return new Experiment(Name, Description, _alternatives, _metrics.ToArray()); }
        }

        public object CurrentAlternative
        {
            get { return _alternatives[Alternative - 1]; }
        }

        public Participant CurrentParticipant
        {
            get { return EnsureParticipant(Identify()); }
        }

        internal int Alternative
        {
            get
            {
                if (Outcome.HasValue)
                {
                    return Outcome.Value;
                }
                
                var identity = Identify();

                var participant = EnsureParticipant(identity);

                if(participant.Shown.HasValue)
                {
                    return participant.Shown.Value;
                }

                var alternative = Audience.Split.Value(identity, Alternatives);
                participant.Shown = alternative;
                
                if(Conclude(this))
                {
                    ConcludedAt = DateTime.Now;
                    Outcome = Choose(this);
                }

                return alternative;
            }
        }

        private Participant EnsureParticipant(string identity)
        {
            Participant participant;
            if (!_participants.TryGetValue(identity, out participant))
            {
                participant = new Participant {Identity = identity};
                _participants.Add(identity, participant);
            }
            return participant;
        }
    }
}