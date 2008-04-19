using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
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

			SetupResult.For(context.Request).Return(request);
			SetupResult.For(request.RequestType).Return(requestType);

			_mocks.Replay(context);
			_mocks.Replay(request);

			ControllerContext controllerContext = new ControllerContext(context, fakeRouteData, _controller);
			controller.ControllerContext = controllerContext;
		}

		[Test]
		public void ControllerDescriptorShouldFindFilters()
		{
			ControllerDescriptor descriptor = new ControllerDescriptor();
			ControllerMetaData metaData = descriptor.GetMetaData(_controller);
			ActionMetaData action = metaData.GetAction("MultipleFilters");

			Assert.AreEqual(3, action.Filters.Count);
			Assert.AreEqual(-1, action.Filters[0].Order);
			Assert.AreEqual(1, action.Filters[1].Order);
			Assert.AreEqual(100, action.Filters[2].Order);
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
		class FilteredController : MvcContrib.ConventionController
		{
			public bool SuccessfulFilterCalled = false;
			public bool UnSuccessfulFilterCalled = false;
			public bool MultipleFiltersCalled = false;
			public bool PostOnlyCalled = false;
			public bool DependentFilterCalled = false;

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
		}
	}
}
