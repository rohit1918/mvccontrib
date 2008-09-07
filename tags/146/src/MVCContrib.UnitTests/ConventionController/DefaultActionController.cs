using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
	class DefaultActionController : Controller
	{
		public bool DefaultActionCalled;

        [DefaultAction]
		public ActionResult DefaultAction()
		{
			DefaultActionCalled = true;
			return new EmptyResult();
		}
	}
}
