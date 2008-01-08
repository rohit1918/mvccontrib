using System.Web.Mvc;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers
{
	public class HomeController : Controller
	{
		[ControllerAction]
		public void Index()
		{
			RenderView("index");
		}
	}
}