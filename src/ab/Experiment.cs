using System;

namespace ab
{
    public class Experiment : ICopyable<Experiment>
    {
        private readonly string[] _metrics;
        private readonly object[] _alternatives;

        public string Name { get; private set; }
        public string Description { get; set; }
        public Func<string> Identify { get; set; }
        public int Alternatives
        {
            get { return _alternatives.Length; }
        }
        
        public Experiment(string name, string description, object[] alternatives = null, params string[] metrics)
        {
            _alternatives = alternatives ?? new object[] { true, false };
            _metrics = metrics;
            Name = name;
            Description = description;
            Identify = Identity.Get.Value;
        }
        
        public Experiment Copy
        {
            get { return new Experiment(Name, Description, _alternatives, _metrics); }
        }

        public object Current
        {
            get { return this[Group]; }
        }
        
        public object this[int group]
        {
            get { return _alternatives[group - 1]; }
        }

        public int Group
        {
            get
            {
                // Track that the alternative was visited by the current identity
                
                // Force a group if using Chooses() method

                // Map bucket to an alternative object, i.e. int -> value
                
                return Audience.Split.Value(Identify(), Alternatives);
            }
        }
    }
}