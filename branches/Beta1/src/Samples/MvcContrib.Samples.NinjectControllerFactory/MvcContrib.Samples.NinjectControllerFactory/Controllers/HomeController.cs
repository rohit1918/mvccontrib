using System.Web.Mvc;
using NinjectControllerFactory.Domain;

namespace MvcContrib.Samples.NinjectControllerFactory.Controllers
{
    public class HomeController : Controller
    {
        private Samurai _samurai;

        public HomeController(Samurai samurai)
        {
            _samurai = samurai;
        }

        public ActionResult Index()
        {
            ViewData["attackresult"] = _samurai.Attack("the evildoers");
            return View("index");
        }
    }
}
