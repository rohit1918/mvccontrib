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
        
        public void Index()
        {
            RenderView("Index");
        }

        
        public void About()
        {
            RenderView("About");
        }
    }
}
