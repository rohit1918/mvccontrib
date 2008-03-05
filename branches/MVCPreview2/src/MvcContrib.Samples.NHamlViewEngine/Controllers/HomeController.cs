using System.Web.Mvc;

namespace MvcContrib.Samples.NHamlViewEngine.Controllers
{
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