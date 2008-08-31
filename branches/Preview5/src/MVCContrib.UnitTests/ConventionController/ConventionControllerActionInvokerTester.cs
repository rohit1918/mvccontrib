using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using MvcContrib.MetaData;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;

namespace MvcContrib.UnitTests.ConventionController
{
	//TODO: Should find default action
	[TestFixture]
	public class ConventionControllerActionInvokerTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;
		private TestController _controller;
//		private ConventionControllerActionInvoker _invoker;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _controller);
			_mocks.ReplayAll();
			//_invoker = new ConventionControllerActionInvoker();
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

		/*[Test]
		public void FindActionMetaData_should_return_null_for_empty_string()
		{
			Assert.IsNull(_invoker.FindActionMetaData(string.Empty));
		}

		[Test]
		public void FindActionMetaData_should_return_null_for_null_action()
		{
			Assert.IsNull(_invoker.FindActionMetaData(null));
		}*/

		/*[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void FindActionMetaData_should_throw_for_overloaded_actions()
		{
			_invoker.FindActionMetaData("SimpleAction");
		}*/

/*
		[Test]
		public void FindActionMeta_should_return_defaultaction_the_specified_action_does_not_exist()
		{
			var controller = new DefaultActionController();

			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), controller);
			_invoker = new ConventionControllerActionInvoker()
            {
                ControllerDescriptor = new ControllerDescriptor()
            };


			var meta = _invoker.FindActionMetaData("Unknown");
			Assert.AreEqual("DefaultAction", meta.Name);
		}
*/

/*
		[Test]
		public void FindActionMeta_should_return_null_for_unknown_action()
		{
			Assert.That(_invoker.FindActionMetaData("Unknown"), Is.Null);
		}
*/

/*
		[Test]
		public void Should_set_selectedaction_on_controller()
		{
			Assert.That(_invoker.SelectedAction, Is.Null);
			_invoker.InvokeAction("BasicAction", null);
			Assert.That(_invoker.SelectedAction, Is.Not.Null);
			Assert.That(_invoker.SelectedAction.Name, Is.EqualTo("BasicAction"));
		}
*/

		[Test, Ignore("Preview 5 breaks this test - binders execute before filters.")]
		public void Filters_should_execute_before_binders()
		{
			_controller.ActionInvoker.InvokeAction(_context, "BinderFilterOrderingAction");
			string expected = "FilterBinder";
			Assert.That(_controller.BinderFilterOrdering, Is.EqualTo(expected));
		}

		
	}
}
