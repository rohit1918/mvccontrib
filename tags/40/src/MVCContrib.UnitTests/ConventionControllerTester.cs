using System;
using System.Collections.Specialized;
using System.Reflection;
using System.Threading;
using System.Web.Mvc;
using MvcContrib;
using MvcContrib.Attributes;
using MvcContrib.MetaData;
using NUnit.Framework;
using Rhino.Mocks;
using System.Web;

namespace MVCContrib.UnitTests
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
			SetupHttpContext();
		}

		private void SetupHttpContext()
		{
			RouteData fakeRouteData = new RouteData();
			fakeRouteData.Values.Add("Action", "Index");
			fakeRouteData.Values.Add("Controller", "Home");

			IHttpContext context = _mocks.DynamicMock<IHttpContext>();
			IHttpRequest request = _mocks.DynamicMock<IHttpRequest>();

			SetupResult.For(context.Request).Return(request);
			SetupResult.For(request.QueryString).Return(new NameValueCollection());
			SetupResult.For(request.Form).Return(new NameValueCollection());

			_mocks.Replay(context);
			_mocks.Replay(request);

			ControllerContext controllerContext = new ControllerContext(context, fakeRouteData, _controller);
			_controller.ControllerContext = controllerContext;
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
		public void ValidActionReturnsTrue()
		{
			Assert.IsTrue(_controller.DoInvokeAction("ComplexAction"));
			Assert.IsTrue(_controller.ActionWasCalled);
		}

		[Test]
		public void ValidActionReturnsFalseWhenOnPreActionReturnsFalse()
		{
			_controller.OnPreActionReturnValue = false;
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

		class TestController : ConventionController
		{
			public bool OnPreActionReturnValue = true;
			public bool ActionWasCalled = false;
			public bool OnErrorWasCalled = false;

			public void BasicAction(int id)
			{
			}

			public void SimpleAction(string param1)
			{
			}

			public void SimpleAction(string param1, int param2)
			{
			}

			public void ComplexAction([Deserialize("ids")] int[] ids)
			{
				ActionWasCalled = true;
			}

			public void BadAction()
			{
				throw new AbandonedMutexException();
			}

			protected override bool OnPreAction(string actionName, MethodInfo methodInfo)
			{
				return OnPreActionReturnValue;
			}

			public bool DoInvokeAction(string action)
			{
				return InvokeAction(action);
			}

			public void DoInvokeActionMethod(ActionMetaData action)
			{
				InvokeActionMethod(action);
			}

			public void InvokeOnErrorWithoutInnerException()
			{
				Exception e = new Exception("Blah");
				OnError(MetaData.GetAction("BasicAction"), e);
			}

			protected override bool OnError(ActionMetaData action, Exception exception)
			{
				bool result = base.OnError(action, exception);
				OnErrorWasCalled = true;
				return result;
			}
		}
	}
}
