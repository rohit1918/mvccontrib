using System.Web.Mvc;

namespace MvcContrib.Samples.ObjectBuilderControllerFactory.Controllers
{
	public class ObjectBuilderController : IController
	{
		public void Execute(ControllerContext controllerContext)
		{
            controllerContext.HttpContext.Response.Write("ObjectBuilder");
		}
	}
}