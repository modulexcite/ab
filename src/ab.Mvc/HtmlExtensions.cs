using System;
using System.Web;
using System.Web.Mvc;

namespace ab.Mvc
{
    public static class HtmlExtensions
    {
        public static IHtmlString AB(this HtmlHelper helper, string experiment)
        {
            var exp = Experiments.All[new ExperimentKey(experiment)];
            Console.WriteLine(exp.Group);
            return new MvcHtmlString(exp.Group.ToString());
        }
    }
}