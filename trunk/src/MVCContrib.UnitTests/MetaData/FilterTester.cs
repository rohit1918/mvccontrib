using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Collections.Specialized;
using Castle.Windsor;
using MvcContrib.Castle;
using MvcContrib.Filters;
using MvcContrib.MetaData;
using MvcContrib.Services;
using MvcContrib.UnitTests.IoC;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.MetaData
{
	[TestFixture]
	public class FilterTester
	{
		private FilteredController _controller;
		private MockRepository _mocks;

		[SetUp]
		public void Setup()
		{
			_controller = new FilteredController();
			_mocks = new MockRepository();
		}

		private void SetupHttpContext(Controller controller, string requestType)
		{
			RouteData fakeRouteData = new RouteData();
			fakeRouteData.Values.Add("Action", "Index");
			fakeRouteData.Values.Add("Controller", "Home");

			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();
			HttpResponseBase reponse = _mocks.DynamicMock<HttpResponseBase>();

			SetupResult.For(context.Request).Return(request);
			SetupResult.For(request.RequestType).Return(requestType);
			SetupResult.For(request.Params).Return(new NameValueCollection());

			_mocks.Replay(context);
			_mocks.Replay(request);
			_mocks.Replay(reponse);

			//request.Params = new NameValueCollection();

			ControllerContext controllerContext = new ControllerContext(context, fakeRouteData, _controller);
			controller.ControllerContext = controllerContext;
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void PostOnlyShouldReturnFalseIfRequestTypeIsNotPost()
		{
			SetupHttpContext(_controller, "GET");
			_controller.DoInvokeAction("PostOnly");
		}

		[Test]
		public void PostOnlyShouldReturnTrueIfRequestTypeIsPost()
		{
			SetupHttpContext(_controller, "POST");

			bool result = _controller.DoInvokeAction("PostOnly");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.PostOnlyCalled);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void PredicatePreconditionShouldThrowSpecifiedExceptionOnInvalidRouteDataPrecondition()
		{
			SetupHttpContext(_controller, "POST");
			_controller.RouteData.Values.Add("id", "0"); //invalid id
			_controller.DoInvokeAction("PredicatePreconditionRouteData");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void PredicatePreconditionShouldThrowSpecifiedExceptionOnInvalidRequestPrecondition()
		{
			SetupHttpContext(_controller, "POST");
			_controller.Request.Params.Add("id", "0"); //invalid id
			_controller.DoInvokeAction("PredicatePreconditionRequest");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void PredicatePreconditionShouldThrowSpecifiedExceptionOnMissingRouteDataParameter()
		{
			SetupHttpContext(_controller, "POST");
			_controller.DoInvokeAction("PredicatePreconditionRouteData");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void PredicatePreconditionShouldThrowSpecifiedExceptionOnMissingRequestParameter()
		{
			SetupHttpContext(_controller, "POST");
			_controller.DoInvokeAction("PredicatePreconditionRequest");
		}

		[Test]
		public void PredicatePreconditionShouldNotThrowSpecifiedExceptionOnValidRouteDataPrecondition()
		{
			SetupHttpContext(_controller, "POST");
			_controller.RouteData.Values.Add("id", "1"); //valid id
			_controller.DoInvokeAction("PredicatePreconditionRouteData");

			Assert.IsTrue(_controller.PredicatePreconditionCalled);
		}

		[Test]
		public void PredicatePreconditionShouldNotThrowSpecifiedExceptionOnValidRequestPrecondition()
		{
			SetupHttpContext(_controller, "POST");
			_controller.Request.Params.Add("id", "1"); //valid id
			_controller.DoInvokeAction("PredicatePreconditionRequest");

			Assert.IsTrue(_controller.PredicatePreconditionCalled);
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RegExPreconditionShouldThrowSpecifiedExceptionOnInvalidRouteDataRegEx()
		{
			SetupHttpContext(_controller, "POST");
			_controller.RouteData.Values.Add("id", "0"); //invalid id
			_controller.DoInvokeAction("RegExPreconditionRouteData");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RegExPreconditionShouldThrowSpecifiedExceptionOnInvalidRequestRegEx()
		{
			SetupHttpContext(_controller, "POST");
			_controller.Request.Params.Add("id", "0"); //invalid id
			_controller.DoInvokeAction("RegExPreconditionRequest");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RegExPreconditionShouldThrowSpecifiedExceptionOnMissingRouteDataParameter()
		{
			SetupHttpContext(_controller, "POST");
			_controller.DoInvokeAction("RegExPreconditionRouteData");
		}

		[Test, ExpectedException(typeof(ArgumentOutOfRangeException))]
		public void RegExPreconditionShouldThrowSpecifiedExceptionOnMissingRequestParameter()
		{
			SetupHttpContext(_controller, "POST");
			_controller.DoInvokeAction("RegExPreconditionRequest");
		}

		[Test]
		public void RegExPreconditionShouldNotThrowSpecifiedExceptionOnValidRouteDataRegEx()
		{
			SetupHttpContext(_controller, "POST");
			_controller.RouteData.Values.Add("id", "1"); //valid id
			_controller.DoInvokeAction("RegExPreconditionRouteData");

			Assert.IsTrue(_controller.RegExPreconditionCalled);
		}

		[Test]
		public void RegExPreconditionShouldNotThrowSpecifiedExceptionOnValidRequestRegEx()
		{
			SetupHttpContext(_controller, "POST");
			_controller.Request.Params.Add("id", "1"); //valid id
			_controller.DoInvokeAction("RegExPreconditionRequest");

			Assert.IsTrue(_controller.RegExPreconditionCalled);
		}

		[Test]
		public void ActionShouldNotBeInvokedIfOneFilterReturnsTrueAndAnotherReturnsFalse()
		{
			SetupHttpContext(_controller, "GET");

			bool result = _controller.DoInvokeAction("MultipleFilters");

			Assert.IsTrue(result);
			Assert.IsFalse(_controller.MultipleFiltersCalled);
		}

		[Test]
		public void ActionShouldBeInvokedIfFilterReturnsTrue()
		{
			SetupHttpContext(_controller, "GET");

			bool result = _controller.DoInvokeAction("SuccessfulFilter");

			Assert.IsTrue(result);
			Assert.IsTrue(_controller.SuccessfulFilterCalled);
		}

		[Test]
		public void ActionShouldNotBeInvokedIfFilterReturnsFalse()
		{
			SetupHttpContext(_controller, "GET");

			bool result = _controller.DoInvokeAction("UnsuccessfulFilter");

			Assert.IsTrue(result);
			Assert.IsFalse(_controller.UnSuccessfulFilterCalled);
		}

		class FilterReturnsTrue : ActionFilterAttribute
		{
			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				filterContext.Cancel = false;
			}
		}

		class FilterReturnsFalse : ActionFilterAttribute
		{
			public override void OnActionExecuting(ActionExecutingContext filterContext)
			{
				filterContext.Cancel = true;
			}
		}

		[FilterReturnsTrue]
		class FilteredController : Controller
		{
			public bool SuccessfulFilterCalled = false;
			public bool UnSuccessfulFilterCalled = false;
			public bool MultipleFiltersCalled = false;
			public bool PostOnlyCalled = false;
			public bool DependentFilterCalled = false;
			public bool PredicatePreconditionCalled = false;
			public bool RegExPreconditionCalled = false;

			public bool DoInvokeAction(string action)
			{
				ActionInvoker = new ConventionControllerActionInvoker(ControllerContext);
				return ActionInvoker.InvokeAction(action, null);
			}

			[FilterReturnsTrue]
			public ActionResult SuccessfulFilter()
			{
				SuccessfulFilterCalled = true;
				return new EmptyResult();
			}

			[FilterReturnsFalse]
			public ActionResult UnsuccessfulFilter()
			{
				UnSuccessfulFilterCalled = true;
				return new EmptyResult();
			}

			[FilterReturnsTrue(Order = 1)]
			[FilterReturnsFalse(Order = 100)]
			public ActionResult MultipleFilters()
			{
				MultipleFiltersCalled = true;
				return new EmptyResult();
			}

			[PostOnly]
			public ActionResult PostOnly()
			{
				PostOnlyCalled = true;
				return new EmptyResult();
			}

			[PredicatePreconditionFilter("id", PreconditionFilter.ParamType.RouteData, "IsGreaterThanZero", typeof(ArgumentOutOfRangeException))]
			public ActionResult PredicatePreconditionRouteData()
			{
				PredicatePreconditionCalled = true;
				return new EmptyResult();
			}

			[PredicatePreconditionFilter("id", PreconditionFilter.ParamType.Request, "IsGreaterThanZero", typeof(ArgumentOutOfRangeException))]
			public ActionResult PredicatePreconditionRequest()
			{
				PredicatePreconditionCalled = true;
				return new EmptyResult();
			}

			[RegExPreconditionFilter("id", PreconditionFilter.ParamType.RouteData, "^[1-9][0-9]*$", typeof(ArgumentOutOfRangeException))]
			public ActionResult RegExPreconditionRouteData()
			{
				RegExPreconditionCalled = true;
				return new EmptyResult();
			}

			[RegExPreconditionFilter("id", PreconditionFilter.ParamType.Request, "^[1-9][0-9]*$", typeof(ArgumentOutOfRangeException))]
			public ActionResult RegExPreconditionRequest()
			{
				RegExPreconditionCalled = true;
				return new EmptyResult();
			}

			//for PredicatePreconditionFilter test
			protected bool IsGreaterThanZero(object value)
			{
				try
				{
					int id = Convert.ToInt32(value);
					return id > 0;
				}
				catch
				{
					return false;
				}
			}

		}
	}
}
