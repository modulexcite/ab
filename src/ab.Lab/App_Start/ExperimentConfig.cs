namespace ab.Lab
{
    public class ExperimentConfig
    {
        public static void Register()
        {
            Experiments.Register(
                name: "Jokes on link", 
                description: "Testing to prove that more people will click the link if there's a joke on it.",
                metrics: new [] { "Button clicks" },                    // Associates ticks against the "Button clicks" counter with this experiment
                alternatives: new object[] { true, false },             // Typed experiment alternatives ; default is common "A/B" binary case
                conclude: experiment => experiment.Participants == 10,  // Optional criteria for automatically concluding an experiment; default is never
                choose: null /* ... */                                  // Optional criteria for choosing best performer by index; default is best converting alternative
            );
        }
    }
}