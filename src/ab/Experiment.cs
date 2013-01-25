using System;

namespace ab
{
    public class Experiment : ICopyable<Experiment>
    {
        private readonly string[] _metrics;

        public string Name { get; private set; }
        public int Factors { get; set; }

        public Experiment(string name, params string[] metrics)
        {
            _metrics = metrics;
            Name = name;

            Factors = 2;
            IdentityFunction = Identity.Default;
        }
        
        public Func<string> IdentityFunction { get; set; }

        public int Group
        {
            get { return Audience.Split.Value(IdentityFunction(), Factors); }
        }

        public Experiment Copy
        {
            get { return new Experiment(Name, _metrics); }
        }
    }
}