using System;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.Samples.IoC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RenderView("Index");
        }

        
        public ActionResult About()
        {
            return RenderView("About");
        }
    }
}
