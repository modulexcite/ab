using System.Text;
using System.Web.Mvc;

namespace ab.Lab.Controllers
{
    public class MetricsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Metrics()
        {
            var result = new ContentResult
            {
                Content = ab.Metrics.Json(),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
            return result;
        }
    }
}