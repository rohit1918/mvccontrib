using System.Threading;
using System.Web.Mvc;
using MvcContrib.Attributes;

namespace MvcContrib.UnitTests.ConventionController
{
	class TestController : MvcContrib.ConventionController
	{
/*
		public bool CancelAction;
*/
		public bool ActionWasCalled;
		public bool? OnErrorResult = false;
		public bool ActionExecutingCalled;
		public bool CustomActionResultCalled;
		public string BinderFilterOrdering = string.Empty;

		[TestFilter]
		public ActionResult BinderFilterOrderingAction([TestBinder] object item)
		{
			return new EmptyResult();
		}

		public ActionResult BasicAction(int id)
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1)
		{
			return new EmptyResult();
		}

		public ActionResult SimpleAction(string param1, int param2)
		{
			return new EmptyResult();
		}

		[NonAction]
		public ActionResult HiddenAction()
		{
			return new EmptyResult();
		}

		public ActionResult ComplexAction([Deserialize("ids")] int[] ids)
		{
			ActionWasCalled = true;
			return new EmptyResult();
		}

		public ActionResult CustomResult()
		{
			return new CustomActionResult();
		}

		public ActionResult BadAction()
		{
			throw new AbandonedMutexException();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ActionExecutingCalled = true;
			filterContext.Result = new EmptyResult();
		}

		public ActionResult XmlResult()
		{
			return Xml("Test 1 2 3");
		}

		public ActionResult BinaryResult()
		{
			return Binary(new byte[1], "application/ms-excel", true, "test.pdf");
		}

		public RedirectToRouteResult RedirectActionOnSameController()
		{
			return RedirectToAction<TestController>(c => c.BasicAction(1));
		}

		public RedirectToRouteResult RedirectActionOnAnotherController()
		{
			return RedirectToAction<AnotherTestController>(c => c.SomeAction(2));
		}
	}
}