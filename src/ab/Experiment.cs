using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ab
{
    public class Experiment
    {
        private readonly HashSet<string> _metrics;
        private readonly object[] _alternatives;
        private readonly ConcurrentDictionary<string, Participant> _participants;
        
        /// <summary>
        /// The list of unique metric names being tracked
        /// </summary>
        public IEnumerable<string> Metrics
        {
            get { return _metrics; }
        }

        /// <summary>
        /// All experiment alternative values
        /// </summary>
        public IEnumerable<object> Alternatives
        {
            get { return _alternatives; }
        }

        /// <summary>
        /// All known participants (for whom an alternative has been shown)
        /// </summary>
        public IEnumerable<Participant> Participants
        {
            get { return _participants.Values; }
        }

        /// <summary>
        /// The unique name of this experiment, which acts as a natural key
        /// </summary>
        public string Name { get; private set; }
        
        /// <summary>
        /// The description of the experiment as registered; this is not persisted in any backing store
        /// </summary>
        public string Description { get; private  set; }
        
        /// <summary>
        /// The function used to identify a user for cohort splits; by default, <code>Identify.Default</code> is used
        /// </summary>
        public Func<string> Identify { get; private set; }
        
        /// <summary>
        /// The function used to decide when to conclude an experiment; by default, <code>Conclusion.Default</code> is used
        /// </summary>
        public Func<Experiment, bool> Conclude { get; private set; }

        /// <summary>
        /// The function used to decide the current winning alternative; by default, <code>Scoring.Default</code> is used
        /// </summary>
        public Func<Experiment, int> Score { get; private set; }

        /// <summary>
        /// The function used to decide what alternative to show to an identified participant; by default, <code>Audient.Default</code> is used
        /// </summary>
        public Func<string, int, int> SplitOn { get; private set; }

        public int? Outcome { get; private set; }
        public DateTime CreatedAt { get; internal set; }
        public DateTime? ConcludedAt { get; private set; }
        
        protected internal Experiment(string name, string description, Func<string> identify = null, Func<Experiment, bool> conclude = null, Func<Experiment, int> score = null, Func<string, int, int> splitOn = null, object[] alternatives = null, params string[] metrics)
        {
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
            
            _alternatives = alternatives ?? new object[] { true, false };
            _metrics = new HashSet<string>(metrics);
            
            Identify = identify ?? ab.Identify.Default.Value;
            Conclude = conclude ?? Conclusion.Default.Value;
            Score = score ?? Scoring.Default.Value;
            SplitOn = splitOn ?? Audience.Default.Value;

            _participants = new ConcurrentDictionary<string, Participant>();
        }
        
        /// <summary>
        /// Forces the selection of the given alternative for the current participant;
        /// </summary>
        /// <param name="alternative"></param>
        public void Choose(int alternative)
        {
            if(CurrentParticipant.Shown.HasValue && CurrentParticipant.Shown.Value == alternative)
            {
                return;
            }
            CurrentParticipant.Seen = 0;
            CurrentParticipant.Conversions = 0;
            CurrentParticipant.Shown = alternative;
        }

        /// <summary>
        /// Forces a conclusion to this experiment, irrespective of the <see cref="Conclude"/> function
        /// </summary>
        public void End()
        {
            ConcludedAt = DateTime.Now;
            Outcome = Score(this);
        }

        /// <summary>
        /// Retrieves the current participant; if none found, they are added to the cohort
        /// </summary>
        internal Participant CurrentParticipant
        {
            get { return EnsureParticipant(Identify()); }
        }

        /// <summary>
        /// The value of the alternative for the current participant </summary>
        internal object AlternativeValue
        {
            get { return _alternatives[AlternativeIndex - 1]; }
        }

        /// <summary>
        /// The index of the alternative for the current participant
        /// </summary>
        internal int AlternativeIndex
        {
            get
            {
                if (Outcome.HasValue)
                {
                    return Outcome.Value;
                }
                
                var identity = Identify();
                var participant = EnsureParticipant(identity);

                int alternative;
                if(participant.Shown.HasValue)
                {
                    alternative = participant.Shown.Value;
                }
                else
                {
                    alternative = SplitOn(identity, Alternatives.Count());
                    participant.Shown = alternative;
                }
                
                if(Conclude(this))
                {
                    End();
                }

                return alternative;
            }
        }
        
        internal bool HasMetric(string metric)
        {
            return _metrics.Contains(metric);
        }

        internal Dictionary<int, int> EmptyHash()
        {
            var hash = new Dictionary<int, int>();
            for (var i = 1; i <= Alternatives.Count(); i++)
            {
                hash.Add(i, 0);
            }
            return hash;
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
    }
}