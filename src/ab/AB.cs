using System.Web;

namespace ab
{
    public static class AB
    {
        /// <summary>
        /// Returns whether the current identity is part of the control group; this is synonymous with being in the first experiment group
        /// </summary>
        /// <param name="experiment"></param>
        /// <returns></returns>
        public static bool IsControl(string experiment)
        {
            return Group(experiment) == 1;
        }

        /// <summary>
        /// Returns the value of the current identity's experiment group
        /// </summary>
        /// <param name="experiment"></param>
        /// <returns></returns>
        public static int Group(string experiment)
        {
            var exp = Experiments.All[new ExperimentKey(experiment)];
            return exp == null ? 1 : exp.Group;
        }

        /// <summary>
        /// Returns the value of the current experiment choice, as determined by the experiment identity
        /// </summary>
        /// <param name="experiment"></param>
        /// <returns></returns>
        public static IHtmlString Value(string experiment)
        {
            var exp = Experiments.All[new ExperimentKey(experiment)];
            var choice = exp == null ? "?" : exp.Current;
            return new HtmlString(choice.ToString());
        }
    }
}