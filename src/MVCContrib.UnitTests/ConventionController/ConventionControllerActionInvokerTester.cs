using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ConventionController
{
	[TestFixture]
	public class ConventionControllerActionInvokerTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;
		private TestController _controller;
		private TestActionInvoker _invoker;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_invoker = new TestActionInvoker();
			_controller = new TestController(_invoker);
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _controller);
			_invoker.SetContext(_context);
			_mocks.ReplayAll();
		}

		[Test]
		public void Should_find_default_action()
		{
			_controller.ActionInvoker.InvokeAction(_context, "Foo");
			Assert.IsTrue(_controller.CatchAllWasCalled);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void Multiple_default_actions_throw()
		{
			_controller = new TestControllerWithMultipleDefaultActions();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _controller);
			_controller.ActionInvoker.InvokeAction(_context, "Foo");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void FindActionMetaData_should_return_null_for_empty_string()
		{
			_invoker.FindActionMetaData(string.Empty);
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]		
		public void FindActionMetaData_should_return_null_for_null_action()
		{
			_invoker.FindActionMetaData(null);
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FindActionMetaData_should_throw_for_overloaded_actions()
		{
			_invoker.FindActionMetaData("SimpleAction");
		}

		[Test]
		public void FindActionMeta_should_return_defaultaction_the_specified_action_does_not_exist()
		{
			var controller = new DefaultActionController();

			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_invoker = new TestActionInvoker();
			_invoker.SetContext(_context);

			var meta = _invoker.FindActionMetaData("Unknown");
			Assert.AreEqual("DefaultAction", meta.Name);
		}

		[Test]
		public void FindActionMeta_should_return_null_for_unknown_action()
		{
			var controller = new TestControllerWithNoDefaultActions();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_invoker = new TestActionInvoker();
            _invoker.SetContext(_context);

            
			Assert.That(_invoker.FindActionMetaData("Unknown"), Is.Null);
		}

		[Test]
		public void Should_set_selectedaction()
		{
		    //_invoker.ControllerContext.Controller.ValueProvider = MockRepository.GenerateMock<IValueProvider>();
			Assert.That(_invoker.SelectedAction, Is.Null);
            
			_invoker.InvokeAction(_context, "BasicAction");
			Assert.That(_invoker.SelectedAction, Is.Not.Null);
			Assert.That(_invoker.SelectedAction.Name, Is.EqualTo("BasicAction"));
		}

		private class TestActionInvoker : ConventionControllerActionInvoker
		{
            
			public void SetContext(ControllerContext ctx)
			{
				//ControllerContext = ctx;
			}
		}

	}

}
