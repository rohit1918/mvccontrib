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
			Assert.That(_invoker.SelectedAction, Is.Null);
			_invoker.InvokeAction(_context, "BasicAction");
			Assert.That(_invoker.SelectedAction, Is.Not.Null);
			Assert.That(_invoker.SelectedAction.Name, Is.EqualTo("BasicAction"));
		}

		[Test]
		public void When_binding_a_non_nullable_value_type_and_no_value_is_specified_then_the_default_for_that_type_should_be_returned()
		{
			_invoker.InvokeAction(_context, "BasicAction");
			Assert.That(_controller.BasicActionResult, Is.EqualTo(0));
		}

		[Test]
		public void The_binder_should_only_be_replaced_if_the_default_binder_is_the_DefaultModelBinder()
		{
			ModelBinders.DefaultBinder = new BinderStub();
			_invoker.InvokeAction(_context, "BasicAction");
			Assert.That(_controller.BasicActionResult, Is.EqualTo(5));
		}

		[Test, Ignore("Preview 5 breaks this test - binders execute before filters.")]
		public void Filters_should_execute_before_binders()
		{
			_controller.ActionInvoker.InvokeAction(_context, "BinderFilterOrderingAction");
			string expected = "FilterBinder";
			Assert.That(_controller.BinderFilterOrdering, Is.EqualTo(expected));
		}

		[TearDown]
		public void Teardown()
		{
			ModelBinders.DefaultBinder = null;
		}

		private class TestActionInvoker : ConventionControllerActionInvoker
		{
			public void SetContext(ControllerContext ctx)
			{
				ControllerContext = ctx;
			}
		}

		private class BinderStub : DefaultModelBinder 
		{
			public override object GetValue(ControllerContext controllerContext, string modelName, Type modelType, ModelStateDictionary modelState) 
			{
				if(modelType == typeof(int))
				{
					return 5;
				}
				return base.GetValue(controllerContext, modelName, modelType, modelState);
			}
		}

	}

}
