using System;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using MvcContrib.ActionResults;
using MvcContrib.Attributes;
using MvcContrib.MetaData;

namespace MvcContrib.UnitTests.ConventionController
{
	class TestController : MvcContrib.ConventionController
	{
		public bool CancelAction = false;
		public bool ActionWasCalled = false;
		public bool OnErrorWasCalled = false;
		public bool? OnErrorResult = null;
		public bool ReturnBinderInvoked = false;
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
			filterContext.Cancel = CancelAction;
		}

		public ActionResult XmlResult()
		{
			return Xml("Test 1 2 3");
		}

	}
}
