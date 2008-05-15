using System;
using System.Web;
using System.Web.Mvc;
using StructureMap;

namespace MvcContrib.Samples.IoC.Controllers
{
    [PluginFamily("Default")]
    [Pluggable("Default")]
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
