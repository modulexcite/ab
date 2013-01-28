using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ab
{
    public class Experiment : ICopyable<Experiment>
    {
        private readonly HashSet<string> _metrics;
        protected internal readonly object[] _alternatives;

        public string Name { get; private set; }
        public string Description { get; set; }
        public int Alternatives
        {
            get { return _alternatives.Length; }
        }

        public Func<string> Identify { get; private set; }
        public Func<Experiment, bool> Conclude { get; private set; }
        public Func<Experiment, int> Choose { get; private set; }

        public int? Outcome { get; private set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime? ConcludedAt { get; private set; }
        
        private readonly ConcurrentDictionary<string, Participant> _participants; 
        public int Participants
        {
            get { return _participants.Count; }
        }
        public Dictionary<int, int> ParticipantsByAlternative
        {
            get
            {
                var hash = new Dictionary<int, int>();
                for (var i = 1; i <= Alternatives; i++)
                {
                    hash.Add(i, 0);
                }

                foreach (var participant in _participants.Values)
                {
                    if (!participant.Shown.HasValue)
                    {
                        continue;
                    }
                    hash[participant.Shown.Value]++;
                }
                return hash;
            }
        }
        
        public IDictionary<int, int> ConvertedByAlternative
        {
            get
            {
                var hash = new Dictionary<int, int>();
                for (var i = 1; i <= Alternatives; i++)
                {
                    hash.Add(i, 0);
                }

                foreach (var participant in _participants.Values)
                {
                    if (!participant.Shown.HasValue)
                    {
                        continue;
                    }
                    if(participant.Converted)
                    {
                        hash[participant.Shown.Value]++;
                    }
                }
                return hash;
            }
        }

        internal IDictionary<int, int> ConversionsByAlternative
        {
            get
            {
                var hash = new Dictionary<int, int>();
                for (var i = 1; i <= Alternatives; i++)
                {
                    hash.Add(i, 0);
                }

                foreach (var participant in _participants.Values)
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
        
        protected internal Experiment(string name, string description, Func<string> identify = null, Func<Experiment, bool> conclude = null, Func<Experiment, int> choose = null, object[] alternatives = null, params string[] metrics)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            
            _alternatives = alternatives ?? new object[] { true, false };
            _metrics = new HashSet<string>(metrics);
            
            Identify = identify ?? Identity.Get.Value;
            Conclude = conclude ?? (experiment => false);
            Choose = choose ?? HighestDistinctConvertingAlternative();

            _participants = new ConcurrentDictionary<string, Participant>();
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
                
                var hash = ParticipantsByAlternative;
                IEnumerator enumerator = hash.Keys.GetEnumerator();
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

        public object AlternativeValue
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
                _participants.TryAdd(identity, participant);
            }
            return participant;
        }

        public Experiment Copy
        {
            get { return new Experiment(Name, Description, Identify, Conclude, Choose, _alternatives, ToArray(_metrics)); }
        }

        public string[] ToArray(HashSet<string> set)
        {
            var array = new string[set.Count];
            var i = 0;
            foreach(var item in set)
            {
                array[i] = item;
                i++;
            }
            return array;
        }
    }
}