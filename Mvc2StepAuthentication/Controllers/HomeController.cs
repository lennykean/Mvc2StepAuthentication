using System.Web.Mvc;

namespace Mvc2StepAuthentication.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
