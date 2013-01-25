using System;

namespace ab
{
    public class Experiment : ICopyable<Experiment>
    {
        private readonly string[] _metrics;

        public string Name { get; private set; }
        public string Description { get; set; }
        public Func<string> IdentityFunction { get; set; }
        public int Alternatives { get; set; }
        
        public Experiment(string name, string description, params string[] metrics)
        {
            _metrics = metrics;
            Name = name;
            Description = description;
            IdentityFunction = Identity.Get.Value;
            Alternatives = 2;
        }
        
        public Experiment Copy
        {
            get { return new Experiment(Name, Description, _metrics); }
        }

        public int Group
        {
            get { return Audience.Split.Value(IdentityFunction(), Alternatives); }
        }
    }
}