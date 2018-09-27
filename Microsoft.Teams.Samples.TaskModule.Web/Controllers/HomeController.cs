using System.Web.Mvc;

namespace Microsoft.Teams.Samples.TaskModule.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("tasks")]
        public ActionResult Tasks()
        {
            return View();
        }

        [Route("customform")]
        public ActionResult CustomForm()
        {
            return View();
        }

        [Route("youtube")]
        public ActionResult YouTube()
        {
            return View();
        }

        [Route("powerapp")]
        public ActionResult PowerApp()
        {
            return View();
        }

    }
}
