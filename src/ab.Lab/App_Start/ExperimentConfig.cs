namespace ab.Lab
{
    public class ExperimentConfig
    {
        public static void Register()
        {
            Experiments.Register(
                name: "Jokes on link", 
                description: "Testing to prove that more people will click the link if there's a joke on it.",
                metrics: new [] { "Button clicks" }
            );
        }
    }
}