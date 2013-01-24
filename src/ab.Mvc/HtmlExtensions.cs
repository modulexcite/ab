using System;
using System.Web.Mvc;

namespace ab.Mvc
{
    public static class HtmlExtensions
    {
        public static MvcExperiment AB(this HtmlHelper helper, string experiment)
        {
            var split = Audience.SplitTwo.Value(Identity.Default());
            Console.WriteLine(split);
            return new MvcExperiment();
        }
    }

    public class MvcExperiment : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
}