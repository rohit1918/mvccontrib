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
	[TestFixture]
	public class ConventionControllerActionInvokerTester
	{
		private MockRepository _mocks;
		private ControllerContext _context;
		private TestController _controller;
		private ConventionControllerActionInvoker _invoker;

		[SetUp]
		public void Setup()
		{
			_mocks = new MockRepository();
			_controller = new TestController();
			_context = new ControllerContext(_mocks.DynamicHttpContextBase(), new RouteData(), _controller);
			_mocks.ReplayAll();
			_invoker = new ConventionControllerActionInvoker(_context);
		}

		[Test]
		public void When_instantiated_the_Controller_property_should_be_set()
		{
			Assert.That(_invoker.ControllerContext.Controller, Is.SameAs(_controller));
		}

		[Test]
		public void FindActionMetaData_should_return_null_for_empty_string()
		{
			Assert.IsNull(_invoker.FindActionMetaData(string.Empty));
		}

		[Test]
		public void FindActionMetaData_should_return_null_for_null_action()
		{
			Assert.IsNull(_invoker.FindActionMetaData(null));
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvokeAction_should_throw_for_empty_action()
		{
			_invoker.InvokeAction(string.Empty, null);
		}

		[Test]
		[ExpectedException(typeof(ArgumentException))]
		public void InvokeAction_should_throw_for_null_action()
		{
			_invoker.InvokeAction(null, null);
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
			_invoker = new ConventionControllerActionInvoker(_context)
            {
                ControllerDescriptor = new ControllerDescriptor()
            };


			var meta = _invoker.FindActionMetaData("Unknown");
			Assert.AreEqual("DefaultAction", meta.Name);
		}

		[Test]
		public void FindActionMeta_should_return_null_for_unknown_action()
		{
			Assert.That(_invoker.FindActionMetaData("Unknown"), Is.Null);
		}

		[Test]
		public void Should_set_selectedaction_on_controller()
		{
			Assert.That(_invoker.SelectedAction, Is.Null);
			_invoker.InvokeAction("BasicAction", null);
			Assert.That(_invoker.SelectedAction, Is.Not.Null);
			Assert.That(_invoker.SelectedAction.Name, Is.EqualTo("BasicAction"));
		}

		[Test]
		public void Invoke_should_return_false_for_invalid_action()
		{
			Assert.That(_invoker.InvokeAction("Unknown", null), Is.False);
		}

		[Test]
		public void Valid_action_Returns_true()
		{
			Assert.IsTrue(_invoker.InvokeAction("ComplexAction", null));
			Assert.IsTrue(_controller.ActionWasCalled);
		}

		[Test]
		public void ActionExecuting_should_be_called()
		{
			_invoker.InvokeAction("ComplexAction", null);
			Assert.IsTrue(_controller.ActionExecutingCalled);
		}

		[Test, ExpectedException(typeof(TargetInvocationException))]
		public void Bad_action_throws()
		{
			_invoker.InvokeAction("BadAction", null);
		}

		[Test]
		public void HiddenAction_returns_false()
		{
			Assert.IsFalse(_invoker.InvokeAction("HiddenAction", null));
		}

		[Test]
		public void Valid_action_returns_false_when_OnActionExecuting_cancels_action()
		{
			_controller.CancelAction = true;
			_invoker.InvokeAction("ComplexAction", null);
			Assert.IsFalse(_controller.ActionWasCalled);
		}

		[Test]
		public void Custom_result_should_execute()
		{
			Assert.That(_controller.CustomActionResultCalled, Is.False);
			_invoker.InvokeAction("CustomResult", null);
			Assert.That(_controller.CustomActionResultCalled, Is.True);

		}

		[Test]
		public void Filters_should_execute_before_binders()
		{
			_invoker.InvokeAction("BinderFilterOrderingAction", null);
			string expected = "FilterBinder";
			Assert.That(_controller.BinderFilterOrdering, Is.EqualTo(expected));
		}
	}
}
