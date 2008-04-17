using System;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
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

		/*public  WithRedirect()
		{
			RedirectToAction("ComplexAction");
			//Simulate RedirectToAction throwing a ThreadAbortException
			ConstructorInfo ctor = typeof(ThreadAbortException).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
			ThreadAbortException exc = (ThreadAbortException)ctor.Invoke(Type.EmptyTypes);
			throw exc;
		}*/

		public ActionResult BadAction()
		{
			throw new AbandonedMutexException();
		}

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			ActionExecutingCalled = true;
			filterContext.Cancel = CancelAction;
		}

		public ActionResult InvokeOnErrorWithoutInnerException()
		{
			Exception e = new Exception("Blah");
			OnError(MetaData.GetAction("BasicAction"), e);

			return new EmptyResult();
		}

		protected override bool OnError(ActionMetaData action, Exception exception)
		{
			bool result = base.OnError(action, exception);
			OnErrorWasCalled = true;
			OnErrorResult = result;
			return result;
		}
	}
}