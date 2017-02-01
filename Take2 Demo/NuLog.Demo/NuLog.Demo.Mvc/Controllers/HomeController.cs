using System.Web.Mvc;

namespace NuLog.Demo.Mvc.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILogger _logger = LogManager.GetLogger();

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            _logger.Log(string.Format("Request to \"{0}\"", Request.Url));

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}