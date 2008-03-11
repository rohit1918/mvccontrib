using System.Web.Mvc;
using MvcContrib.Samples.ObjectBuilderControllerFactory.Models;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers
{
	public class HomeController : Controller
	{
		private IService _service;

		public HomeController(IService service)
		{
			_service = service;
		}

		public void Index()
		{
			ViewData["numbers"] = _service.GetNumbers();
			RenderView("index");
		}
	}
}
