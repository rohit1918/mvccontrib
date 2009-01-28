using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerActionInvokerTester
	{
		private TestController _controller;
		private TestActionInvoker _invoker;
		private ControllerContext _context;

		[SetUp]
		public void Setup()
		{
			_invoker = new TestActionInvoker();
			_controller = new TestController(_invoker);
			_context= new ControllerContext(MvcMockHelpers.DynamicHttpContextBase(), new RouteData(), _controller);
		}

		[Test]
		public void Should_invoke_default_action()
		{
			_controller.ActionInvoker.InvokeAction(_context, "Foo");
			Assert.IsTrue(_controller.CatchAllWasCalled);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Multiple_default_actions_throw()
		{
			_controller = new TestControllerWithMultipleDefaultActions();
			_context.Controller = _controller;
			_controller.ActionInvoker.InvokeAction(_context, "Foo");
		}

		[Test]
		public void Should_find_default_action()
		{
			var descriptor = new ReflectedControllerDescriptor(typeof(DefaultActionController));
			var action = _invoker.FindAction(new ControllerContext(), descriptor, "Unknown");
			Assert.AreEqual("DefaultAction", action.ActionName);
		}

		[Test]
		public void Should_return_null_for_unknown_action()
		{
			var descriptor = new ReflectedControllerDescriptor(typeof(TestControllerWithNoDefaultActions));
			var action = _invoker.FindAction(new ControllerContext(), descriptor, "Unknown");
			Assert.IsNull(action);
		}

		private class TestActionInvoker : ConventionControllerActionInvoker
		{
			public new ActionDescriptor FindAction(ControllerContext controllerContext, ControllerDescriptor controllerDescriptor,
			                                       string actionName)
			{
				return base.FindAction(controllerContext, controllerDescriptor, actionName);
			}
		}
	}
}