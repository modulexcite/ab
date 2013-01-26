using System.Text;
using System.Web.Mvc;

namespace ab.Lab.Controllers
{
    public class ABController : Controller
    {
        public ActionResult Index()
        {
            var model = Experiments.AllSorted.Values;
            return View(model);
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
