namespace ab.Lab
{
    public class ExperimentConfig
    {
        public static void Register()
        {
            Experiments.Register(
                name: "Jokes on link", 
                description: "Testing to prove that more people will click the link if there's a joke on it.",
                alternatives: new object[] { true, false }, // This is the default, common "A/B" case
                metrics: new [] { "Button clicks" }
            );
        }
    }
}