/*
using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web;

namespace MvcContrib.UnitTests
{
	[TestFixture]
	public class ConventionControllerTester
	{
		private TestController _controller;
		private MockRepository _mocks;

		[SetUp]
		public void SetUp()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
			_controller.ControllerDescriptor = new ControllerDescriptor();
			SetupHttpContext(_controller);
		}

		private void SetupHttpContext(Controller controller)
		{
			RouteData fakeRouteData = new RouteData();
			fakeRouteData.Values.Add("Action", "Index");
			fakeRouteData.Values.Add("Controller", "Home");

			HttpContextBase context = _mocks.DynamicMock<HttpContextBase>();
			HttpRequestBase request = _mocks.DynamicMock<HttpRequestBase>();
			HttpResponseBase response = _mocks.DynamicMock<HttpResponseBase>();

			SetupResult.For(context.Request).Return(request);
			SetupResult.For(context.Response).Return(response);
			SetupResult.For(request.QueryString).Return(new NameValueCollection());
			SetupResult.For(request.Form).Return(new NameValueCollection());

			_mocks.Replay(context);
			_mocks.Replay(request);

			ControllerContext controllerContext = new ControllerContext(context, fakeRouteData, _controller);
			controller.ControllerContext = controllerContext;
		}

		[Test]
		[ExpectedException(typeof(TargetInvocationException))]
		public void BadActionThrows()
		{
			_controller.DoInvokeAction("BadAction");
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void EmptyActionThrows()
		{
			_controller.DoInvokeAction(string.Empty);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void NullActionThrows()
		{
			_controller.DoInvokeAction(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void OverloadedActionsThrow()
		{
			_controller.DoInvokeAction("SimpleAction");
		}

		[Test]
		public void UnknownActionReturnsFalse()
		{
			Assert.IsFalse(_controller.DoInvokeAction("Unknown"));
		}

		[Test]
		public void UnknownActionCallsDefaultAction()
		{
			DefaultActionController controller = new DefaultActionController();
			SetupHttpContext(controller);
			controller.ControllerDescriptor = new ControllerDescriptor();

			Assert.IsTrue(controller.DoInvokeAction("Unknown"));
			Assert.IsTrue(controller.DefaultActionCalled);
			Assert.AreEqual("DefaultAction", controller.SelectedAction.Name);
		}

		[Test]
		public void ValidActionReturnsTrue()
		{
			Assert.IsTrue(_controller.DoInvokeAction("ComplexAction"));
			Assert.IsTrue(_controller.ActionWasCalled);
		}

		[Test]
		public void HiddenActionReturnsFalse()
		{
			Assert.IsFalse(_controller.DoInvokeAction("HiddenAction"));
		}

		[Test]
		public void ValidActionReturnsFalseWhenOnPreActionReturnsFalse()
		{
			_controller.CancelAction = true;
			_controller.DoInvokeAction("ComplexAction");
			Assert.IsFalse(_controller.ActionWasCalled);
		}

		[Test]
		public void ControllerDescriptorDefaultsToCached()
		{
			TestController controller = new TestController();
			Assert.IsNotNull(controller.ControllerDescriptor);
			Assert.AreEqual(typeof(CachedControllerDescriptor), controller.ControllerDescriptor.GetType());
		}

		[Test]
		public void OnErrorShouldNotThrowIfInnerExceptionIsNull()
		{
			TestController controller = new TestController();
			controller.InvokeOnErrorWithoutInnerException();
			Assert.IsTrue(controller.OnErrorWasCalled);
		}

		[Test]
		public void ShouldNotBeRescuedWhenThreadAbortExceptionThrownDueToRedirect()
		{
			Expect.Call(delegate { _controller.Response.Redirect(null); });
			_mocks.Replay(_controller.Response);
			_controller.DoInvokeAction("WithRedirect");
			_mocks.Verify(_controller.Response);

			Assert.IsTrue(_controller.OnErrorWasCalled);
			Assert.IsTrue(_controller.OnErrorResult.Value);
		}

		[Test]
		public void ReturnBinderShouldBeInvoked()
		{
			_controller.DoInvokeAction("ReturnBinder");
			Assert.IsTrue(_controller.ReturnBinderInvoked);
		}
	}
}
*/
