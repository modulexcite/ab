using System.Text;
using System.Web.Mvc;

namespace ab.Lab.Controllers
{
    public class ABController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ContentResult Experiments()
        {
            var result = new ContentResult
            {
                Content = ab.Experiments.Json(),
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
            return result;
        }

        public ActionResult Metrics()
        {
            var result = new ContentResult
            {
                Content = "TODO",
                ContentType = "application/json",
                ContentEncoding = Encoding.UTF8
            };
            return result;
        }
    }
}
