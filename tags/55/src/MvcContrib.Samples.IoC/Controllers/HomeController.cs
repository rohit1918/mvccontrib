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
        [ControllerAction]
        public void Index()
        {
            RenderView("Index");
        }

        [ControllerAction]
        public void About()
        {
            RenderView("About");
        }
    }
}
