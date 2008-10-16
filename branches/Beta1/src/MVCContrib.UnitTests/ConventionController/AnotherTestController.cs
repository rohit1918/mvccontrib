using System.Web.Mvc;

namespace MvcContrib.UnitTests.ConventionController
{
	public class AnotherTestController : Controller
	{
		public RedirectToRouteResult RedirectActionOnSameControllerWithExtensions()
		{
			return this.RedirectToAction(c => c.SomeAction(1));
		}

		public RedirectToRouteResult RedirectActionOnAnotherControllerWithExtensions()
		{
			return this.RedirectToAction<TestController>(c => c.BasicAction(1));
		}

		public ActionResult SomeAction(int id)
		{
			return new EmptyResult();
		}
	}
}