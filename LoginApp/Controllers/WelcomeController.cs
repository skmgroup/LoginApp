using System.Web.Mvc;

namespace LoginApp.Controllers
{
    [Authorize]
    public class WelcomeController : Controller
    {
        public ActionResult Index() => View();
    }
}