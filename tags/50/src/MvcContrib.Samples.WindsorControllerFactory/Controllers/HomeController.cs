using System.Web.Mvc;

namespace MvcContrib.Samples.WindsorControllerFactory.Controllers
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