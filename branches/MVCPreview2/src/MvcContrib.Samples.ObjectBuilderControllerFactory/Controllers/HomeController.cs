using System.Web.Mvc;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers
{
	public class HomeController : Controller
	{
		
		public void Index()
		{
			RenderView("index");
		}
	}
}
