using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace ab.Lab
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            Experiments.Register("Foo");

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}