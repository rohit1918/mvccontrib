using System.Web.Mvc;

namespace MvcContrib.Samples.NHamlViewEngine.Controllers
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
