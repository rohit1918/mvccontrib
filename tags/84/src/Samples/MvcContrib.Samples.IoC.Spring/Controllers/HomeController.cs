using System;
using System.Web;
using System.Web.Mvc;

namespace MvcContrib.Samples.IoC.Controllers
{
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
