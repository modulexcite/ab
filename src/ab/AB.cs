namespace ab
{
    public static class AB
    {
        public static bool IsControl(string experiment)
        {
            return Group(experiment) == 1;
        }

        public static int Group(string experiment)
        {
            var exp = Experiments.All[new ExperimentKey(experiment)];
            return exp == null ? 1 : exp.Group;
        }
    }
}