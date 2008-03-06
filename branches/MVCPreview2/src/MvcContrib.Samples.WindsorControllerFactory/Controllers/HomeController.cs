using System.Web.Mvc;

namespace MvcContrib.Samples.WindsorControllerFactory.Controllers
{
	public class HomeController : Controller
	{
		
		public void Index()
		{
			RenderView("index");
		}
	}
}
