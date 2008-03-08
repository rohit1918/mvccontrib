using System.Web.Mvc;

namespace MvcContrib.Samples.WindsorControllerFactory.Controllers
{
	public class WindsorController : IController
	{
		public void Execute(ControllerContext controllerContext)
		{
			controllerContext.HttpContext.Response.Write("Windsor");
		}
	}
}